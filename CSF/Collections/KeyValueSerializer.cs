//  
//  KeyValueSerializer.cs
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
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CSF.Collections
{
  /// <summary>
  /// An implementation of a <c>IKeyValueSerializer</c>.
  /// </summary>
  public class KeyValueSerializer<TObject> : IKeyValueSerializer<TObject> where TObject : class, new()
  {
    #region fields
    
    private IList<PropertyKeyAssociation<TObject>> _mappings;
    private string _listFormat;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets or sets the mappings that this instance is 'aware' of.
    /// </summary>
    /// <value>
    /// The mappings.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public IList<PropertyKeyAssociation<TObject>> Mappings
    {
      get {
        return _mappings;
      }
      set {
        if (value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _mappings = value;
        this.CheckInvariants();
      }
    }
    
    /// <summary>
    /// Gets or sets the format for serializing and deserializing collections of object instances.
    /// </summary>
    /// <value>
    /// The format string.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public string ListFormat
    {
      get {
        return _listFormat;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        try
        {
          String.Format(value, String.Empty, String.Empty);
        }
        catch(FormatException ex)
        {
          throw new FormatException("The list format is invalid.  It must contain two placeholders: {0} and {1}", ex);
        }
        
        _listFormat = value;
        this.CheckInvariants();
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Checks any invariants (constraints) on this instance to ensure that its state is valid.
    /// </summary>
    /// <exception cref='InvalidOperationException'>
    /// Is thrown when an operation cannot be performed.
    /// </exception>
    private void CheckInvariants()
    {
      IList<PropertyInfo> properties = new List<PropertyInfo>();
      
      foreach(PropertyKeyAssociation<TObject> association in this.Mappings)
      {
        if(properties.Contains(association.Property))
        {
          throw new InvalidOperationException(String.Format("Duplicate mapping detected: {0}",
                                                            association.Property.Name));
        }
        properties.Add(association.Property);
      }
    }
    
    /// <summary>
    /// Clones a <see cref="NameValueCollection"/> as a string-based dictionary.
    /// </summary>
    /// <returns>
    /// The cloned collection.
    /// </returns>
    /// <param name='collection'>
    /// The collection to clone
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    private IDictionary<string, string> CloneCollection(NameValueCollection collection)
    {
      IDictionary<string, string> collectionCopy = new Dictionary<string, string>();
      
      if(collection == null)
      {
        throw new ArgumentNullException ("collection");
      }
      
      foreach(string key in collection.Keys)
      {
        collectionCopy.Add(key, collection[key]);
      }
      
      return collectionCopy;
    }
    
    #endregion
    
    #region deserialization
    
    /// <summary>
    /// Deserialize the specified collection into an instance of the target object.
    /// </summary>
    /// <returns>
    /// A deserialized object instance, or a default reference if deserialization was unsuccessful.
    /// </returns>
    /// <param name='collection'>
    /// The collection to deserialize.
    /// </param>
    public TObject Deserialize (NameValueCollection collection)
    {
      return this.Deserialize(this.CloneCollection(collection));
    }
  
    /// <summary>
    /// Deserialize the specified collection into an instance of the target object.
    /// </summary>
    /// <returns>
    /// A deserialized object instance, or a default reference if deserialization was unsuccessful.
    /// </returns>
    /// <param name='collection'>
    /// The collection to deserialize.
    /// </param>
    public TObject Deserialize (IDictionary<string, string> collection)
    {
      return this.DeserializeInstance(collection, "{0}");
    }
  
    /// <summary>
    /// Deserializes a collection of object instances from a collection.
    /// </summary>
    /// <returns>
    /// A collection of deserialized object instances, which may be empty.
    /// </returns>
    /// <param name='collection'>
    /// The collection to deserialize from.
    /// </param>
    public IList<TObject> DeserializeMany (NameValueCollection collection)
    {
      return this.DeserializeMany(this.CloneCollection(collection));
    }
  
    /// <summary>
    /// Deserializes a collection of object instances from a collection.
    /// </summary>
    /// <returns>
    /// A collection of deserialized object instances, which may be empty.
    /// </returns>
    /// <param name='collection'>
    /// The collection to deserialize from.
    /// </param>
    public IList<TObject> DeserializeMany (IDictionary<string, string> collection)
    {
      return this.DeserializeMany(collection, this.ListFormat);
    }
  
    /// <summary>
    /// Deserializes a collection of object instances from a collection.
    /// </summary>
    /// <returns>
    /// A collection of deserialized object instances, which may be empty.
    /// </returns>
    /// <param name='collection'>
    /// The collection to deserialize from.
    /// </param>
    /// <param name='formatString'>
    /// The format for deserializing collections of objects.
    /// </param>
    public IList<TObject> DeserializeMany (NameValueCollection collection, string formatString)
    {
      return this.DeserializeMany(this.CloneCollection(collection), this.ListFormat);
    }
  
    /// <summary>
    /// Deserializes a collection of object instances from a collection.
    /// </summary>
    /// <returns>
    /// A collection of deserialized object instances, which may be empty.
    /// </returns>
    /// <param name='collection'>
    /// The collection to deserialize from.
    /// </param>
    /// <param name='formatString'>
    /// The format for deserializing collections of objects.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='FormatException'>
    /// Represents errors caused by passing incorrectly formatted arguments or invalid format specifiers to methods.
    /// </exception>
    public IList<TObject> DeserializeMany (IDictionary<string, string> collection, string formatString)
    {
      Dictionary<string, TObject> foundObjects = new Dictionary<string, TObject>();
      List<TObject> output = new List<TObject>();
      
      if(collection == null)
      {
        throw new ArgumentNullException ("collection");
      }
      else if(formatString == null)
      {
        throw new ArgumentNullException ("formatString");
      }
      
      try
      {
        String.Format(formatString, String.Empty, String.Empty);
      }
      catch(FormatException ex)
      {
        throw new FormatException("The list format is invalid.  It must contain two placeholders: {0} and {1}", ex);
      }
      
      this.CheckInvariants();
      
      foreach(PropertyKeyAssociation<TObject> association in this.Mappings)
      {
        IList<string> instancesFound = this.FindInstances(collection, formatString, association.Key);
        
        foreach(string identifier in instancesFound)
        {
          // If we have already deserialized this object then skip it
          if(foundObjects.ContainsKey(identifier))
          {
            continue;
          }
          
          string keyMask = String.Format(formatString, "{0}", identifier);
          TObject instance = this.DeserializeInstance(collection, keyMask);
          
          if(instance != null)
          {
            foundObjects.Add(identifier, instance);
          }
        }
      }
      
      output.AddRange(foundObjects.Values);
      return output;
    }
    
    /// <summary>
    /// Deserializes a single object instance.
    /// </summary>
    /// <returns>
    /// The deserialized instance, or a null reference if no instance could be deserialized.
    /// </returns>
    /// <param name='collection'>
    /// The collection to deserialize from
    /// </param>
    /// <param name='keyMask'>
    /// A 'mask', formatted as a valid input to <c>String.Format</c>, containing the single placeholder <c>{0}</c>.
    /// This is used to detect the property-keys within the <paramref name="collection"/>.
    /// </param>
    private TObject DeserializeInstance(IDictionary<string, string> collection, string keyMask)
    {
      TObject output = new TObject();
      bool success = false;
      
      foreach(PropertyKeyAssociation<TObject> association in this.Mappings)
      {
        string key = String.Format(keyMask, association.Key);
        if(collection.ContainsKey(key))
        {
          try
          {
            this.DeserializeValue(output, collection[key], association.Property);
            success = true;
          }
          catch(Exception)
          {
            if(association.Mandatory)
            {
              success = false;
              break;
            }
          }
        }
        else if(association.Mandatory)
        {
          success = false;
          break;
        }
      }
      
      return success? output : null;
    }
    
    /// <summary>
    /// Deserializes a single <see cref="String"/> value and stores it within the <paramref name="target"/>.
    /// </summary>
    /// <param name='target'>
    /// The target object to receive the value.
    /// </param>
    /// <param name='value'>
    /// The value to deserialize.
    /// </param>
    /// <param name='property'>
    /// The property to deserialize.
    /// </param>
    private void DeserializeValue(TObject target, string value, PropertyInfo property)
    {
      if (property == null)
      {
        throw new ArgumentNullException ("property");
      }
      else if (target == null)
      {
        throw new ArgumentNullException ("target");
      }
      else if (value == null)
      {
        throw new ArgumentNullException ("value");
      }
      
      property.SetValue(target, Convert.ChangeType(value, property.PropertyType), null);
    }
    
    /// <summary>
    /// Finds all of the possible keys within the <paramref name="collection"/> that match the
    /// <paramref name="formatString"/> for the specified <paramref name="propertyKey"/>.
    /// </summary>
    /// <returns>
    /// The instance identifiers discovered.
    /// </returns>
    /// <param name='collection'>
    /// The collection of values.
    /// </param>
    /// <param name='formatString'>
    /// The format string.
    /// </param>
    /// <param name='propertyKey'>
    /// The key of the property being searched for.
    /// </param>
    private IList<string> FindInstances(IDictionary<string, string> collection, string formatString, string propertyKey)
    {
      IList<string> output = new List<string>();
      
      string escapedFormat = String.Concat("^", Regex.Escape(formatString), "$");
      escapedFormat = escapedFormat.Replace(@"\{0}", "{0}").Replace(@"\{1}", "{1}");
      
      string instancePattern = String.Format(escapedFormat, Regex.Escape(propertyKey), "(.+)");
      Regex instanceFinder = new Regex(instancePattern);
      
      foreach(string key in collection.Keys)
      {
        Match potentialMatch = instanceFinder.Match(key);
        
        if(potentialMatch.Success)
        {
          output.Add(potentialMatch.Groups[1].Value);
        }
      }
      
      return output;
    }
    
    #endregion
    
    #region serialization
  
    /// <summary>
    /// Serialize the specified value into the given collection.
    /// </summary>
    /// <param name='value'>
    /// The object instance to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to receive the serialized data.
    /// </param>
    public void Serialize (TObject value, NameValueCollection collection)
    {
      this.Serialize(value, this.CloneCollection(collection));
    }
  
    /// <summary>
    /// Serialize the specified value into the given collection.
    /// </summary>
    /// <param name='value'>
    /// The object instance to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to receive the serialized data.
    /// </param>
    public void Serialize (TObject value, IDictionary<string, string> collection)
    {
      this.SerializeInstance(value, collection, "{0}");
    }
  
    /// <summary>
    /// Serialize a collection of values into the given <see cref="System.String"/> collection.
    /// </summary>
    /// <param name='values'>
    /// The collection of object instances/values to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to receive the serialized data.
    /// </param>
    public void SerializeMany (IEnumerable<TObject> values, NameValueCollection collection)
    {
      this.SerializeMany(values, this.CloneCollection(collection));
    }
  
    /// <summary>
    /// Serialize a collection of values into the given <see cref="System.String"/> collection.
    /// </summary>
    /// <param name='values'>
    /// The collection of object instances/values to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to receive the serialized data.
    /// </param>
    public void SerializeMany (IEnumerable<TObject> values, IDictionary<string, string> collection)
    {
      this.SerializeMany(values, collection, this.ListFormat);
    }
  
    /// <summary>
    /// Serialize a collection of values into the given <see cref="System.String"/> collection.
    /// </summary>
    /// <param name='values'>
    /// The collection of object instances/values to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to receive the serialized data.
    /// </param>
    /// <param name='formatString'>
    /// The format string for naming properties within the <paramref name="collection"/>.  This should be a valid
    /// input for <c>String.Format</c> with two placeholders: <c>{0}</c> and <c>{1}</c> where placeholder zero is the
    /// name of the property and placeholder one is a sequence identifier to uniquely identify the object instance in
    /// the collection.
    /// </param>
    public void SerializeMany (IEnumerable<TObject> values, NameValueCollection collection, string formatString)
    {
      this.SerializeMany(values, this.CloneCollection(collection), formatString);
    }
  
    /// <summary>
    /// Serialize a collection of values into the given <see cref="System.String"/> collection.
    /// </summary>
    /// <param name='values'>
    /// The collection of object instances/values to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to receive the serialized data.
    /// </param>
    /// <param name='formatString'>
    /// The format string for naming properties within the <paramref name="collection"/>.  This should be a valid
    /// input for <c>String.Format</c> with two placeholders: <c>{0}</c> and <c>{1}</c> where placeholder zero is the
    /// name of the property and placeholder one is a sequence identifier to uniquely identify the object instance in
    /// the collection.
    /// </param>
    public void SerializeMany (IEnumerable<TObject> values, IDictionary<string, string> collection, string formatString)
    {
      int i = 0;
      
      if(values == null)
      {
        throw new ArgumentNullException ("values");
      }
      else if(collection == null)
      {
        throw new ArgumentNullException ("collection");
      }
      else if(formatString == null)
      {
        throw new ArgumentNullException ("formatString");
      }
      
      try
      {
        String.Format(formatString, String.Empty, String.Empty);
      }
      catch(FormatException ex)
      {
        throw new FormatException("The list format is invalid.  It must contain two placeholders: {0} and {1}", ex);
      }
      
      this.CheckInvariants();
      
      foreach(TObject value in values)
      {
        this.SerializeInstance(value, collection, String.Format(formatString, "{0}", i++));
      }
    }
    
    /// <summary>
    /// Serializes a single object instance/value to the specified <paramref name="collection"/>.
    /// </summary>
    /// <param name='value'>
    /// The object instance to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to receive the serialized data.
    /// </param>
    /// <param name='keyMask'>
    /// The format string for naming properties within the collection.  This should be a valid input for
    /// <c>String.Format</c> with a single placeholder: <c>{0}</c>.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// <para>
    /// Thrown if the current <see cref="Mappings"/> indicate one or more mandatory properties but the corresponding
    /// properties on the <paramref name="value"/> are <c>null</c>.
    /// </para>
    /// <para>
    /// In this scenario, the state of <paramref name="collection"/> is undefined and should be considered corrupted.
    /// </para>
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="value"/> is <c>null</c>.
    /// </exception>
    private void SerializeInstance(TObject value, IDictionary<string, string> collection, string keyMask)
    {
      if(value == null)
      {
        throw new ArgumentNullException("value");
      }
      
      foreach(PropertyKeyAssociation<TObject> property in this.Mappings)
      {
        object propertyValue = property.Property.GetValue(value, null);
        string key = String.Format(keyMask, property.Key);
        
        if(propertyValue != null)
        {
          collection.Add(key, propertyValue.ToString());
        }
        else if(property.Mandatory)
        {
          throw new InvalidOperationException(String.Format("Cannot serialize '{0}', the corresponding value is null " +
                                                            "but this property is marked as mandatory.",
                                                            key));
        }
      }
    }
    
    #endregion
    
    #region mapping
    
    /// <summary>
    /// Maps a single property for serialization and deserialization.
    /// </summary>
    /// <returns>
    /// The current instance (permitting method-chaining).
    /// </returns>
    /// <param name='property'>
    /// A lambda expression indicating the property to map.
    /// </param>
    public IKeyValueSerializer<TObject> Map (Expression<Func<TObject, object>> property)
    {
      this.Mappings.Add(new PropertyKeyAssociation<TObject>(property));
      this.CheckInvariants();
      return this;
    }
  
    /// <summary>
    /// Maps a single property for serialization and deserialization.
    /// </summary>
    /// <returns>
    /// The current instance (permitting method-chaining).
    /// </returns>
    /// <param name='property'>
    /// A lambda expression indicating the property to map.
    /// </param>
    /// <param name='mandatory'>
    /// A value indicating whether or not a value for this property is mandatory when deserializing an object instance.
    /// </param>
    public IKeyValueSerializer<TObject> Map (Expression<Func<TObject, object>> property, bool mandatory)
    {
      this.Mappings.Add(new PropertyKeyAssociation<TObject>(property, mandatory));
      this.CheckInvariants();
      return this;
    }
  
    /// <summary>
    /// Maps a single property for serialization and deserialization.
    /// </summary>
    /// <returns>
    /// The current instance (permitting method-chaining).
    /// </returns>
    /// <param name='property'>
    /// A <see cref="PropertyInfo"/> indicating the property to map.
    /// </param>
    public IKeyValueSerializer<TObject> Map (PropertyInfo property)
    {
      this.Mappings.Add(new PropertyKeyAssociation<TObject>(property));
      this.CheckInvariants();
      return this;
    }
  
    /// <summary>
    /// Maps a single property for serialization and deserialization.
    /// </summary>
    /// <returns>
    /// The current instance (permitting method-chaining).
    /// </returns>
    /// <param name='property'>
    /// A <see cref="PropertyInfo"/> indicating the property to map.
    /// </param>
    /// <param name='mandatory'>
    /// A value indicating whether or not a value for this property is mandatory when deserializing an object instance.
    /// </param>
    public IKeyValueSerializer<TObject> Map (PropertyInfo property, bool mandatory)
    {
      this.Mappings.Add(new PropertyKeyAssociation<TObject>(property, mandatory));
      this.CheckInvariants();
      return this;
    }
  
    /// <summary>
    /// Maps a single property for serialization and deserialization.
    /// </summary>
    /// <returns>
    /// The current instance (permitting method-chaining).
    /// </returns>
    /// <param name='property'>
    /// A lambda expression indicating the property to map.
    /// </param>
    /// <param name='key'>
    /// The <see cref="String"/> key used to represent this property value when serializing or deserializing.
    /// </param>
    public IKeyValueSerializer<TObject> Map (Expression<Func<TObject, object>> property, string key)
    {
      this.Mappings.Add(new PropertyKeyAssociation<TObject>(property, key));
      this.CheckInvariants();
      return this;
    }
  
    /// <summary>
    /// Maps a single property for serialization and deserialization.
    /// </summary>
    /// <returns>
    /// The current instance (permitting method-chaining).
    /// </returns>
    /// <param name='property'>
    /// A lambda expression indicating the property to map.
    /// </param>
    /// <param name='key'>
    /// The <see cref="String"/> key used to represent this property value when serializing or deserializing.
    /// </param>
    /// <param name='mandatory'>
    /// A value indicating whether or not a value for this property is mandatory when deserializing an object instance.
    /// </param>
    public IKeyValueSerializer<TObject> Map (Expression<Func<TObject, object>> property, string key, bool mandatory)
    {
      this.Mappings.Add(new PropertyKeyAssociation<TObject>(property, key, mandatory));
      this.CheckInvariants();
      return this;
    }
  
    /// <summary>
    /// Maps a single property for serialization and deserialization.
    /// </summary>
    /// <param name='property'>
    /// A <see cref="PropertyInfo"/> indicating the property to map.
    /// </param>
    /// <param name='key'>
    /// The <see cref="String"/> key used to represent this property value when serializing or deserializing.
    /// </param>
    public IKeyValueSerializer<TObject> Map (PropertyInfo property, string key)
    {
      this.Mappings.Add(new PropertyKeyAssociation<TObject>(property, key));
      this.CheckInvariants();
      return this;
    }
  
    /// <summary>
    /// Maps a single property for serialization and deserialization.
    /// </summary>
    /// <returns>
    /// The current instance (permitting method-chaining).
    /// </returns>
    /// <param name='property'>
    /// A <see cref="PropertyInfo"/> indicating the property to map.
    /// </param>
    /// <param name='key'>
    /// The <see cref="String"/> key used to represent this property value when serializing or deserializing.
    /// </param>
    /// <param name='mandatory'>
    /// A value indicating whether or not a value for this property is mandatory when deserializing an object instance.
    /// </param>
    public IKeyValueSerializer<TObject> Map (PropertyInfo property, string key, bool mandatory)
    {
      this.Mappings.Add(new PropertyKeyAssociation<TObject>(property, key, mandatory));
      this.CheckInvariants();
      return this;
    }
  
    /// <summary>
    /// Sets <see cref="ListFormat"/> using the specified value.
    /// </summary>
    /// <returns>
    /// The current instance (permitting method-chaining).
    /// </returns>
    /// <param name='formatString'>
    /// The format string.
    /// </param>
    public IKeyValueSerializer<TObject> SetListFormat (string formatString)
    {
      this.ListFormat = formatString;
      return this;
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <c>KeyValueSerializer</c> class.
    /// </summary>
    public KeyValueSerializer ()
    {
      this.Mappings = new List<PropertyKeyAssociation<TObject>>();
      this.ListFormat = "{0}_{1}";
    }
    
    #endregion
  }
}

