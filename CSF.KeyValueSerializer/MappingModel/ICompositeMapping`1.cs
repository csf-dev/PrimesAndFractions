//
//  ICompositeMapping.cs
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
using System.Collections.Generic;

namespace CSF.KeyValueSerializer.MappingModel
{
  /// <summary>
  /// Interface for a mapping which describes a composite mapping of multiple collection values to a single property.
  /// </summary>
  public interface ICompositeMapping<TValue> : IMapping
  {
    #region properties

    /// <summary>
    /// Gets the function used to deserialize the property value from a dictionary of string values, indexed by the
    /// component identifiers from this mapping.
    /// </summary>
    /// <value>
    /// A method body containing the deserialization function.
    /// </value>
    Func<IDictionary<object,string>,TValue> DeserializationFunction { get; set; }

    /// <summary>
    /// Gets a collection of the components that make up this mapping and their respective mappings.
    /// </summary>
    /// <value>
    /// A collection of the components, indexed by their identifiers.
    /// </value>
    IDictionary<object,ICompositeComponentMapping<TValue>> Components { get; set; }

    #endregion
  }
}

