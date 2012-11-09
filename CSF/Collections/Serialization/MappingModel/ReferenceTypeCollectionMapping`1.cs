//
//  ReferenceTypeCollectionMapping.cs
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
  /// A mapping implementation for mapping a reference-type collection property.
  /// </summary>
  public class ReferenceTypeCollectionMapping<TItem>
    : CollectionMapping<TItem>, IReferenceTypeCollectionMapping<TItem>
    where TItem : class
  {
    #region properties

    /// <summary>
    /// Gets or sets the way that this collection is mapped.
    /// </summary>
    /// <value>
    /// The map-as mapping.
    /// </value>
    public virtual IClassMapping<TItem> MapAs
    {
      get {
        return (IClassMapping<TItem>) base.BaseMapAs;
      }
      set {
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
      IMapping output = null;

      if(this.MapAs != null)
      {
        output = this.MapAs.MapAs;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the reference type collection mapping.
    /// </summary>
    public ReferenceTypeCollectionMapping() : base(null, null, true) {}

    /// <summary>
    /// Initializes a new instance of the reference type collection mapping.
    /// </summary>
    /// <param name='parentMapping'>
    /// Parent mapping.
    /// </param>
    /// <param name='property'>
    /// Property.
    /// </param>
    public ReferenceTypeCollectionMapping(IMapping parentMapping,
                                          PropertyInfo property) : base(parentMapping, property, false) {}
    #endregion
  }
}

