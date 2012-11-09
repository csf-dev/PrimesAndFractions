//
//  IValueTypeCollectionKeyValueSerializer.cs
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
using CSF.Collections.Serialization.MappingHelpers;
using System.Collections.Generic;

namespace CSF.Collections.Serialization
{
  /// <summary>
  /// Interface for a key/value serializer that serializes/deserializes an instance of a collection of struct-like types.
  /// </summary>
  /// <typeparam name='TOutput'>
  /// The type of the 'root' object that this serializer instance will work with.
  /// </typeparam>
  public interface IValueTypeCollectionKeyValueSerializer<TOutput>
    : IKeyValueSerializer<ICollection<TOutput>>
    where TOutput : struct
  {
    /// <summary>
    /// Adds mappings to this instance for the scenario in which <c>TObject</c> is a collection of
    /// struct-like/value-type objects.
    /// </summary>
    /// <param name='mapping'>
    /// An action (possibly a pointer to a delegate or an anonymous method) that expresses the mappings to serialize
    /// and/or deserialize objects
    /// </param>
    void Map(Action<IValueTypeCollectionMappingHelper<ICollection<TOutput>,TOutput>> mapping);
  }
}

