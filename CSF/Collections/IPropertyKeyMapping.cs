//
//  IPropertyKeyMapping.cs
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

namespace CSF.Collections
{
  /// <summary>
  /// Interface for a mapping between a string 'key' and a property of <c>TObject</c>.
  /// </summary>
  public interface IPropertyKeyMapping<TObject>
  {
    #region properties

    /// <summary>
    /// Gets the string key at which this object exists.
    /// </summary>
    /// <value>
    /// The key.
    /// </value>
    string Key { get; }

    /// <summary>
    /// Gets the mapped property.
    /// </summary>
    /// <value>
    /// The property.
    /// </value>
    PropertyInfo Property { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is mandatory.
    /// </summary>
    /// <remarks>
    /// Mandatory mappings mean that that a deserialization action will be considered a failure if there are values
    /// missing for mandatory mappings.
    /// </remarks>
    /// <value>
    /// <c>true</c> if mandatory; otherwise, <c>false</c>.
    /// </value>
    bool Mandatory { get; }

    #endregion

    #region methods

    /// <summary>
    /// Deserializes a single value for the property associated with the current instance, storing it into the target
    /// object.
    /// </summary>
    /// <param name='target'>
    /// The target object, upon which to store the deserialized value.
    /// </param>
    /// <param name='value'>
    /// The value to deserialize.
    /// </param>
    void DeserializeValue(TObject target, string value);

    /// <summary>
    /// Serializes and returns a single value for the property associated with the current instance.
    /// </summary>
    /// <returns>
    /// The string representation of the value.
    /// </returns>
    /// <param name='value'>
    /// The object to serialize.
    /// </param>
    string SerializeValue(TObject value);

    #endregion
  }
}

