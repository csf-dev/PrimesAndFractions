//
//  SerializedConfigurationData.cs
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using CSF.Reflection;

namespace CSF.Serialization
{
  /// <summary>
  /// An implementation of <see cref="ISerializationContainer"/> for XML-serialized data.
  /// </summary>
  public class XmlSerializationContainer : ISerializationContainer
  {
    #region properties

    /// <summary>
    /// Gets or sets the <see cref="Type"/> using its name.
    /// </summary>
    /// <value>
    /// The name of the type associated with the data.
    /// </value>
    public virtual string TypeName
    {
      get;
      set;
    }

    #endregion

    #region ISerializationContainer implementation

    /// <summary>
    /// Gets or sets the serialized data. 
    /// </summary>
    /// <value>
    /// The data. 
    /// </value>
    public virtual string Data
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the type that this data serializes/deserializes to/from. 
    /// </summary>
    /// <value>
    /// The type associated with the data. 
    /// </value>
    public virtual Type Type
    {
      get {
        return String.IsNullOrEmpty(this.TypeName)? null : Reflect.TypeFromAppDomain(this.TypeName);
      }
      set {
        this.TypeName = (value != null)? value.FullName : null;
      }
    }

    /// <summary>
    /// Deserialize this instance into an object. 
    /// </summary>
    public virtual object Deserialize()
    {
      object output;
      XmlSerializer serializer = this.GetSerializer();

      using(TextReader reader = new StringReader(this.Data))
      {
        output = serializer.Deserialize(reader);
      }

      return output;
    }

    /// <summary>
    /// Serializes the data into the <see cref="Data"/> property.
    /// </summary>
    /// <param name='data'>
    /// The data to serialize.
    /// </param>
    public virtual void Serialize (object data)
    {
      StringBuilder newData = new StringBuilder();
      XmlSerializer serializer = this.GetSerializer();

      using(TextWriter writer = new StringWriter(newData))
      {
        serializer.Serialize(writer, data);
      }

      this.Data = newData.ToString();
    }

    #endregion

    #region private methods

    /// <summary>
    /// Creates and returns a serializer instance.
    /// </summary>
    /// <returns>
    /// A serializer.
    /// </returns>
    private XmlSerializer GetSerializer()
    {
      if(this.Type == null)
      {
        throw new InvalidOperationException("Cannot create serializer, no type has been selected.");
      }

      return new XmlSerializer(this.Type);
    }

    #endregion
  }
}

