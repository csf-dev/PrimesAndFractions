//
//  IMappingHelper.cs
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

namespace CSF.KeyValueSerializer.MappingHelpers
{
  /// <summary>
  /// A 'base' interface for a mapping helper, providing functionality common to all mappings.
  /// </summary>
  public interface IMappingHelper
  {
    /// <summary>
    /// Indicates that this item (type or property) is mandatory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Where this option has been selected for a mapping then during serialization/deserialization, if the property or
    /// type represented by this mapping cannot be serialized/deserialized then the serialization/deserialization is
    /// aborted as a failure.  This has a different effect depending upon the scenario:
    /// </para>
    /// <para>
    /// Where the current mapping represents a property of a class-like mapping and the operation is deserialization
    /// then - if this mapped property cannot be deserialized then the deserialization of the entire 'parent type' is
    /// considered a failure.  If the 'parent type' is the root of the deserialization hierarchy then this means that
    /// the entire deserialization action is aborted as a failure but if the 'parent type' is not the root of the
    /// hierarchy then some deserialization results could be returned.  In the latter scenario, the 'parent type' will
    /// be missing from the output, as if it could not be deserialized.
    /// </para>
    /// <para>
    /// Where the current mapping represents a property of a class-like mapping and the operation is serialization
    /// then - if this mapped property cannot be serialized (for example, it contains a null value or the getter throws
    /// an exception) then the serialization of the entire 'parent type' is considered a failure.  If the 'parent type'
    /// is the root of the serialization hierarchy then this means that the entire serialization action is aborted as a
    /// failure but if the 'parent type' is not the root of the hierarchy then some serialization could occur.  In the
    /// latter scenario, the 'parent type' will be missing from the output, as if it could not be serialized.
    /// </para>
    /// <para>
    /// Where the current mapping represents the root type of the mapping hierarchy then this option has no meaning and
    /// selecting it will produce undefined results and emit a warning.
    /// </para>
    /// </remarks>
    void Mandatory();

    /// <summary>
    /// Indicates that when serializing/deserializing this instance, a 'flag' value should be used.
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
    /// This overload will accept any non-null/non-empty string value for the flag value when deserializing.  When
    /// serializing the value <c>True</c> is stored within this flag.
    /// </para>
    /// </remarks>
    /// <param name='key'>
    /// The key within the collection that serves as the flag.
    /// </param>
    void UsingFlag(string key);

    /// <summary>
    /// Indicates that when serializing/deserializing this instance, a 'flag' value should be used.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This option indicates that the property or type represented by the current mapping carries an additional 'flag'
    /// value within the string collection to indicate its presence.  When deserialising, if this flag is not present or
    /// carries an incorrect value then the deserialisation of this property/type is aborted as if no value could be
    /// deserialized.  When serialising, if serialization is successful then a value is stored within this flag key, to
    /// indicate that a value is present.
    /// </para>
    /// </remarks>
    /// <param name='key'>
    /// The key within the collection that serves as the flag.
    /// </param>
    /// <param name='value'>
    /// The flag value that is expected when deserializing, or written to the stream when serializing.
    /// </param>
    void UsingFlag(string key, string value);
  }
}

