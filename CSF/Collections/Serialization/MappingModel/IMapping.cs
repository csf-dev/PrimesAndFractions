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

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// A 'base' interface for mapping data relating to a given type.
  /// </summary>
  public interface IMapping
  {
    /// <summary>
    /// Gets the naming rule for this property, which is possibly only a default naming rule.
    /// </summary>
    /// <value>
    /// The naming rule.
    /// </value>
    IKeyNamingPolicy KeyNamingPolicy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is the root mapping.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is the root mapping; otherwise, <c>false</c>.
    /// </value>
    bool IsRootMapping { get; }

    /// <summary>
    /// Gets the 'parent' mapping that 'contains' the current mapping.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property should be non-null for everything but the root of the serialization hierarchy.  For every other
    /// mapping this property contains the mapping that is the immediate parent in the hierarchy.
    /// </para>
    /// <list type="bullets">
    /// <item>
    /// For a mapping of a property that exists on a class-like mapping, the parent is the mapping of that
    /// class-like item.
    /// </item>
    /// <item>
    /// For the mapping of composite components, the parent is the composite property mapping.
    /// </item>
    /// <item>
    /// For the mapping of collection items (either reference or value type), the parent is the collection mapping.
    /// </item>
    /// </list>
    /// </remarks>
    /// <value>
    /// The parent mapping.
    /// </value>
    IMapping ParentMapping { get; }

    /// <summary>
    /// Gets the property that this mapping relates to.
    /// </summary>
    /// <value>
    /// The property.
    /// </value>
    PropertyInfo Property { get; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Collections.Serialization.MappingModel.IMapping"/> is
    /// mandatory.
    /// </summary>
    /// <value>
    /// <c>true</c> if mandatory; otherwise, <c>false</c>.
    /// </value>
    bool Mandatory { get; set; }

    /// <summary>
    /// Gets a value that indicates a flag-key if it is in-use for this mapping.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This option indicates that the property or type represented by the current mapping carries an additional 'flag'
    /// value within the string collection to indicate its presence.  When deserialising, if this flag is not present or
    /// carries an incorrect value then the deserialisation of this property/type is aborted as if no value could be
    /// deserialized.  When serialising, if serialization is successful then a value is stored within this flag key, to
    /// indicate that a value is present.
    /// </para>
    /// <para>
    /// If this property is non-null and non-empty then a flag is expected.  If <see cref="FlagValue"/> is null or empty
    /// then any non-null/non-empty value is accepted when deserialising (and the value <c>True</c> is used when
    /// serializing).  If <see cref="FlagValue"/> is non-null and non-empty then that value is required/used instead.
    /// </para>
    /// </remarks>
    /// <value>
    /// The flag key.
    /// </value>
    string FlagKey { get; set; }

    /// <summary>
    /// Gets a value that is required within <see cref="FlagKey"/> when deserializing, and that is written to
    /// the flag-key when serializing.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property indicates the value that is required within the <see cref="FlagKey"/> in order to satisfy the
    /// flag-requirement.  It has no effect if <see cref="FlagKey"/> is null or an empty string.
    /// </para>
    /// </remarks>
    /// <value>
    /// The flag value.
    /// </value>
    string FlagValue { get; set; }

    /// <summary>
    /// Validates this mapping instance.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the mapping is not valid.
    /// </exception>
    void Validate();

    /// <summary>
    /// Deserialize the specified data as an object instance.
    /// </summary>
    /// <returns>
    /// A value that indicates whether deserialization was successful or not.
    /// </returns>
    /// <param name='data'>
    /// The dictionary/collection of string data to deserialize from.
    /// </param>
    /// <param name='result'>
    /// The output/deserialized object instance.  If the return value is false (unsuccessful deserialization) then the
    /// output value of this parameter is undefined.
    /// </param>
    /// <param name='collectionIndices'>
    /// A collection of integers, indicating the indices of any collection mappings passed-through during the
    /// </param>
    bool Deserialize(IDictionary<string,string> data, out object result, int[] collectionIndices);

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
    void Serialize(object data, ref IDictionary<string,string> result, int[] collectionIndices);

    /// <summary>
    /// Gets the mapping for the current item (the map-as).
    /// </summary>
    /// <returns>
    /// The mapping.
    /// </returns>
    IMapping GetMapping();

    /// <summary>
    /// Gets the mapping for the given property.
    /// </summary>
    /// <returns>
    /// The mapping.
    /// </returns>
    /// <param name='property'>
    /// An expression that indicates the property to retrieve the mapping for.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type associated with the current mapping (the type that 'hosts' the property).
    /// </typeparam>
    IMapping GetMapping<TObject>(Expression<Func<TObject, object>> property);

    /// <summary>
    /// Gets the name of the 'key' that is used for the current mapping.
    /// </summary>
    /// <returns>
    /// The key name.
    /// </returns>
    /// <param name='collectionIndices'>
    /// A collection of integer 'collection indices' for any collection-type mappings that have been passed-through.
    /// </param>
    string GetKeyName(params int[] collectionIndices);

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
    string GetKeyName(object componentIdentifier, params int[] collectionIndices);
  }
}

