//
//  ICollectionMappingHelper.cs
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

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Interface for a mapping helper that maps a collection of items.
  /// </summary>
  public interface ICollectionMappingHelper : IMappingHelper
  {
    /// <summary>
    /// Indicates that this mapping will use a comma-separated list of many item values within a single string value.
    /// </summary>
    void CommaSeparatedList();

    /// <summary>
    /// Indicates that this mapping will use an array-like notation, storing its separate values in many string values.
    /// </summary>
    void ArrayStyleList();

    /// <summary>
    /// Sets the minimum 'array index' used whilst searching for values to deserialize.
    /// </summary>
    /// <param name='index'>
    /// The index.
    /// </param>
    void DeserializeMinIndex(int index);

    /// <summary>
    /// Sets the maximum 'array index' used whilst searching for values to deserialize.
    /// </summary>
    /// <param name='index'>
    /// The index.
    /// </param>
    void DeserializeMaxIndex(int index);
  }
}

