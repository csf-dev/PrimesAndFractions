//
//  IEndpointMapping.cs
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

namespace CSF.KeyValueSerializer.MappingModel
{
  /// <summary>
  /// Interface for an <see cref="IMapping"/> that is a viable end-point of a mapping tree, IE: one that may have a key
  /// within the key/value collection.
  /// </summary>
  public interface IEndpointMapping
  {
    /// <summary>
    /// Gets the collection key that corresponds to the data for this property. 
    /// </summary>
    /// <param name='collectionIndices'>
    /// Zero or more indices for collection-type mappings that are passed-through when traversing from the root of the
    /// serialization hierarchy to the associated mapping.
    /// </param>
    /// <returns>
    /// The collection key. 
    /// </returns>
    string GetKeyName(params int[] collectionIndices);
  }
}

