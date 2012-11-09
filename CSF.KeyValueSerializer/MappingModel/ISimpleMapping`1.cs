//
//  ISimpleMapping.cs
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
using System.Reflection;

namespace CSF.KeyValueSerializer.MappingModel
{
  /// <summary>
  /// Interface for a mapping which describes a simple value-to-property mapping using a single collection value.
  /// </summary>
  public interface ISimpleMapping<TValue> : IMapping, IEndpointMapping
  {
    #region properties

    /// <summary>
    /// Gets the function used to deserialize the property value from a string.
    /// </summary>
    /// <value>
    /// A method body containing the deserialization function.
    /// </value>
    Func<string,TValue> DeserializationFunction { get; set; }

    /// <summary>
    /// Gets the function used to serialize a string from the property value.
    /// </summary>
    /// <value>
    /// A method body containing the serialization function.
    /// </value>
    Func<TValue,string> SerializationFunction { get; set; }

    #endregion
  }
}

