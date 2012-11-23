using System;
using System.Collections.Generic;
using CSF.KeyValueSerializer.MappingModel;

namespace CSF.KeyValueSerializer
{
  /// <summary>
  /// Inteface for a non-generic key-value serializer.
  /// </summary>
  public interface IKeyValueSerializer
  {
    /// <summary>
    /// Deserialize the specified data, returning an object instance.
    /// </summary>
    /// <param name='data'>
    /// The collection of string data to deserialize.
    /// </param>
    object Deserialize(IDictionary<string,string> data);

    /// <summary>
    /// Serialize the specified data, returning a dictionary/collection of string data.
    /// </summary>
    /// <param name='data'>
    /// The object instance to serialize.
    /// </param>
    IDictionary<string,string> Serialize(object data);

    /// <summary>
    /// Gets the root mapping from this serializer instance.
    /// </summary>
    /// <value>
    /// The root mapping.
    /// </value>
    IMapping RootMapping { get; }
  }
}

