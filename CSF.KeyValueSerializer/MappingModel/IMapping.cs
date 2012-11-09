//
//  IMapping.cs
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
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace CSF.KeyValueSerializer.MappingModel
{
  /// <summary>
  /// A 'base' interface for mapping data relating to a given type.
  /// </summary>
  public interface IMapping
  {
    #region mapping information

    /// <summary>
    /// Gets the 'parent' mapping within the mapping hierarchy.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property should be non-null for everything but the root of the serialization hierarchy.  The scenarios
    /// for this are:
    /// </para>
    /// <list type="bullets">
    /// <item>
    /// For a mapping that represents a property of a class-like (reference type) mapping, the parent mapping is the
    /// reference-type (class) mapping for the type that contains the relevant property.
    /// </item>
    /// <item>
    /// For a mapping of a class-like (reference type) mapping, in which that class mapping is mapped as something else
    /// via the <c>MapAs</c> property, the parent mapping is that class mapping.
    /// </item>
    /// <item>
    /// For the mapping of collection items (either reference or value type), the parent is the mapping of the
    /// collection itself.
    /// </item>
    /// </list>
    /// </remarks>
    /// <value>
    /// The parent mapping.
    /// </value>
    IMapping ParentMapping { get; }

    /// <summary>
    /// Gets a reference to the property that the current mapping relates to.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For a mapping that relates to a property of a class-like (reference type) mapping, this value contains a
    /// reference to that class property.  For mappings that represent the root of the mapping hierarchy, or that
    /// otherwise do not represent a property of a class-like mapping, this value returns a null reference.
    /// </para>
    /// </remarks>
    /// <value>
    /// The property, which may be null.
    /// </value>
    PropertyInfo Property { get; }

    /// <summary>
    /// Gets or sets a <see cref="IKeyNamingPolicy"/> for the current mapping instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property cannot be null under any circumstances, when a new mapping instance is constructed, it is
    /// initialised to an instance of <see cref="DefaultKeyNamingPolicy"/>.  Strict checks are in place to ensure that
    /// this cannot be set to a null reference.
    /// </para>
    /// </remarks>
    /// <value>
    /// The key-naming policy associated with the current mapping.
    /// </value>
    IKeyNamingPolicy KeyNamingPolicy { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="CSF.KeyValueSerializer.MappingModel.IMapping"/>
    /// is mandatory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A mandatory mapping behaves specially when serializing or deserializing.
    /// </para>
    /// <list type="bullets">
    /// <item>
    /// During serialization, if a serialization function produces a null result (instead of an empty string) then -
    /// if the mapping is marked mandatory - the addition of a serialization key is skipped altogether for that mapping.
    /// </item>
    /// <item>
    /// During serialization, if a 'child' mapping fails to serialize (for example, as above) and that child mapping is
    /// marked as mandatory then the serialization of the <see cref="ParentMapping"/> does not occur.  In this scenario,
    /// no keys/values are added to the output/serialized collection for the parent mapping, and the parent mapping is
    /// in-turn considered to have failed serialization.  If an entire hierarchy is marked mandatory, this can result in
    /// the entire serialization process being aborted.
    /// </item>
    /// <item>
    /// During deserialization, if any 'child' mapping fails to deserialize (for example, if no matching key was found,
    /// or the deserialization function raises an exception) then the deserialization of the <see cref="ParentMapping"/> 
    /// does not occur.  In this scenario the deserialization of the parent mapping is aborted and a default value
    /// (for example a null reference) is used as its deserialization result.  In an entire hierarchy is marked
    /// mandatory, this can result in the entire deserialization process being aborted (and a null reference returned
    /// as the result).
    /// </item>
    /// </list>
    /// </remarks>
    /// <value>
    /// <c>true</c> if this mapping is mandatory; otherwise, <c>false</c>.
    /// </value>
    bool Mandatory { get; set; }

    /// <summary>
    /// Gets or sets a 'flag key' for use when serializing or deserializing this mapping instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A flag key and the related <see cref="FlagValue"/> control when a mapping is serialized/deserialized.
    /// </para>
    /// <list type="bullets">
    /// <item>
    /// When serializing, if serialization is successful then an additional item is written to the serialized output,
    /// using this key and either the flag value or (if the flag value is <c>null</c>) the string <c>True</c>.
    /// </item>
    /// <item>
    /// When deserializing, the deserialization not attempted (and thus considered a failure) unless an entry is present
    /// within the input data, found within this key.  That data must either match the flag value or (if the flag value
    /// is <c>null</c>) it must be any non-null/non-empty string.
    /// </item>
    /// </list>
    /// <para>
    /// In order to be valid, a mapping may have any of the following states.  Any other states will cause a validation
    /// failure.
    /// </para>
    /// <list type="bullets">
    /// <item>Both <see cref="FlagKey"/> and <see cref="FlagValue"/> equal to <c>null</c> or an empty string</item>
    /// <item><see cref="FlagKey"/> non-null/non-empty and <see cref="FlagValue"/> <c>null</c> or an empty string</item>
    /// <item>Both <see cref="FlagKey"/> and <see cref="FlagValue"/> non-null/non-empty</item>
    /// </list>
    /// </remarks>
    /// <value>
    /// The 'flag key' value.
    /// </value>
    string FlagKey { get; set; }

    /// <summary>
    /// Gets or sets a 'flag value' for use when serializing or deserializing this mapping instance.
    /// </summary>
    /// <seealso cref="FlagKey"/>
    /// <value>
    /// The 'flag value' value.
    /// </value>
    string FlagValue { get; set; }

    #endregion

    #region methods

    /// <summary>
    /// Creates a new key-naming policy and attaches it to the current mapping instance.
    /// </summary>
    /// <typeparam name='TPolicy'>
    /// The type of key-naming policy to create.
    /// </typeparam>
    /// <exception cref="InvalidOperationException">
    /// Thrown if there was any kind of error whilst using the default factory method to create an instance of
    /// <typeparamref name="TPolicy"/>, such as if the type does not expose a public constructor that takes a single
    /// parameter, of type <see cref="IMapping"/>.
    /// </exception>
    void AttachKeyNamingPolicy<TPolicy>() where TPolicy : IKeyNamingPolicy;

    /// <summary>
    /// Creates a new key-naming policy and attaches it to the current mapping instance.
    /// </summary>
    /// <param name='factoryMethod'>
    /// A factory method that creates the naming policy.
    /// </param>
    /// <typeparam name='TPolicy'>
    /// The type of key-naming policy to create.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="factoryMethod"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if there was any kind of error whilst using the provided factory method to create an instance of
    /// <typeparamref name="TPolicy"/>.  This also includes the scenario where the factory method returns a null
    /// reference instead of a key naming policy instance.
    /// </exception>
    void AttachKeyNamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod) where TPolicy : IKeyNamingPolicy;

    /// <summary>
    /// Validates this mapping instance. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This base method is designed to be overridden and expanded upon by derived types.
    /// </para>
    /// </remarks>
    /// <exception cref="InvalidMappingException">
    /// Thrown if the mapping is in an invalid state and is certain to be useless for both serialization and
    /// deserialization.
    /// </exception>
    void Validate();

    /// <summary>
    /// Gets the child mapping for the current mapping instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This overload, that does not use a property identifier, gets the map-as mapping for one of:
    /// </para>
    /// <list type="bullets">
    /// <item>A class-like (reference type) mapping that is being mapped as another mapping.</item>
    /// <item>
    /// A collection mapping (value-type of reference-type).  The mapping returned is that of the collection item, for
    /// a value-type collection this will be an 'endpoint' mapping (simple or composite) but for a reference-type
    /// collection this will be a class-type mapping which will have child mappings of its own.
    /// </item>
    /// </list>
    /// <para>
    /// This method will always either return a non-null mapping or raise an exception.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The child mapping instance.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown if the current mapping instance does not support child mappings in this manner.  For example, if the
    /// current mapping is a simple or composite mapping.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the current mapping instance provisionally supports getting a child mapping in this manner but its
    /// state means that such a mapping is not applicable.
    /// </exception>
    IMapping GetMapping();

    /// <summary>
    /// Gets the child mapping for the current mapping instance, identified by a property expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This overload, that uses a property identifier, gets 'child' mappings for class-like (reference-type) mappings.
    /// </para>
    /// <para>
    /// This method will always either return a non-null mapping or raise an exception.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The child mapping instance.
    /// </returns>
    /// <param name="property">
    /// An expression that identifies a property.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// Thrown if the current mapping instance does not support child mappings in this manner.  Specifically, if this
    /// mapping is anything but a class-like mapping.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the current mapping instance provisionally supports getting a child mapping in this manner but its
    /// state means that such a mapping is not applicable.
    /// </exception>
    IMapping GetMapping<TObject>(Expression<Func<TObject, object>> property);

    /// <summary>
    /// Gets the name of the serialization/deserialization 'key' that will be used to serialize/deserialize the
    /// current mapping.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method will always either return a non-null value or raise an exception.  It will never return null.
    /// It is only truly applicable for simple mappings.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The string 'key'.
    /// </returns>
    /// <param name='collectionIndices'>
    /// A collection of integer 'collection indices' for any collection-type mappings that have been passed-through.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// Thrown if the current mapping instance does not support getting a key in this manner (if it is anything but a
    /// simple mapping).
    /// </exception>
    string GetKeyName(params int[] collectionIndices);

    /// <summary>
    /// Gets the name of the serialization/deserialization 'key' that will be used to serialize/deserialize a component
    /// of the current mapping.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method will always either return a non-null value or raise an exception.  It will never return null.
    /// It is only truly applicable for composite mappings.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The string 'key'.
    /// </returns>
    /// <param name='componentIdentifier'>
    /// The identifier for a component of the associated composite mapping.
    /// </param>
    /// <param name='collectionIndices'>
    /// A collection of integer 'collection indices' for any collection-type mappings that have been passed-through.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// Thrown if the current mapping instance does not support getting a key in this manner (if it is anything but a
    /// composite mapping).
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if the composite mapping does not contain a component matching the given identifier.
    /// </exception>
    string GetKeyName(object componentIdentifier, params int[] collectionIndices);

    #endregion

    #region serialization

    /// <summary>
    /// Serialize the specified data, exposing the serialized results as an output parameter.
    /// </summary>
    /// <param name='data'>
    /// The object graph to serialize, the root of which should be an object instance that corresponds to the current
    /// mapping.
    /// </param>
    /// <param name='result'>
    /// The dictionary of string values containing the serialized data that is created from the current mapping
    /// instance.
    /// </param>
    /// <param name='collectionIndices'>
    /// A collection of integers, indicating the indices of any collection mappings passed-through during the
    /// serialization process.
    /// </param>
    /// <returns>
    /// A value that indicates whether or not the serialization was successful.
    /// </returns>
    bool Serialize(object data, out IDictionary<string,string> result, int[] collectionIndices);

    /// <summary>
    /// Deserializes the specified data and exposes the result as an output parameter.
    /// </summary>
    /// <returns>
    /// A value that indicates whether deserialization was successful or not.
    /// </returns>
    /// <param name='data'>
    /// The dictionary/collection of string data to deserialize.
    /// </param>
    /// <param name='result'>
    /// The output/deserialized object instance. If the return value is false (unsuccessful deserialization) then the
    /// output value of this parameter is undefined.  Otherwise this parameter exposes an object graph representing the
    /// deserialized data.
    /// </param>
    /// <param name='collectionIndices'>
    /// A collection of integers, indicating the indices of any collection mappings passed-through during the
    /// deserialization process.
    /// </param>
    bool Deserialize(IDictionary<string,string> data, out object result, int[] collectionIndices);

    #endregion
  }
}

