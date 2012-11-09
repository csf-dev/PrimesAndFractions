//
//  ReferenceTypeCollectionMappingHelper.cs
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
using CSF.KeyValueSerializer.MappingModel;
using CSF.Entities;
using System.Linq.Expressions;
using System.Collections.Generic;
using CSF.Reflection;
using System.Linq;
using System.Reflection;

namespace CSF.KeyValueSerializer.MappingHelpers
{
  /// <summary>
  /// Mapping helper type for a collection of reference-type items.
  /// </summary>
  public class ReferenceTypeCollectionMappingHelper<TObject,TItem>
    : CollectionMappingHelper<IReferenceTypeCollectionMapping<TItem>>, IReferenceTypeCollectionMappingHelper<TObject,TItem>
    where TObject : class
    where TItem : class
  {
    #region properties
    
    /// <summary>
    /// Gets the mapping helper for the 'child' class mapping.
    /// </summary>
    /// <value>
    /// The child helper.
    /// </value>
    protected virtual IClassMappingHelper<TItem> ChildHelper
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    ///  Specifies a factory-function to use when creating instances of the mapped class, instead of the default
    /// parameterless constructor. 
    /// </summary>
    /// <param name='factoryFunction'>
    ///  The factory method/function. 
    /// </param>
    public void UsingFactory(Func<TItem> factoryFunction)
    {
      this.ChildHelper.UsingFactory(factoryFunction);
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
    public IReferenceTypeCollectionMappingHelper<TObject,TItem> CollectionNamingPolicy<TPolicy>()
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
    public IReferenceTypeCollectionMappingHelper<TObject,TItem> CollectionNamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy
    {
      this.Mapping.AttachKeyNamingPolicy<TPolicy>(factoryMethod);
      return this;
    }

    /// <summary>
    ///  Facilitates the setting of a key-naming-policy into the associated mapping. 
    /// </summary>
    /// <returns>
    ///  The current mapping helper instance, to facilitate chaining of methods. 
    /// </returns>
    /// <typeparam name='TPolicy'>
    /// The 1st type parameter.
    /// </typeparam>
    public IClassMappingHelper<TItem> NamingPolicy<TPolicy>()
      where TPolicy : IKeyNamingPolicy
    {
      return this.ChildHelper.NamingPolicy<TPolicy>();
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
    /// The 1st type parameter.
    /// </typeparam>
    public IClassMappingHelper<TItem> NamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy
    {
      return this.ChildHelper.NamingPolicy<TPolicy>(factoryMethod);
    }

    #endregion

    #region mapping this instance as something

    /// <summary>
    ///  Maps the class using an association with a single value within the collection. 
    /// </summary>
    public ISimpleMappingHelper<TItem, TItem> Simple()
    {
      return this.ChildHelper.Simple();
    }

    /// <summary>
    ///  Maps the class using an association with multiple values within the collection. 
    /// </summary>
    public ICompositeMappingHelper<TItem, TItem> Composite()
    {
      return this.ChildHelper.Composite();
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
    public IEntityMappingHelper<TItem, TEntity, TIdentity> Entity<TEntity, TIdentity>()
      where TEntity : IEntity
    {
      return this.ChildHelper.Entity<TEntity,TIdentity>();
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
    public ISimpleMappingHelper<TItem, TValue> Simple<TValue>(Expression<Func<TItem, TValue>> property)
    {
      return this.ChildHelper.Simple<TValue>(property);
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
    public ICompositeMappingHelper<TItem, TValue> Composite<TValue>(Expression<Func<TItem, TValue>> property)
    {
      return this.ChildHelper.Composite<TValue>(property);
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
    /// <typeparam name='TItem'>
    ///  The type of the items within the collection. 
    /// </typeparam>
    public void Collection<TNested>(Expression<Func<TItem,ICollection<TNested>>> property,
                                    Action<IReferenceTypeCollectionMappingHelper<TItem, TNested>> mapping)
      where TNested : class
    {
      this.ChildHelper.Collection<TNested>(property, mapping);
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
    /// <typeparam name='TItem'>
    ///  The type of the items within the collection. 
    /// </typeparam>
    public void ValueCollection<TNested>(Expression<Func<TItem, ICollection<TNested>>> property,
                                         Action<IValueTypeCollectionMappingHelper<TItem, TNested>> mapping)
      where TNested : struct
    {
      this.ChildHelper.ValueCollection<TNested>(property, mapping);
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
    public void Class<TClass>(Expression<Func<TItem, TClass>> property,
                              Action<IClassMappingHelper<TClass>> mapping)
      where TClass : class
    {
      this.ChildHelper.Class<TClass>(property, mapping);
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
    public IEntityMappingHelper<TItem, TEntity, TIdentity> Entity<TEntity, TIdentity>(Expression<Func<TItem, IEntity<TEntity, TIdentity>>> property)
      where TEntity : IEntity
    {
      return this.ChildHelper.Entity<TEntity, TIdentity>(property);
    }

    #endregion
    
    #region constructor

    /// <summary>
    /// Initializes a new instance of the reference-type collection mapping helper type.
    /// </summary>
    /// <param name='mapping'>
    /// Mapping.
    /// </param>
    public ReferenceTypeCollectionMappingHelper(IReferenceTypeCollectionMapping<TItem> mapping) : base(mapping)
    {
      mapping.MapAs = new ClassMapping<TItem>(mapping, null);
      this.ChildHelper = new ClassMappingHelper<TItem>(mapping.MapAs);
    }

    #endregion
  }
}

