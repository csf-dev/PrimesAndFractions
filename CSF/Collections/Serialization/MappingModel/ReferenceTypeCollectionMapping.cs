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

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// A mapping implementation for mapping a reference-type collection property.
  /// </summary>
  public class ReferenceTypeCollectionMapping<TCollectionItem>
    : MappingBase, IReferenceTypeCollectionMapping<TCollectionItem>
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

    /// <summary>
    /// Gets or sets the minimum 'array index' used whilst searching for values to deserialize.
    /// </summary>
    /// <value>
    /// The minimum 'array index' for deserialization.
    /// </value>
    public int DeserializeMinimumIndex
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the maximum 'array index' used whilst searching for values to deserialize.
    /// </summary>
    /// <value>
    /// The maximum 'array index' for deserialization.
    /// </value>
    public int DeserializeMaximumIndex
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the way that this collection is mapped.
    /// </summary>
    /// <value>
    /// The map-as mapping.
    /// </value>
    public IClassMapping<TCollectionItem> MapAs
    {
      get;
      set;
    }

    /// <summary>
    ///  Deserialize the specified data as an object instance. 
    /// </summary>
    /// <returns>
    ///  A value that indicates whether deserialization was successful or not. 
    /// </returns>
    /// <param name='data'>
    ///  The dictionary/collection of string data to deserialize from. 
    /// </param>
    /// <param name='result'>
    /// The output/deserialized object instance.  If the return value is false (unsuccessful deserialization) then the
    /// output value of this parameter is undefined.
    /// </param>
    /// <param name='collectionIndices'>
    ///  A collection of integers, indicating the indices of any collection mappings passed-through during the 
    /// </param>
    private bool Deserialize (IDictionary<string, string> data,
                              out ICollection<TCollectionItem> result,
                              params int[] collectionIndices)
    {
      bool output = false;

      result = new List<TCollectionItem>();

      if(this.MayDeserialize(data))
      {
        int[] indices;

        if(collectionIndices == null)
        {
          indices = new int[1];
        }
        else
        {
          indices = new int[collectionIndices.Length + 1];
          Array.Copy(collectionIndices, 0, indices, 0, collectionIndices.Length);
        }

        for(int index = this.DeserializeMinimumIndex; index <= this.DeserializeMaximumIndex; index++)
        {
          object tempResult = null;
          bool success = false;

          indices[indices.Length - 1] = index;
          try
          {
            success = this.MapAs.Deserialize(data, out tempResult, indices);
          }
          catch(Exception) {}

          if(success)
          {
            result.Add((TCollectionItem) tempResult);
            output = true;
          }
        }
      }

      if(!output && this.Mandatory)
      {
        throw new MandatorySerializationException(this);
      }

      return output;
    }

    /// <summary>
    ///  Deserialize the specified data as an object instance. 
    /// </summary>
    /// <returns>
    ///  A value that indicates whether deserialization was successful or not. 
    /// </returns>
    /// <param name='data'>
    ///  The dictionary/collection of string data to deserialize from. 
    /// </param>
    /// <param name='result'>
    ///  The output/deserialized object instance. If the return value is false (unsuccessful deserialization) then the
    /// output value of this parameter is undefined. 
    /// </param>
    /// <param name='collectionIndices'>
    ///  A collection of integers, indicating the indices of any collection mappings passed-through during the 
    /// </param>
    public override bool Deserialize (IDictionary<string, string> data,
                                      out object result,
                                      params int[] collectionIndices)
    {
      ICollection<TCollectionItem> tempResult;
      bool output = this.Deserialize(data, out tempResult, collectionIndices);
      result = tempResult;
      return output;
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
      this.DeserializeMaximumIndex = 0;
      this.DeserializeMaximumIndex = 50;
    }

    #endregion
  }
}

