//
//  ICompositeMappingPart.cs
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
using CSF.KeyValueSerializer.MappingModel;

namespace CSF.KeyValueSerializer.MappingHelpers
{
  /// <summary>
  /// Interface for a fluent-interface helper that maps types for which a singular data item is composed of multiple
  /// values within the collection.
  /// </summary>
  public interface ICompositeMappingHelper<TObject,TValue> : IMappingHelper
    where TObject : class
  {
    /// <summary>
    /// Facilitates the setting of a key-naming-policy into the associated mapping.
    /// </summary>
    /// <returns>
    /// The current mapping helper instance, to facilitate chaining of methods.
    /// </returns>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    ICompositeMappingHelper<TObject,TValue> NamingPolicy<TPolicy>()
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
    ICompositeMappingHelper<TObject,TValue> NamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy;

    /// <summary>
    /// Specifies a component of this composite mapping, facilitating the mapping of that component.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for that component, within this composite mapping.  The identifier may be any object and is
    /// distinguished from other identifiers by its default equality comparer.
    /// </param>
    /// <param name='mappingMethod'>
    /// A method body that indicates how the component should be mapped.
    /// </param>
    ICompositeMappingHelper<TObject,TValue> Component(object identifier,
                                                      Action<ICompositeComponentMappingHelper<TObject,TValue>> mappingMethod);

    /// <summary>
    /// A method body that indicates how instances of the mapped type should be deserialized from the component values.
    /// </summary>
    /// <param name='deserializationFunction'>
    /// A method body containing the deserialization function.
    /// </param>
    ICompositeMappingHelper<TObject,TValue> Deserialize(Func<IDictionary<object,string>,TValue> deserializationFunction);
  }
}

