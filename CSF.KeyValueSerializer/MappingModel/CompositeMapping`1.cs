//
//  CompositePropertyMapping.cs
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
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CSF.KeyValueSerializer.MappingModel
{
  /// <summary>
  /// Implementation of a composite mapping.
  /// </summary>
  /// <remarks>
  /// <para>
  /// In a composite property mapping, the actual object/value being mapped is created from values found within
  /// multiple keys.  Each of these pieces used is called a 'component'.
  /// </para>
  /// </remarks>
  public class CompositeMapping<TValue> : MappingBase<TValue>, ICompositeMapping<TValue>
  {
    #region fields

    private IDictionary<object, ICompositeComponentMapping<TValue>> _components;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the function used to deserialize the property value from a dictionary of string values, indexed by
    /// the component identifiers from this mapping. 
    /// </summary>
    /// <value>
    /// A method body containing the deserialization function. 
    /// </value>
    public Func<IDictionary<object, string>, TValue> DeserializationFunction
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets a collection of the components that make up this mapping and their respective mappings.
    /// </summary>
    /// <value>
    ///  A collection of the components, indexed by their identifiers. 
    /// </value>
    public IDictionary<object, ICompositeComponentMapping<TValue>> Components
    {
      get {
        return _components;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _components = value;
      }
    }

    #endregion

    #region methods

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
    public override bool Deserialize(IDictionary<string, string> data,
                                     out TValue result,
                                     params int[] collectionIndices)
    {
      bool output = false;

      result = default(TValue);

      this.Validate();

      if(this.SatisfiesFlag(data))
      {
        IDictionary<object, string> values = new Dictionary<object, string>();

        foreach(object identifier in this.Components.Keys)
        {
          string key = this.Components[identifier].GetKeyName(collectionIndices);
          if(data.ContainsKey(key))
          {
            values.Add(identifier, data[key]);
          }
        }

        if(values.Count > 0)
        {
          try
          {
            result = this.DeserializationFunction(values);
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
    public override bool Serialize(TValue data, out IDictionary<string,string> result, int[] collectionIndices)
    {
      bool output = true;
      result = new Dictionary<string, string>();

      this.Validate();

      foreach(ICompositeComponentMapping<TValue> component in this.Components.Values)
      {
        string serialized = null;

        try
        {
          serialized = component.SerializationFunction(data);
          if(serialized == null)
          {
            output = false;
          }
        }
        catch(Exception) { output = false; }

        if(output)
        {
          result.Add(this.GetKeyName(component.ComponentIdentifier, collectionIndices), serialized);
        }
        else
        {
          break;
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
    /// Gets the name of the 'key' that is used for the current mapping.
    /// </summary>
    /// <returns>
    /// The key name.
    /// </returns>
    /// <param name='componentIdentifier'>
    /// The identifier for a component of a composite mapping.
    /// </param>
    /// <param name='collectionIndices'>
    /// A collection of integer 'collection indices' for any collection-type mappings that have been passed-through.
    /// </param>
    public override string GetKeyName (object componentIdentifier, params int[] collectionIndices)
    {
      if(!this.Components.ContainsKey(componentIdentifier))
      {
        throw new ArgumentException("This composite mapping does not contain the specified component.",
                                    "componentIdentifier");
      }

      return this.Components[componentIdentifier].GetKeyName(collectionIndices);
    }

    /// <summary>
    /// Validates this mapping instance. 
    /// </summary>
    public override void Validate ()
    {
      base.Validate ();

      if(this.Components.Count == 0)
      {
        throw new InvalidMappingException("A composite mapping must contain at least one component to be valid.");
      }
      else if(this.DeserializationFunction == null
              && this.Components.Values.Any(x => x.SerializationFunction == null))
      {
        throw new InvalidMappingException("This composite mapping is 'useless'.  It must either expose a " +
                                          "deserialization function or all of its components must expose " +
                                          "serialization functions.");
      }

      // There is more to do here!
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the composite property mapping class.
    /// </summary>
    /// <param name='parentMapping'>
    /// Parent mapping.
    /// </param>
    /// <param name='property'>
    /// Property.
    /// </param>
    public CompositeMapping(IMapping parentMapping, PropertyInfo property) : base(parentMapping, property, false)
    {
      this.DeserializationFunction = null;
      this.Components = new Dictionary<object, ICompositeComponentMapping<TValue>>();
    }

    #endregion
  }
}

