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
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Specialized;
using CSF.Collections.Serialization.MappingHelpers;

namespace CSF.Collections.Serialization
{
  /// <summary>
  /// Interface for a key/value serializer.
  /// </summary>
  /// <typeparam name='TObject'>
  /// The type of the 'root' object that this serializer instance will work with.
  /// </typeparam>
  public interface IKeyValueSerializer<TObject>
      where TObject : class
  {
    /// <summary>
    /// Adds one or more of mappings to the serializer instance using a mapping method.
    /// </summary>
    /// <param name='mappings'>
    /// An action (possibly a pointer to a delegate or an anonymous method) that expresses the mappings to serialize
    /// and/or deserialize objects
    /// </param>
    void Map(Action<IClassMappingHelper<TObject>> mappings);

    /// <summary>
    /// Adds mappings to this instance for the scenario in which <c>TObject</c> is a collection of
    /// class-like/reference-type objects.
    /// </summary>
    /// <param name='mapping'>
    /// An action (possibly a pointer to a delegate or an anonymous method) that expresses the mappings to serialize
    /// and/or deserialize objects
    /// </param>
    /// <typeparam name='TCollectionItem'>
    /// The type of objects contained within the collection.
    /// </typeparam>
    void Collection<TCollectionItem>(Action<IReferenceTypeCollectionMappingHelper<TObject,TCollectionItem>> mapping)
      where TCollectionItem : class;

    /// <summary>
    /// Adds mappings to this instance for the scenario in which <c>TObject</c> is a collection of
    /// struct-like/value-type objects.
    /// </summary>
    /// <param name='mapping'>
    /// An action (possibly a pointer to a delegate or an anonymous method) that expresses the mappings to serialize
    /// and/or deserialize objects
    /// </param>
    /// <typeparam name='TCollectionItem'>
    /// The type of objects contained within the collection.
    /// </typeparam>
    void ValueTypeCollection<TCollectionItem>(Action<IValueTypeCollectionMappingHelper<TObject,TCollectionItem>> mapping)
      where TCollectionItem : struct;

    /// <summary>
    /// Deserialize the specified data, returning an object instance.
    /// </summary>
    /// <param name='data'>
    /// The collection of string data to deserialize.
    /// </param>
    TObject Deserialize(IDictionary<string,string> data);

    /// <summary>
    /// Serialize the specified data, returning a dictionary/collection of string data.
    /// </summary>
    /// <param name='data'>
    /// The object instance to serialize.
    /// </param>
    IDictionary<string,string> Serialize(TObject data);
  }
}

