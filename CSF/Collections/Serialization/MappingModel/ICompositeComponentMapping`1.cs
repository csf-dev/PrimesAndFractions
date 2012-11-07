//
//  ICompositeComponentMapping.cs
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

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// Interface for the mapping of a single 'component' of a composite mapping.
  /// </summary>
  public interface ICompositeComponentMapping<TValue> : IEndpointMapping
  {
    /// <summary>
    /// Gets the 'parent' composite mapping.
    /// </summary>
    /// <value>
    /// The parent mapping.
    /// </value>
    ICompositeMapping<TValue> ParentMapping { get; }

    /// <summary>
    /// Gets the component identifier for this mapping.
    /// </summary>
    /// <value>
    /// The component identifier.
    /// </value>
    object ComponentIdentifier { get; }

    /// <summary>
    /// Gets the function used to serialize a single string value (corresponding to this component) from the property
    /// value.
    /// </summary>
    /// <value>
    /// A method body containing the serialization function.
    /// </value>
    Func<TValue,string> SerializationFunction { get; set; }
  }
}

