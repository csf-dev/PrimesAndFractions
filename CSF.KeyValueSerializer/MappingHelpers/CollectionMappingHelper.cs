//
//  CollectionMappingHelper.cs
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
using CSF.KeyValueSerializer.MappingModel;

namespace CSF.KeyValueSerializer.MappingHelpers
{
  /// <summary>
  /// Base class for mapping helpers that map collection-type mappings.
  /// </summary>
  public abstract class CollectionMappingHelper<TMapping> : MappingHelper<TMapping>, ICollectionMappingHelper
    where TMapping : ICollectionMapping
  {
    #region ICollectionMappingHelper implementation

    /// <summary>
    /// Indicates that this mapping will use a comma-separated list of many item values within a single string value.
    /// </summary>
    public void CommaSeparatedList()
    {
      this.Mapping.CollectionKeyType = CollectionKeyType.Aggregate;
    }

    /// <summary>
    /// Indicates that this mapping will use an array-like notation, storing its separate values in many string values.
    /// </summary>
    public void ArrayStyleList()
    {
      this.Mapping.CollectionKeyType = CollectionKeyType.Separate;
    }

    /// <summary>
    /// Sets the minimum 'array index' used whilst searching for values to deserialize.
    /// </summary>
    /// <param name='index'>
    /// The index.
    /// </param>
    public void DeserializeMinIndex(int index)
    {
      this.Mapping.DeserializeMinimumIndex = index;
    }

    /// <summary>
    /// Sets the maximum 'array index' used whilst searching for values to deserialize.
    /// </summary>
    /// <param name='index'>
    /// The index.
    /// </param>
    public void DeserializeMaxIndex(int index)
    {
      this.Mapping.DeserializeMaximumIndex = index;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the collection mapping helper type.
    /// </summary>
    /// <param name='mapping'>
    /// Mapping.
    /// </param>
    public CollectionMappingHelper(TMapping mapping) : base(mapping) {}

    #endregion
  }
}

