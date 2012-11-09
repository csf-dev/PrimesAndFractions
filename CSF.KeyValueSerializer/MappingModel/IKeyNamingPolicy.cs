//
//  IComponentNamingRule.cs
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
  /// Interface for a key-naming policy.
  /// </summary>
  public interface IKeyNamingPolicy
  {
    /// <summary>
    /// Gets the mapping associated with this naming rule.
    /// </summary>
    /// <value>
    /// The associated mapping.
    /// </value>
    IMapping AssociatedMapping { get; }

    /// <summary>
    /// Resolves and returns the name of the key for the <see cref="AssociatedMapping"/>.
    /// </summary>
    /// <returns>
    /// The key name.
    /// </returns>
    /// <param name='collectionIndices'>
    /// Zero or more indices for collection-type mappings that are passed-through when traversing from the root of the
    /// serialization hierarchy to the associated mapping.
    /// </param>
    string GetKeyName(params int[] collectionIndices);

    /// <summary>
    /// Resolves and returns the name of the key for the <see cref="AssociatedMapping"/>.
    /// </summary>
    /// <returns>
    /// The key name.
    /// </returns>
    /// <param name='collectionIndices'>
    /// Zero or more indices for collection-type mappings that are passed-through when traversing from the root of the
    /// serialization hierarchy to the associated mapping.
    /// </param>
    /// <param name='currentCollectionNumber'>
    /// The current collection number that is being dealt with.  Essentially the current index within
    /// <paramref name="collectionIndices"/>
    /// </param>
    string GetKeyName(int[] collectionIndices, ref int currentCollectionNumber);
  }
}

