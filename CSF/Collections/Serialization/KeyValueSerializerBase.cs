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
  public abstract class KeyValueSerializerBase<TOutput> : IKeyValueSerializer<TOutput>
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
    ///  Deserialize the specified data, returning an object instance. 
    /// </summary>
    /// <param name='data'>
    ///  The collection of string data to deserialize. 
    /// </param>
    public TOutput Deserialize (IDictionary<string, string> data)
    {
      if(this.RootMapping == null)
      {
        throw new InvalidOperationException("This serializer instance does not yet have a root mapping.");
      }

      object result;
      TOutput output = default(TOutput);

      if(this.RootMapping.Deserialize(data, out result, new int[0]))
      {
        output = (TOutput) result;
      }

      return output;
    }

    /// <summary>
    ///  Serialize the specified data, returning a dictionary/collection of string data. 
    /// </summary>
    /// <param name='data'>
    ///  The object instance to serialize. 
    /// </param>
    public IDictionary<string, string> Serialize(TOutput data)
    {
      if(this.RootMapping == null)
      {
        throw new InvalidOperationException("This serializer instance does not yet have a root mapping.");
      }

      IDictionary<string,string> output = new Dictionary<string, string>();
      this.RootMapping.Serialize(data, ref output, new int[0]);
      return output;
    }

    #endregion
  }
}

