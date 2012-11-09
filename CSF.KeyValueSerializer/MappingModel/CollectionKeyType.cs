//
//  ICollectionEnumerationMethod.cs
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
  /// Enumerates the methods by which values are extracted (and aggregated) in order to represent a collection.
  /// </summary>
  public enum CollectionKeyType
  {
    /// <summary>
    /// Multiple values are stored within a single key, separated by commas.
    /// </summary>
    Aggregate = 1,

    /// <summary>
    /// Many keys are used, using array-like notation to distinguish them from each other.
    /// </summary>
    Separate
  }
}

