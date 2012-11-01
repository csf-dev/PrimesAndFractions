//
//  IValueMappingPart.cs
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
using CSF.Collections.Serialization.MappingModel;

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Interface for a mapping helper that maps a simple value-to-property (or type) association.
  /// </summary>
  public interface ISimpleMappingHelper<TObject,TValue> : IMappingHelper
    where TObject : class
  {
    /// <summary>
    /// Facilitates the provision of a custom function for serializing object values to a string.
    /// </summary>
    /// <param name='serializationFunction'>
    /// A method body for serializing values.
    /// </param>
    ISimpleMappingHelper<TObject,TValue> Serialize(Func<TValue,string> serializationFunction);

    /// <summary>
    /// Facilitates the provision of a custom function for deserializing strings to an object value.
    /// </summary>
    /// <param name='deserializationFunction'>
    /// A method body for deserializing values.
    /// </param>
    ISimpleMappingHelper<TObject,TValue> Deserialize(Func<string,TValue> deserializationFunction);

    /// <summary>
    /// Facilitates the setting of a key-naming-policy into the associated mapping.
    /// </summary>
    /// <returns>
    /// The current mapping helper instance, to facilitate chaining of methods.
    /// </returns>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    ISimpleMappingHelper<TObject,TValue> NamingPolicy<TPolicy>()
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
    ISimpleMappingHelper<TObject, TValue> NamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy;
  }
}

