//
//  ClassMappingHelper.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2012 Craig Fowler
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CSF.Collections.Serialization.MappingModel;
using CSF.Entities;
using CSF.Reflection;
using System.Reflection;

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Implementation of a mapping helper for class objects.
  /// </summary>
  public class ClassMappingHelper<TObject> : MappingHelper<IClassMapping<TObject>>, IClassMappingHelper<TObject>
    where TObject : class
  {
    #region methods

    /// <summary>
    ///  Specifies a factory-function to use when creating instances of the mapped class, instead of the default
    /// parameterless constructor. 
    /// </summary>
    /// <param name='factoryFunction'>
    ///  The factory method/function. 
    /// </param>
    public void UsingFactory(Func<TObject> factoryFunction)
    {
      this.Mapping.FactoryMethod = factoryFunction;
    }

    /// <summary>
    ///  Facilitates the setting of a key-naming-policy into the associated mapping. 
    /// </summary>
    /// <returns>
    ///  The current mapping helper instance, to facilitate chaining of methods. 
    /// </returns>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    public IClassMappingHelper<TObject> NamingPolicy<TPolicy>()
      where TPolicy : IKeyNamingPolicy
    {
      this.Mapping.AttachKeyNamingPolicy<TPolicy>();
      return this;
    }

    /// <summary>
    ///  Facilitates the setting of a key-naming-policy into the associated mapping. 
    /// </summary>
    /// <returns>
    ///  The current mapping helper instance, to facilitate chaining of methods. 
    /// </returns>
    /// <param name='factoryMethod'>
    ///  A custom factory method to use when constructing the naming policy. 
    /// </param>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    public IClassMappingHelper<TObject> NamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy
    {
      this.Mapping.AttachKeyNamingPolicy<TPolicy>(factoryMethod);
      return this;
    }

    #endregion

    #region mapping this instance as something

    /// <summary>
    ///  Maps the class using an association with a single value within the collection. 
    /// </summary>
    public ISimpleMappingHelper<TObject, TObject> Simple()
    {
      this.Mapping.MapAs = new SimpleMapping<TObject>(this.Mapping, null);

      return new SimpleMappingHelper<TObject, TObject>((ISimpleMapping<TObject>) this.Mapping.MapAs);
    }

    /// <summary>
    ///  Maps the class using an association with multiple values within the collection. 
    /// </summary>
    public ICompositeMappingHelper<TObject, TObject> Composite()
    {
      this.Mapping.MapAs = new CompositeMapping<TObject>(this.Mapping, null);

      return new CompositeMappingHelper<TObject, TObject>((ICompositeMapping<TObject>) this.Mapping.MapAs);
    }

    /// <summary>
    /// Maps the class as a special case of the <c>Value&lt;TValue&gt;()</c> mapping - in which the value within the
    /// collection is parsed as an <see cref="CSF.Entities.IIdentity"/> and unwrapped as an entity instance.
    /// </summary>
    /// <typeparam name='TEntity'>
    /// The entity-type for the current instance.
    /// </typeparam>
    /// <typeparam name='TIdentity'>
    /// The identity-type for the current instance.
    /// </typeparam>
    public IEntityMappingHelper<TObject, TEntity, TIdentity> Entity<TEntity, TIdentity>()
      where TEntity : IEntity
    {
      this.Mapping.MapAs = new SimpleMapping<TEntity>(this.Mapping, null);

      return new EntityMappingHelper<TObject, TEntity, TIdentity>((ISimpleMapping<TEntity>) this.Mapping.MapAs);
    }

    #endregion

    #region adding mappings

    /// <summary>
    ///  Maps a property of the class using an association with a single value within the collection. 
    /// </summary>
    /// <param name='property'>
    ///  An expression indicating the property to be mapped. 
    /// </param>
    /// <typeparam name='TValue'>
    ///  The type of the property's value. 
    /// </typeparam>
    public ISimpleMappingHelper<TObject, TValue> Simple<TValue>(Expression<Func<TObject, TValue>> property)
    {
      ISimpleMapping<TValue> mapping;
      PropertyInfo prop = Reflect.Property<TObject,TValue>(property);

      mapping = (ISimpleMapping<TValue>) this.Mapping.Mappings.Where(x => x.Property == prop).FirstOrDefault();
      if(mapping == null)
      {
        mapping = new SimpleMapping<TValue>(this.Mapping, prop);
        this.Mapping.Mappings.Add(mapping);
      }

      return new SimpleMappingHelper<TObject, TValue>(mapping);
    }

    /// <summary>
    ///  Maps a property of the class using an association with multiple values within the collection. 
    /// </summary>
    /// <param name='property'>
    ///  An expression indicating the property to be mapped. 
    /// </param>
    /// <typeparam name='TValue'>
    ///  The type of the property's value. 
    /// </typeparam>
    public ICompositeMappingHelper<TObject, TValue> Composite<TValue>(Expression<Func<TObject, TValue>> property)
    {
      ICompositeMapping<TValue> mapping;
      PropertyInfo prop = Reflect.Property<TObject,TValue>(property);

      mapping = (ICompositeMapping<TValue>) this.Mapping.Mappings.Where(x => x.Property == prop).FirstOrDefault();
      if(mapping == null)
      {
        mapping = new CompositeMapping<TValue>(this.Mapping, prop);
        this.Mapping.Mappings.Add(mapping);
      }

      return new CompositeMappingHelper<TObject, TValue>(mapping);
    }

    /// <summary>
    ///  Maps a property of the class as a collection of class-like/reference-type instances. 
    /// </summary>
    /// <param name='property'>
    ///  An expression indicating the property to be mapped. 
    /// </param>
    /// <param name='mapping'>
    ///  A method body that contains the mapping definition for each item within the collection. 
    /// </param>
    /// <typeparam name='TCollectionItem'>
    ///  The type of the items within the collection. 
    /// </typeparam>
    public void Collection<TCollectionItem>(Expression<Func<TObject,ICollection<TCollectionItem>>> property,
                                            Action<IReferenceTypeCollectionMappingHelper<TObject, TCollectionItem>> mapping)
      where TCollectionItem : class
    {
      IReferenceTypeCollectionMapping<TCollectionItem> baseMapping;
      PropertyInfo prop = Reflect.Property<TObject,ICollection<TCollectionItem>>(property);

      baseMapping = (IReferenceTypeCollectionMapping<TCollectionItem>) this.Mapping.Mappings.Where(x => x.Property == prop).FirstOrDefault();
      if(baseMapping == null)
      {
        baseMapping = new ReferenceTypeCollectionMapping<TCollectionItem>(this.Mapping, prop);
        this.Mapping.Mappings.Add(baseMapping);
      }

      mapping(new ReferenceTypeCollectionMappingHelper<TObject, TCollectionItem>(baseMapping));
    }

    /// <summary>
    ///  Maps a property of the class as a collection of struct-like/value-type instances. 
    /// </summary>
    /// <param name='property'>
    ///  An expression indicating the property to be mapped. 
    /// </param>
    /// <param name='mapping'>
    ///  A method body that contains the mapping definition for each item within the collection. 
    /// </param>
    /// <typeparam name='TCollectionItem'>
    ///  The type of the items within the collection. 
    /// </typeparam>
    public void ValueCollection<TCollectionItem>(Expression<Func<TObject,ICollection<TCollectionItem>>> property,
                                                 Action<IValueTypeCollectionMappingHelper<TObject, TCollectionItem>> mapping)
      where TCollectionItem : struct
    {
      IValueTypeCollectionMapping<TCollectionItem> baseMapping;
      PropertyInfo prop = Reflect.Property<TObject,ICollection<TCollectionItem>>(property);

      baseMapping = (IValueTypeCollectionMapping<TCollectionItem>) this.Mapping.Mappings.Where(x => x.Property == prop).FirstOrDefault();
      if(baseMapping == null)
      {
        baseMapping = new ValueTypeCollectionMapping<TCollectionItem>(this.Mapping, prop);
        this.Mapping.Mappings.Add(baseMapping);
      }

      mapping(new ValueTypeCollectionMappingHelper<TObject, TCollectionItem>(baseMapping));
    }

    /// <summary>
    ///  Maps a property of the class as a complex class (must be a class/reference type) with a mapping of its own. 
    /// </summary>
    /// <param name='property'>
    ///  An expression indicating the property to be mapped. 
    /// </param>
    /// <param name='mapping'>
    ///  A method body that contains the mapping definition for the property. 
    /// </param>
    /// <typeparam name='TClass'>
    ///  The type of the property that the new mappings will match to. 
    /// </typeparam>
    public void Class<TClass>(Expression<Func<TObject, TClass>> property,
                              Action<IClassMappingHelper<TClass>> mapping)
      where TClass : class
    {
      IClassMapping<TClass> baseMapping;
      PropertyInfo prop = Reflect.Property<TObject,TClass>(property);

      baseMapping = (IClassMapping<TClass>) this.Mapping.Mappings.Where(x => x.Property == prop).FirstOrDefault();
      if(baseMapping == null)
      {
        baseMapping = new ClassMapping<TClass>(this.Mapping, prop);
        this.Mapping.Mappings.Add(baseMapping);
      }

      mapping(new ClassMappingHelper<TClass>(baseMapping));
    }

    /// <summary>
    /// Maps a property of the class as a special case of the <c>Value&lt;TValue&gt;()</c> mapping - in which the value
    /// within the collection is parsed as an <see cref="CSF.Entities.IIdentity"/> and unwrapped as an entity instance.
    /// </summary>
    /// <param name='property'>
    /// An expression indicating the property to be mapped.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The entity-type for the current instance.
    /// </typeparam>
    /// <typeparam name='TIdentity'>
    /// The identity-type for the current instance.
    /// </typeparam>
    public IEntityMappingHelper<TObject, TEntity, TIdentity> Entity<TEntity, TIdentity>(Expression<Func<TObject, IEntity<TEntity, TIdentity>>> property)
      where TEntity : IEntity
    {
      ISimpleMapping<TEntity> mapping;
      PropertyInfo prop = Reflect.Property<TObject,IEntity<TEntity, TIdentity>>(property);

      mapping = (ISimpleMapping<TEntity>) this.Mapping.Mappings.Where(x => x.Property == prop).FirstOrDefault();
      if(mapping == null)
      {
        mapping = new SimpleMapping<TEntity>(this.Mapping, prop);
        this.Mapping.Mappings.Add(mapping);
      }

      return new EntityMappingHelper<TObject, TEntity, TIdentity>(mapping);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the class mapping helper type.
    /// </summary>
    /// <param name='mapping'>
    /// Mapping.
    /// </param>
    public ClassMappingHelper(IClassMapping<TObject> mapping) : base(mapping) {}

    #endregion
  }
}

