//  
//  ICollectionSerializer.cs
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace CSF.Collections
{
  /// <summary>
  /// Interface for a type that can provide serialization and deserialization of types to and from collections that
  /// represent key/value pairs of information.
  /// </summary>
  /// <typeparam name="TObject">
  /// The type of object that this serializer instance deals with.
  /// </typeparam>
  /// <remarks>
  /// <para>
  /// This type uses mappings between properties of a type and string-based 'keys' within a collection that represents
  /// key/value pairs of information.  These mappings provide a mechanism of automatically deserializing object
  /// instances from such collections and also serializing object instances to these collections.
  /// </para>
  /// <para>
  /// This is particularly useful in a web-based environment, for example, in which HTTP POST and GET data arrives as
  /// <see cref="NameValueCollection"/> instances.
  /// </para>
  /// </remarks>
  public interface IKeyValueSerializer<TObject> where TObject : new()
  {
    #region properties
    
    /// <summary>
    /// Gets a collection of the mappings that this instance is 'aware' of.
    /// </summary>
    /// <value>
    /// The mappings.
    /// </value>
    IList<PropertyKeyAssociation<TObject>> Mappings { get; }
    
    #endregion
    
    #region deserialization
    
    /// <summary>
    /// Deserialize an object instance from the specified collection.
    /// </summary>
    /// <returns>
    /// An instance of <typeparamref name="TObject" />, or a default value if no instance could be deserialized
    /// (possibly null).
    /// </returns>
    /// <param name='collection'>
    /// The key/value-based collection to deserialize from.
    /// </param>
    TObject Deserialize(NameValueCollection collection);
    
    /// <summary>
    /// Deserialize an object instance from the specified collection.
    /// </summary>
    /// <returns>
    /// An instance of <typeparamref name="TObject" />, or a default value if no instance could be deserialized
    /// (possibly null).
    /// </returns>
    /// <param name='collection'>
    /// The key/value-based collection to deserialize from.
    /// </param>
    TObject Deserialize(IDictionary<string, string> collection);
    
    /// <summary>
    /// Deserializes many object instances from the specified collection.
    /// </summary>
    /// <returns>
    /// A collection of <typeparamref name="TObject" />, which may be empty if no instances could be deserialized.
    /// </returns>
    /// <param name='collection'>
    /// The key/value-based collection to deserialize from.
    /// </param>
    IList<TObject> DeserializeMany(NameValueCollection collection);
    
    /// <summary>
    /// Deserializes many object instances from the specified collection.
    /// </summary>
    /// <returns>
    /// A collection of <typeparamref name="TObject" />, which may be empty if no instances could be deserialized.
    /// </returns>
    /// <param name='collection'>
    /// The key/value-based collection to deserialize from.
    /// </param>
    IList<TObject> DeserializeMany(IDictionary<string, string> collection);
    
    /// <summary>
    /// Deserializes many object instances from the specified collection.
    /// </summary>
    /// <returns>
    /// A collection of <typeparamref name="TObject" />, which may be empty if no instances could be deserialized.
    /// </returns>
    /// <param name='collection'>
    /// The key/value-based collection to deserialize from.
    /// </param>
    /// <param name='formatString'>
    /// A string that represents the format to use for deserializing multiple objects.  It should be a regular
    /// expression pattern that contains two named capturing groups: <c>key</c> and <c>id</c>.
    /// </param>
    IList<TObject> DeserializeMany(NameValueCollection collection, string formatString);
    
    /// <summary>
    /// Deserializes many object instances from the specified collection.
    /// </summary>
    /// <returns>
    /// A collection of <typeparamref name="TObject" />, which may be empty if no instances could be deserialized.
    /// </returns>
    /// <param name='collection'>
    /// The key/value-based collection to deserialize from.
    /// </param>
    /// <param name='formatString'>
    /// A string that represents the format to use for deserializing multiple objects.  It should be a regular
    /// expression pattern that contains two named capturing groups: <c>key</c> and <c>id</c>.
    /// </param>
    IList<TObject> DeserializeMany(IDictionary<string, string> collection, string formatString);
    
    #endregion
    
    #region serialization
    
    /// <summary>
    /// Serializes the given value to the specified collection.
    /// </summary>
    /// <param name='value'>
    /// An instance of <typeparamref name="TObject" /> to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to serialize to.
    /// </param>
    void Serialize(TObject value, IDictionary<string, string> collection);
    
    /// <summary>
    /// Serializes the given value to the specified collection.
    /// </summary>
    /// <param name='value'>
    /// An instance of <typeparamref name="TObject" /> to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to serialize to.
    /// </param>
    void Serialize(TObject value, NameValueCollection collection);
    
    /// <summary>
    /// Serializes a collection of values to the specified collection.
    /// </summary>
    /// <param name='values'>
    /// A collection of <typeparamref name="TObject" /> to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to serialize to.
    /// </param>
    void SerializeMany(IEnumerable<TObject> values, IDictionary<string, string> collection);
    
    /// <summary>
    /// Serializes a collection of values to the specified collection.
    /// </summary>
    /// <param name='values'>
    /// A collection of <typeparamref name="TObject" /> to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to serialize to.
    /// </param>
    void SerializeMany(IEnumerable<TObject> values, NameValueCollection collection);
    
    /// <summary>
    /// Serializes a collection of values to the specified collection.
    /// </summary>
    /// <param name='values'>
    /// A collection of <typeparamref name="TObject" /> to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to serialize to.
    /// </param>
    /// <param name='formatString'>
    /// A string that indicates the format of the keys to use in serializing.
    /// </param>
    void SerializeMany(IEnumerable<TObject> values, IDictionary<string, string> collection, string formatString);
    
    /// <summary>
    /// Serializes a collection of values to the specified collection.
    /// </summary>
    /// <param name='values'>
    /// A collection of <typeparamref name="TObject" /> to serialize.
    /// </param>
    /// <param name='collection'>
    /// The collection to serialize to.
    /// </param>
    /// <param name='formatString'>
    /// A string that indicates the format of the keys to use in serializing.
    /// </param>
    void SerializeMany(IEnumerable<TObject> values, NameValueCollection collection, string formatString);
    
    #endregion
    
    #region mapping
  
    /// <summary>
    /// Maps a single property for serialization and/or deserialization.
    /// </summary>
    /// <returns>
    /// An instance of the serializer being worked on, such that calls may be chained.
    /// </returns>
    /// <param name='property'>
    /// A lambda expression or other reference that identifies the property to map.
    /// </param>
    IKeyValueSerializer<TObject> Map(Expression<Func<TObject, object>> property);
    
    /// <summary>
    /// Maps a single property for serialization and/or deserialization.
    /// </summary>
    /// <returns>
    /// An instance of the serializer being worked on, such that calls may be chained.
    /// </returns>
    /// <param name='property'>
    /// A lambda expression or other reference that identifies the property to map.
    /// </param>
    /// <param name='mandatory'>
    /// Mandatory.
    /// </param>
    IKeyValueSerializer<TObject> Map(Expression<Func<TObject, object>> property, bool mandatory);
    
    /// <summary>
    /// Maps a single property for serialization and/or deserialization.
    /// </summary>
    /// <returns>
    /// An instance of the serializer being worked on, such that calls may be chained.
    /// </returns>
    /// <param name='property'>
    /// A <see cref="PropertyInfo"/> that indicates the property to map.
    /// </param>
    IKeyValueSerializer<TObject> Map(PropertyInfo property);
    
    /// <summary>
    /// Maps a single property for serialization and/or deserialization.
    /// </summary>
    /// <returns>
    /// An instance of the serializer being worked on, such that calls may be chained.
    /// </returns>
    /// <param name='property'>
    /// A <see cref="PropertyInfo"/> that indicates the property to map.
    /// </param>
    /// <param name='mandatory'>
    /// A value indicating whether or not this property is mandatory.  If true then deserialization will be considered
    /// a failure if no value for this property is found.
    /// </param>
    IKeyValueSerializer<TObject> Map(PropertyInfo property, bool mandatory);
  
    /// <summary>
    /// Maps a single property for serialization and/or deserialization.
    /// </summary>
    /// <returns>
    /// An instance of the serializer being worked on, such that calls may be chained.
    /// </returns>
    /// <param name='property'>
    /// A lambda expression or other reference that identifies the property to map.
    /// </param>
    /// <param name='key'>
    /// The string key for this property when serializing or deserializing.
    /// </param>
    IKeyValueSerializer<TObject> Map(Expression<Func<TObject, object>> property, string key);
    
    /// <summary>
    /// Maps a single property for serialization and/or deserialization.
    /// </summary>
    /// <returns>
    /// An instance of the serializer being worked on, such that calls may be chained.
    /// </returns>
    /// <param name='property'>
    /// A lambda expression or other reference that identifies the property to map.
    /// </param>
    /// <param name='key'>
    /// The string key for this property when serializing or deserializing.
    /// </param>
    /// <param name='mandatory'>
    /// A value indicating whether or not this property is mandatory.  If true then deserialization will be considered
    /// a failure if no value for this property is found.
    /// </param>
    IKeyValueSerializer<TObject> Map(Expression<Func<TObject, object>> property, string key, bool mandatory);
    
    /// <summary>
    /// Maps a single property for serialization and/or deserialization.
    /// </summary>
    /// <returns>
    /// An instance of the serializer being worked on, such that calls may be chained.
    /// </returns>
    /// <param name='property'>
    /// A <see cref="PropertyInfo"/> that indicates the property to map.
    /// </param>
    /// <param name='key'>
    /// The string key for this property when serializing or deserializing.
    /// </param>
    IKeyValueSerializer<TObject> Map(PropertyInfo property, string key);
    
    /// <summary>
    /// Maps a single property for serialization and/or deserialization.
    /// </summary>
    /// <returns>
    /// An instance of the serializer being worked on, such that calls may be chained.
    /// </returns>
    /// <param name='property'>
    /// A <see cref="PropertyInfo"/> that indicates the property to map.
    /// </param>
    /// <param name='key'>
    /// The string key for this property when serializing or deserializing.
    /// </param>
    /// <param name='mandatory'>
    /// A value indicating whether or not this property is mandatory.  If true then deserialization will be considered
    /// a failure if no value for this property is found.
    /// </param>
    IKeyValueSerializer<TObject> Map(PropertyInfo property, string key, bool mandatory);
    
    /// <summary>
    /// Passes a default format for this instance to use when serializing/deserializing collections of object instances.
    /// </summary>
    /// <returns>
    /// An instance of the serializer being worked on, such that calls may be chained.
    /// </returns>
    /// <param name='formatString'>
    /// A string that represents the format to use for deserializing/serializing multiple objects.  It should be a
    /// regular expression pattern that contains two named capturing groups: <c>key</c> and <c>id</c>.
    /// </param>
    IKeyValueSerializer<TObject> ListFormat(string formatString);
    
    #endregion
  }
}

