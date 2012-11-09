//
//  IKeyValueSerializer.cs
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

namespace CSF.KeyValueSerializer
{
  /// <summary>
  /// Interface for a key/value serializer
  /// </summary>
  public interface IKeyValueSerializer<TOutput>
  {
    /// <summary>
    /// Deserialize the specified data, returning an object instance.
    /// </summary>
    /// <param name='data'>
    /// The collection of string data to deserialize.
    /// </param>
    TOutput Deserialize(IDictionary<string,string> data);

    /// <summary>
    /// Serialize the specified data, returning a dictionary/collection of string data.
    /// </summary>
    /// <param name='data'>
    /// The object instance to serialize.
    /// </param>
    IDictionary<string,string> Serialize(TOutput data);
  }
}

