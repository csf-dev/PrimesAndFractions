//
//  IValueTypeCollectionMapping.cs
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
  /// Interface for a mapping which describes a collection of struct-like (value type) object instances.
  /// </summary>
  public interface IValueTypeCollectionMapping<TCollectionItem> : IMapping, ICollectionMapping
    where TCollectionItem : struct
  {
    /// <summary>
    /// Gets the mapping that is used for each item within the collecttion,
    /// </summary>
    /// <value>
    /// The mapping for collection items.
    /// </value>
    IMapping MapAs { get; set; }
  }
}

