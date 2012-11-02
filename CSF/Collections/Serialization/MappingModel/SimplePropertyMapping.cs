//
//  SimplePropertyMapping.cs
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
  /// Represents a simple value-to-property mapping.
  /// </summary>
  public class SimplePropertyMapping<TValue> : MappingBase, ISimpleMapping<TValue>
  {
    #region ISimpleMapping implementation

    /// <summary>
    ///  Gets or sets the function used to deserialize the property value from a string. 
    /// </summary>
    /// <value>
    ///  A method body containing the deserialization function. 
    /// </value>
    public virtual Func<string, TValue> DeserializationFunction
    {
      get;
      set;
    }

    /// <summary>
    ///  Gets or sets the function used to serialize a string from the property value. 
    /// </summary>
    /// <value>
    ///  A method body containing the serialization function. 
    /// </value>
    public virtual Func<TValue, string> SerializationFunction
    {
      get;
      set;
    }

    /// <summary>
    ///  Validates this mapping instance. 
    /// </summary>
    public override void Validate ()
    {
      base.Validate();

      if(this.DeserializationFunction == null && this.SerializationFunction == null)
      {
        throw new InvalidMappingException("Property mapping does not have either a serialization or " +
                                          "deserialization function - this is invalid (a useless mapping).");
      }
    }

    /// <summary>
    /// Gets the collection key that corresponds to the data for this property. 
    /// </summary>
    /// <param name='collectionIndices'>
    /// Zero or more indices for collection-type mappings that are passed-through when traversing from the root of the
    /// serialization hierarchy to the associated mapping.
    /// </param>
    /// <returns>
    ///  The collection key. 
    /// </returns>
    public virtual string GetKeyName(params int[] collectionIndices)
    {
      return this.KeyNamingPolicy.GetKeyName(collectionIndices);
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
    public virtual bool Deserialize(IDictionary<string, string> data,
                                    out TValue result,
                                    params int[] collectionIndices)
    {
      bool output = false;

      result = default(TValue);

      if(this.MayDeserialize(data))
      {
        string keyName = this.GetKeyName(collectionIndices);
        if(data.ContainsKey(keyName))
        {
          try
          {
            result = this.DeserializationFunction(data[keyName]);
            output = true;
          }
          catch(Exception) {}
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
    public override bool Deserialize(IDictionary<string, string> data, out object result, params int[] collectionIndices)
    {
      TValue tempResult;
      bool output = this.Deserialize(data, out tempResult, collectionIndices);
      result = tempResult;
      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of this simple property-mapping class.
    /// </summary>
    /// <param name='property'>
    /// The property that this instance is associated with.
    /// </param>
    /// <param name='parentMapping'>
    /// The parent mapping.
    /// </param>
    public SimplePropertyMapping(IMapping parentMapping, PropertyInfo property) : base(parentMapping, property)
    {
      this.DeserializationFunction = (strVal => (TValue) Convert.ChangeType(strVal, typeof(TValue)));
      this.SerializationFunction = (val => (val != null)? val.ToString() : null);
    }

    /// <summary>
    /// Initializes a new instance of this simple property-mapping class.
    /// </summary>
    /// <param name='property'>
    /// The property that this instance is associated with.
    /// </param>
    /// <param name='parentMapping'>
    /// The parent mapping.
    /// </param>
    /// <param name='classMode'>
    /// A boolean that indicates whether this instance will operate in 'class mode' or not.
    /// </param>
    public SimplePropertyMapping(IMapping parentMapping,
                                 PropertyInfo property,
                                 bool classMode) : this(parentMapping, property)
    {
      if(classMode)
      {
        this.PermitNullParent = true;
        this.PermitNullProperty = true;
      }
    }

    #endregion
  }
}

