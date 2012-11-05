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
        bool success = false;
        object tempResult = null;

        if(collectionIndices == null)
        {
          indices = new int[1];
        }
        else
        {
          indices = new int[collectionIndices.Length + 1];
          Array.Copy(collectionIndices, 0, indices, 0, collectionIndices.Length);
        }

        switch(this.CollectionKeyType)
        {
        case CollectionKeyType.Separate:
          for(int index = this.DeserializeMinimumIndex; index <= this.DeserializeMaximumIndex; index++)
          {
            tempResult = null;
            indices[indices.Length - 1] = index;
            try
            {
              success = this.MapAs.Deserialize(data, out tempResult, indices);
            }
            catch(InvalidMappingException) { throw; }
            catch(Exception) {}

            if(success)
            {
              result.Add((TCollectionItem) tempResult);
              output = true;
            }
          }
          break;

        case CollectionKeyType.Aggregate:
          if(indices.Length != 1)
          {
            throw new InvalidOperationException("When collection type is aggregate (comma-separated) then only one " +
                                                "collection is permitted in the serialization hierarchy.");
          }

          string key = this.KeyNamingPolicy.GetKeyName(null);

          if(data.ContainsKey(key))
          {
            string[] values = data[key].Split(',');
            IDictionary<string,string> tempData = new Dictionary<string, string>();
            foreach(string value in values)
            {
              tempData.Clear();
              tempData.Add(key, value);
              tempResult = null;
              try
              {
                success = this.MapAs.Deserialize(tempData, out tempResult, null);
              }
              catch(InvalidMappingException) { throw; }
              catch(Exception) {}

              if(success)
              {
                result.Add((TCollectionItem) tempResult);
                output = true;
              }
            }
          }

          break;

        default:
          throw new InvalidOperationException("Unsupported collection-key type.");
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

    /// <summary>
    /// Serialize the specified data, exposing the result as an output parameter.
    /// </summary>
    /// <param name='data'>
    /// The object (or object graph) to serialize.
    /// </param>
    /// <param name='result'>
    /// The dictionary of string values to contain the serialized data.
    /// </param>
    /// <param name='collectionIndices'>
    /// A collection of integers, indicating the indices of any collection mappings passed-through during the
    /// </param>
    /// <typeparam name='TInput'>
    /// The type of data to serialize.
    /// </typeparam>
    public override void Serialize(object data, ref IDictionary<string,string> result, int[] collectionIndices)
    {
      ICollection<TCollectionItem> typedData;
      int[] indices;

      if(result == null)
      {
        throw new ArgumentNullException("result");
      }

      typedData = (ICollection<TCollectionItem>) data;

      if(collectionIndices == null)
      {
        indices = new int[1];
      }
      else
      {
        indices = new int[collectionIndices.Length + 1];
        Array.Copy(collectionIndices, 0, indices, 0, collectionIndices.Length);
      }

      switch(this.CollectionKeyType)
      {
      case CollectionKeyType.Separate:
        for(int currentIndex = 0; currentIndex < typedData.Count; currentIndex++)
        {
          indices[indices.Length - 1] = currentIndex;
          this.MapAs.Serialize(typedData.Skip(currentIndex).Take(1).First(), ref result, indices);
        }
        break;

      case CollectionKeyType.Aggregate:
        if(indices.Length != 1)
        {
          throw new InvalidOperationException("When collection type is aggregate (comma-separated) then only one " +
                                              "collection is permitted in the serialization hierarchy.");
        }

        string key = this.KeyNamingPolicy.GetKeyName(null);
        string aggregate = String.Empty;
        ISimpleMapping<TCollectionItem> mapAs = (ISimpleMapping<TCollectionItem>) this.MapAs.MapAs;

        for(int currentIndex = 0; currentIndex < typedData.Count; currentIndex++)
        {
          string value = mapAs.SerializationFunction(typedData.Skip(currentIndex).Take(1).First());
          string separator = (currentIndex > 0)? "," : String.Empty;
          aggregate = String.Concat(aggregate, separator, value);
        }

        result.Add(key, aggregate);

        break;

      default:
        throw new InvalidOperationException("Unsupported collection-key type.");
      }
    }

    /// <summary>
    ///  Validates this mapping instance. 
    /// </summary>
    /// <exception cref='InvalidOperationException'>
    ///  Thrown if the mapping is not valid. 
    /// </exception>
    public override void Validate ()
    {
      base.Validate();

      if(this.CollectionKeyType == CollectionKeyType.Aggregate
         && (this.MapAs.MapAs == null || !(this.MapAs.MapAs is ISimpleMapping)))
      {
        throw new InvalidMappingException("This collection mapping uses the aggregate-separator; only simple " +
                                          "mappings are permitted in this scenario.");
      }
    }

    /// <summary>
    /// Gets the mapping for the current item (the map-as).
    /// </summary>
    /// <returns>
    /// The mapping.
    /// </returns>
    public override IMapping GetMapping()
    {
      if(this.MapAs == null)
      {
        throw new InvalidOperationException("This collection mapping is not mapped as another mapping.");
      }

      return this.MapAs;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the reference type collection mapping.
    /// </summary>
    public ReferenceTypeCollectionMapping() : this(null, null, true) {}

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
                                          PropertyInfo property) : this(parentMapping, property, false) {}

    /// <summary>
    /// Initializes a new instance of the reference type collection mapping.
    /// </summary>
    /// <param name='parentMapping'>
    /// Parent mapping.
    /// </param>
    /// <param name='property'>
    /// Property.
    /// </param>
    /// <param name='isRootMapping'>
    /// Is root mapping.
    /// </param>
    protected ReferenceTypeCollectionMapping(IMapping parentMapping,
                                             PropertyInfo property,
                                             bool isRootMapping) : base(parentMapping, property, isRootMapping)
    {
      this.CollectionKeyType = CollectionKeyType.Separate;
      this.DeserializeMaximumIndex = 0;
      this.DeserializeMaximumIndex = 49;
    }

    #endregion
  }
}

