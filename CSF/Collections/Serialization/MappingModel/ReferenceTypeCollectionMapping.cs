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

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// A mapping implementation for mapping a reference-type collection property.
  /// </summary>
  public class ReferenceTypeCollectionMapping<TCollectionItem>
    : ClassMapping<TCollectionItem>, IReferenceTypeCollectionMapping<TCollectionItem>
    where TCollectionItem : class
  {
    #region fields

    private CollectionKeyType _collectionKeyType;

    #endregion

    #region properties

    /// <summary>
    /// Gets the type of collection keying in-use.
    /// </summary>
    /// <value>
    /// The type of the collection key.
    /// </value>
    public CollectionKeyType CollectionKeyType
    {
      get {
        return _collectionKeyType;
      }
      set {
        if(!Enum.IsDefined(typeof(CollectionKeyType), value))
        {
          throw new ArgumentException("Undefined collection keying type.");
        }

        _collectionKeyType = value;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the reference type collection mapping.
    /// </summary>
    public ReferenceTypeCollectionMapping() : this(null, null) {}

    /// <summary>
    /// Initializes a new instance of the reference type collection mapping.
    /// </summary>
    /// <param name='parentMapping'>
    /// Parent mapping.
    /// </param>
    /// <param name='property'>
    /// Property.
    /// </param>
    public ReferenceTypeCollectionMapping(IMapping parentMapping, PropertyInfo property) : base(parentMapping, property)
    {
      this.CollectionKeyType = CollectionKeyType.Separate;
    }

    #endregion
  }
}

