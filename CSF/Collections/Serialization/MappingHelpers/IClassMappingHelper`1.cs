//
//  IClassMappingPart.cs
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
using System.Linq.Expressions;
using System.Collections.Generic;
using CSF.Entities;
using CSF.Collections.Serialization.MappingModel;

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Interface for a fluent-interface helper that maps complex class-like/reference type objects.
  /// </summary>
  public interface IClassMappingHelper<TObject> : IMappingHelper
    where TObject : class
  {
    #region mapping the whole class instance

    /// <summary>
    /// Maps the class using an association with a single value within the collection.
    /// </summary>
    ISimpleMappingHelper<TObject,TObject> Simple();

    /// <summary>
    /// Maps the class using an association with multiple values within the collection.
    /// </summary>
    ICompositeMappingHelper<TObject,TObject> Composite();

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
    IEntityMappingHelper<TObject,TEntity,TIdentity> Entity<TEntity,TIdentity>()
      where TEntity : IEntity;

    #endregion

    #region mapping class properties

    /// <summary>
    /// Maps a property of the class using an association with a single value within the collection.
    /// </summary>
    /// <param name='property'>
    /// An expression indicating the property to be mapped.
    /// </param>
    /// <typeparam name='TValue'>
    /// The type of the property's value.
    /// </typeparam>
    ISimpleMappingHelper<TObject,TValue> Simple<TValue>(Expression<Func<TObject,TValue>> property);

    /// <summary>
    /// Maps a property of the class using an association with multiple values within the collection.
    /// </summary>
    /// <param name='property'>
    /// An expression indicating the property to be mapped.
    /// </param>
    /// <typeparam name='TValue'>
    /// The type of the property's value.
    /// </typeparam>
    ICompositeMappingHelper<TObject,TValue> Composite<TValue>(Expression<Func<TObject,TValue>> property);

    /// <summary>
    /// Maps a property of the class as a collection of class-like/reference-type instances.
    /// </summary>
    /// <param name='property'>
    /// An expression indicating the property to be mapped.
    /// </param>
    /// <param name='mapping'>
    /// A method body that contains the mapping definition for each item within the collection.
    /// </param>
    /// <typeparam name='TCollectionItem'>
    /// The type of the items within the collection.
    /// </typeparam>
    void Collection<TCollectionItem>(Expression<Func<TObject,ICollection<TCollectionItem>>> property,
                                     Action<IReferenceTypeCollectionMappingHelper<TObject,TCollectionItem>> mapping)
      where TCollectionItem : class;

    /// <summary>
    /// Maps a property of the class as a collection of struct-like/value-type instances.
    /// </summary>
    /// <param name='property'>
    /// An expression indicating the property to be mapped.
    /// </param>
    /// <param name='mapping'>
    /// A method body that contains the mapping definition for each item within the collection.
    /// </param>
    /// <typeparam name='TCollectionItem'>
    /// The type of the items within the collection.
    /// </typeparam>
    void ValueCollection<TCollectionItem>(Expression<Func<TObject,ICollection<TCollectionItem>>> property,
                                          Action<IValueTypeCollectionMappingHelper<TObject,TCollectionItem>> mapping)
      where TCollectionItem : struct;

    /// <summary>
    /// Maps a property of the class as a complex class (must be a class/reference type) with a mapping of its own.
    /// </summary>
    /// <param name='property'>
    /// An expression indicating the property to be mapped.
    /// </param>
    /// <param name='mapping'>
    /// A method body that contains the mapping definition for the property.
    /// </param>
    /// <typeparam name='TClass'>
    /// The type of the property that the new mappings will match to.
    /// </typeparam>
    void Class<TClass>(Expression<Func<TObject,TClass>> property,
                       Action<IClassMappingHelper<TClass>> mapping)
      where TClass : class;

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
    IEntityMappingHelper<TObject,TEntity,TIdentity> Entity<TEntity,TIdentity>(Expression<Func<TObject,IEntity<TEntity,TIdentity>>> property)
      where TEntity : IEntity;

    #endregion

    #region mapping options

    /// <summary>
    /// Facilitates the setting of a key-naming-policy into the associated mapping.
    /// </summary>
    /// <returns>
    /// The current mapping helper instance, to facilitate chaining of methods.
    /// </returns>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    IClassMappingHelper<TObject> NamingPolicy<TPolicy>()
      where TPolicy : IKeyNamingPolicy;

    /// <summary>
    /// Facilitates the setting of a key-naming-policy into the associated mapping.
    /// </summary>
    /// <returns>
    /// The current mapping helper instance, to facilitate chaining of methods.
    /// </returns>
    /// <param name='factoryMethod'>
    /// A custom factory method to use when constructing the naming policy.
    /// </param>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    IClassMappingHelper<TObject> NamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy;

    /// <summary>
    /// Specifies a factory-function to use when creating instances of the mapped class, instead of the default
    /// parameterless constructor.
    /// </summary>
    /// <param name='factoryFunction'>
    /// The factory method/function.
    /// </param>
    void UsingFactory(Func<TObject> factoryFunction);

    #endregion
  }
}

