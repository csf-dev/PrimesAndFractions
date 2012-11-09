//
//  ISerializedConfigurationData.cs
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

namespace CSF.Patterns.SerializedLob
{
  /// <summary>
  /// Interface represents a container for data that can be serialized into string form.
  /// </summary>
  public interface ISerializationContainer
  {
    #region properties

    /// <summary>
    /// Gets or sets the serialized data.
    /// </summary>
    /// <value>
    /// The data.
    /// </value>
    string Data { get; set; }

    /// <summary>
    /// Gets or sets the type that this data serializes/deserializes to/from.
    /// </summary>
    /// <value>
    /// The type associated with the data.
    /// </value>
    Type Type { get; set; }

    #endregion

    #region methods

    /// <summary>
    /// Deserialize this instance into an object.
    /// </summary>
    object Deserialize();

    /// <summary>
    /// Serializes the configuration into the <see cref="Data"/> property.
    /// </summary>
    /// <param name='data'>
    /// The data to serialize.
    /// </param>
    void Serialize(object data);

    #endregion
  }
}

