//
//  CollectionMapping.cs
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

namespace CSF.KeyValueSerializer.MappingModel
{
  /// <summary>
  /// Abstract base class for a collection-like mapping.
  /// </summary>
  public abstract class CollectionMapping<TItem> : MappingBase<ICollection<TItem>>, ICollectionMapping
  {
    #region fields

    private CollectionKeyType _collectionKeyType;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the way that the current collection is mapped.
    /// </summary>
    /// <value>
    /// The map-as mapping.
    /// </value>
    protected virtual IMapping BaseMapAs
    {
      get;
      set;
    }

    /// <summary>
    /// Gets the type of collection keying in-use.
    /// </summary>
    /// <value>
    /// The type of the collection key.
    /// </value>
    public virtual CollectionKeyType CollectionKeyType
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
    public virtual int DeserializeMinimumIndex
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
    public virtual int DeserializeMaximumIndex
    {
      get;
      set;
    }

    #endregion

    #region methods

    /// <summary>
    ///  Validates this mapping instance. 
    /// </summary>
    public override void Validate()
    {
      base.Validate ();

      if(this.BaseMapAs == null)
      {
        throw new InvalidMappingException("Collection-type mappings require a 'child' mapping, mapping the items of " +
                                          "the collection.");
      }
      else if(this.CollectionKeyType == CollectionKeyType.Aggregate)
      {
        IMapping aggregateMapping = this.GetAggregateMapAs();

        if(aggregateMapping == null)
        {
          throw new InvalidMappingException("This collection-type mapping is configured to use aggregate values with " +
                                            "a separator character.  The only valid configuration is to map the " +
                                            "collection using a 'simple' mapping but no mapping was found.");
        }
        else if(!(aggregateMapping is ISimpleMapping))
        {
          throw new InvalidMappingException("This collection-type mapping is configured to use aggregate values with " +
                                            "a separator character.  The only valid configuration is to map the " +
                                            "collection using a 'simple' mapping but a different mapping-type was " +
                                            "found.");
        }
      }
    }

    /// <summary>
    ///  Gets the child mapping for the current mapping instance. 
    /// </summary>
    /// <returns>
    /// The mapping.
    /// </returns>
    public override IMapping GetMapping()
    {
      if(this.BaseMapAs == null)
      {
        throw new InvalidOperationException("This collection mapping does not contain a 'child' mapping (this is " +
                                            "invalid).");
      }

      return this.BaseMapAs;
    }

    /// <summary>
    /// Gets the appropriate map-as mapping to be used/inspected should the current instance be configured to use
    /// <see cref="CollectionKeyType.Aggregate"/>.
    /// </summary>
    /// <returns>
    /// The appropriate map-as mapping.
    /// </returns>
    protected abstract IMapping GetAggregateMapAs();

    /// <summary>
    ///  Serialize the specified data, exposing the serialized results as an output parameter. 
    /// </summary>
    /// <param name='data'>
    ///  The object graph to serialize, the root of which should be an object instance that corresponds to the current
    /// mapping. 
    /// </param>
    /// <param name='result'>
    ///  The dictionary of string values containing the serialized data that is created from the current mapping
    /// instance. 
    /// </param>
    /// <param name='collectionIndices'>
    ///  A collection of integers, indicating the indices of any collection mappings passed-through during the
    /// serialization process. 
    /// </param>
    /// <returns>
    ///  A value that indicates whether or not the serialization was successful. 
    /// </returns>
    public override bool Serialize(ICollection<TItem> data, out IDictionary<string,string> result, int[] collectionIndices)
    {
      int[] indices;
      bool output = false;

      result = new Dictionary<string, string>();

      if(collectionIndices == null)
      {
        indices = new int[1];
      }
      else
      {
        indices = new int[collectionIndices.Length + 1];
        Array.Copy(collectionIndices, 0, indices, 0, collectionIndices.Length);
      }

      if(data != null)
      {
        switch(this.CollectionKeyType)
        {
        case CollectionKeyType.Separate:
          for(int currentIndex = 0; currentIndex < data.Count; currentIndex++)
          {
            IDictionary<string,string> thisItem;
            bool thisSuccess;

            indices[indices.Length - 1] = currentIndex;
            try
            {
              thisSuccess = this.BaseMapAs.Serialize(data.Skip(currentIndex).Take(1).First(), out thisItem, indices);
            }
            catch(MandatorySerializationException)
            {
              output = false;
              break;
            }

            if(thisSuccess)
            {
              foreach(string serializationKey in thisItem.Keys)
              {
                result.Add(serializationKey, thisItem[serializationKey]);
              }
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
          string aggregate = String.Empty;
          bool mandatoryFail = false;
          ISimpleMapping<TItem> mapAs = (ISimpleMapping<TItem>) this.GetAggregateMapAs();

          for(int currentIndex = 0; currentIndex < data.Count; currentIndex++)
          {
            string value;

            try
            {
              value = mapAs.SerializationFunction(data.Skip(currentIndex).Take(1).First());
            }
            catch(Exception)
            {
              if(mapAs.Mandatory)
              {
                mandatoryFail = true;
              }
              continue;
            }

            if(value == null)
            {
              if(mapAs.Mandatory)
              {
                mandatoryFail = true;
              }
              continue;
            }
            else
            {
              string separator = !String.IsNullOrEmpty(aggregate)? "," : String.Empty;
              aggregate = String.Concat(aggregate, separator, value);
            }
          }

          if(!mandatoryFail)
          {
            result.Add(key, aggregate);
            output = true;
          }

          break;

        default:
          throw new InvalidOperationException("Unsupported collection-key type.");
        }
      }

      if(!output)
      {
        result = null;
      }
      else
      {
        this.WriteFlag(result);
      }

      return output;
    }

    /// <summary>
    /// Private (strongly typed) deserialization method.
    /// </summary>
    /// <returns>
    ///  A value that indicates whether deserialization was successful or not. 
    /// </returns>
    /// <param name='data'>
    ///  The dictionary/collection of string data to deserialize. 
    /// </param>
    /// <param name='result'>
    ///  The output/deserialized object instance. If the return value is false (unsuccessful deserialization) then the
    /// output value of this parameter is undefined. Otherwise this parameter exposes an object graph representing the
    /// deserialized data. 
    /// </param>
    /// <param name='collectionIndices'>
    ///  A collection of integers, indicating the indices of any collection mappings passed-through during the
    /// deserialization process. 
    /// </param>
    public override bool Deserialize(IDictionary<string, string> data,
                                     out ICollection<TItem> result,
                                     int[] collectionIndices)
    {
      bool output = false;

      result = new List<TItem>();

      this.Validate();

      if(this.SatisfiesFlag(data))
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
              success = this.BaseMapAs.Deserialize(data, out tempResult, indices);
            }
            catch(InvalidMappingException) { throw; }
            catch(MandatorySerializationException)
            {
              output = false;
              break;
            }
            catch(Exception) {}

            if(success)
            {
              result.Add((TItem) tempResult);
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
              success = false;
              try
              {
                success = this.BaseMapAs.Deserialize(tempData, out tempResult, null);
              }
              catch(InvalidMappingException) { throw; }
              catch(MandatorySerializationException)
              {
                output = false;
                break;
              }
              catch(Exception) {}

              if(success)
              {
                result.Add((TItem) tempResult);
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
      else if(!output)
      {
        result = null;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the collection mapping base class.
    /// </summary>
    /// <param name='parentMapping'>
    /// A reference to the parent mapping instance.
    /// </param>
    /// <param name='property'>
    /// An optional reference to the property that is passed through in this mapping.
    /// </param>
    /// <param name='isRootMapping'>
    /// A value that indicates whether the current instance represents the root of the mapping hierarchy.
    /// </param>
    protected CollectionMapping(IMapping parentMapping,
                                PropertyInfo property,
                                bool isRootMapping) : base(parentMapping, property, isRootMapping)
    {
      this.CollectionKeyType = CollectionKeyType.Separate;
      this.DeserializeMinimumIndex = 0;
      this.DeserializeMaximumIndex = 49;
    }

    #endregion
  }
}

