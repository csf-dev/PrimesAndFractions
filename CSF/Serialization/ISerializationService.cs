//
//  ISerializedConfigurationService.cs
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

namespace CSF.Serialization
{
  /// <summary>
  /// Interface for a service that is capable of serializing and persisting objects in a non-relational manner.
  /// </summary>
  public interface ISerializationService
  {
    #region serialization and deserialization

    /// <summary>
    /// Serialize and persist the specified data.
    /// </summary>
    /// <param name='data'>
    /// The 'payload' data to serialize.
    /// </param>
    /// <typeparam name='TData'>
    /// The type of the data.
    /// </typeparam>
    void Serialize<TData>(TData data);

    /// <summary>
    /// Serialize and persist the specified data.
    /// </summary>
    /// <param name='dataType'>
    /// The type of the data.
    /// </param>
    /// <param name='data'>
    /// The 'payload' data to serialize.
    /// </param>
    void Serialize(Type dataType, object data);

    /// <summary>
    /// Deserialize and return data of the specified type.
    /// </summary>
    /// <typeparam name='TData'>
    /// The type of the data.
    /// </typeparam>
    TData Deserialize<TData>();

    /// <summary>
    /// Deserialize and return data of the specified type.
    /// </summary>
    /// <param name='dataType'>
    /// The type of the data.
    /// </param>
    object Deserialize(Type dataType);

    #endregion

    #region persistance of raw data

    /// <summary>
    /// Persist the specified raw serialized data.
    /// </summary>
    /// <param name='data'>
    /// The raw/serialized data to persist.
    /// </param>
    void Persist(ISerializationContainer data);

    /// <summary>
    /// Retrieves raw/serialized data from the persistence backend.
    /// </summary>
    /// <typeparam name='TData'>
    /// The serialization-type of the data.
    /// </typeparam>
    ISerializationContainer Retrieve<TData>();

    /// <summary>
    /// Retrieves raw/serialized data from the persistence backend.
    /// </summary>
    /// <param name='serializationType'>
    /// The serialization-type of the data.
    /// </param>
    ISerializationContainer Retrieve(Type serializationType);

    #endregion
  }
}

