//
//  ValueTypeCollectionMapping.cs
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
using System.Linq;

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// A mapping implementation for mapping a value-type collection property.
  /// </summary>
  public class ValueTypeCollectionMapping<TItem>
    : CollectionMapping<TItem>, IValueTypeCollectionMapping<TItem>
    where TItem : struct
  {
    #region properties

    /// <summary>
    /// Gets the mapping that is used for each item within the collecttion,
    /// </summary>
    /// <value>
    /// The mapping for collection items.
    /// </value>
    public virtual IMapping MapAs
    {
      get {
        return base.BaseMapAs;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        else if(!(value is ISimpleMapping<TItem>)
                && !(value is ICompositeMapping<TItem>))
        {
          throw new ArgumentException("Map-as must be an appropriate value-type mapping (IE: either a simple or " +
                                      "composite mapping).");
        }

        base.BaseMapAs = value;
      }
    }

    /// <summary>
    /// Gets the appropriate map-as mapping to be used/inspected should the current instance be configured to use
    /// <see cref="CollectionKeyType.Aggregate"/>.
    /// </summary>
    /// <returns>
    /// The appropriate map-as mapping.
    /// </returns>
    protected override IMapping GetAggregateMapAs()
    {
      return this.MapAs;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the value type collection mapping.
    /// </summary>
    public ValueTypeCollectionMapping() : base(null, null, true) {}

    /// <summary>
    /// Initializes a new instance of the value type collection mapping.
    /// </summary>
    /// <param name='parentMapping'>
    /// Parent mapping.
    /// </param>
    /// <param name='property'>
    /// Property.
    /// </param>
    public ValueTypeCollectionMapping(IMapping parentMapping,
                                      PropertyInfo property) : base(parentMapping, property, false) {}

    #endregion
  }
}

