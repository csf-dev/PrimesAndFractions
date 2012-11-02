//
//  KeyValueSerializer.cs
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
using CSF.Collections.Serialization.MappingModel;

namespace CSF.Collections.Serialization
{
  /// <summary>
  /// Default implementation of a key/value serializer.
  /// </summary>
  public class KeyValueSerializer<TObject> : IKeyValueSerializer<TObject>
      where TObject : class
  {
    #region fields

    private IMapping _rootMapping;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the root mapping for this serializer instance.
    /// </summary>
    /// <value>
    /// The root mapping.
    /// </value>
    public virtual IMapping RootMapping
    {
      get {
        return _rootMapping;
      }
      protected set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        else if(_rootMapping != null)
        {
          throw new InvalidOperationException("This serializer instance already contains a root mapping, refusing " +
                                              "to overwrite it.");
        }

        _rootMapping = value;
      }
    }

    #endregion

    #region IKeyValueSerializer implementation

    /// <summary>
    ///  Adds one or more of mappings to the serializer instance using a mapping method. 
    /// </summary>
    /// <param name='mappings'>
    ///  An action (possibly a pointer to a delegate or an anonymous method) that expresses the mappings to serialize
    /// and/or deserialize objects 
    /// </param>
    public void Map (Action<IClassMappingHelper<TObject>> mappings)
    {
      if(mappings == null)
      {
        throw new ArgumentNullException("mappings");
      }

      var mapping = new ClassMapping<TObject>();
      this.RootMapping = mapping;
      var helper = new ClassMappingHelper<TObject>(mapping);

      mappings(helper);
    }

    /// <summary>
    /// Adds mappings to this instance for the scenario in which <c>TObject</c> is a collection of
    /// class-like/reference-type objects.
    /// </summary>
    /// <param name='mappings'>
    /// An action (possibly a pointer to a delegate or an anonymous method) that expresses the mappings to serialize
    /// and/or deserialize objects
    /// </param>
    /// <typeparam name='TCollectionItem'>
    /// The type of objects contained within the collection.
    /// </typeparam>
    public void Collection<TCollectionItem> (Action<IReferenceTypeCollectionMappingHelper<TObject, TCollectionItem>> mappings)
      where TCollectionItem : class
    {
      if(mappings == null)
      {
        throw new ArgumentNullException("mappings");
      }

      var mapping = new ReferenceTypeCollectionMapping<TCollectionItem>();
      this.RootMapping = mapping;
      var helper = new ReferenceTypeCollectionMappingHelper<TObject,TCollectionItem>(mapping);

      mappings(helper);
    }

    /// <summary>
    /// Adds mappings to this instance for the scenario in which <c>TObject</c> is a collection of
    /// struct-like/value-type objects.
    /// </summary>
    /// <param name='mappings'>
    /// An action (possibly a pointer to a delegate or an anonymous method) that expresses the mappings to serialize
    /// and/or deserialize objects
    /// </param>
    /// <typeparam name='TCollectionItem'>
    /// The type of objects contained within the collection.
    /// </typeparam>
    public void ValueTypeCollection<TCollectionItem> (Action<IValueTypeCollectionMappingHelper<TObject, TCollectionItem>> mappings)
      where TCollectionItem : struct
    {
      if(mappings == null)
      {
        throw new ArgumentNullException("mappings");
      }

      var mapping = new ValueTypeCollectionMapping<TCollectionItem>();
      this.RootMapping = mapping;
      var helper = new ValueTypeCollectionMappingHelper<TObject,TCollectionItem>(mapping);

      mappings(helper);
    }

    /// <summary>
    ///  Deserialize the specified data, returning an object instance. 
    /// </summary>
    /// <param name='data'>
    ///  The collection of string data to deserialize. 
    /// </param>
    public TObject Deserialize (IDictionary<string, string> data)
    {
      object result;
      this.RootMapping.Deserialize(data, out result, new int[0]);
      return (TObject) result;
    }

    /// <summary>
    ///  Serialize the specified data, returning a dictionary/collection of string data. 
    /// </summary>
    /// <param name='data'>
    ///  The object instance to serialize. 
    /// </param>
    public IDictionary<string, string> Serialize (TObject data)
    {
      throw new System.NotImplementedException ();
    }

    #endregion
  }
}

