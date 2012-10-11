//
//  CustomPropertyKeyMapping.cs
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
using System.Linq.Expressions;
using CSF.Reflection;

namespace CSF.Collections
{
  /// <summary>
  /// Implementation of a key-property mapping that provides custom serialization and deserialization functions.
  /// </summary>
  public class CustomPropertyKeyMapping<TObject,TProperty> : SimplePropertyKeyMapping<TObject>
  {
    #region fields

    private Func<TObject,string,TProperty> _customDeserialization;
    private Func<TObject,TProperty,string> _customSerialization;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the custom deserialization function.
    /// </summary>
    /// <value>
    /// The custom deserialization function.
    /// </value>
    public virtual Func<TObject, string, TProperty> CustomDeserialization
    {
      get {
        return _customDeserialization;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _customDeserialization = value;
      }
    }

    /// <summary>
    /// Gets or sets the custom serialization function.
    /// </summary>
    /// <value>
    /// The custom serialization function.
    /// </value>
    public virtual Func<TObject, TProperty, string> CustomSerialization
    {
      get {
        return _customSerialization;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _customSerialization = value;
      }
    }

    #endregion

    #region methods

    /// <summary>
    ///  Deserializes a single value for the property associated with the current instance, storing it into the target
    /// object. 
    /// </summary>
    /// <param name='target'>
    ///  The target object, upon which to store the deserialized value. 
    /// </param>
    /// <param name='value'>
    ///  The value to deserialize. 
    /// </param>
    public override void DeserializeValue(TObject target, string value)
    {
      this.Property.SetValue(target, this.CustomDeserialization(target, value), null);
    }

    /// <summary>
    ///  Serializes and returns a single value for the property associated with the current instance. 
    /// </summary>
    /// <returns>
    ///  The string representation of the value. 
    /// </returns>
    /// <param name='value'>
    ///  The object to serialize. 
    /// </param>
    public override string SerializeValue(TObject value)
    {
      return this.CustomSerialization(value, (TProperty) this.Property.GetValue(value, null));
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of this type.
    /// </summary>
    /// <param name='property'>
    /// The property that we are mapping to.
    /// </param>
    public CustomPropertyKeyMapping(PropertyInfo property) : base(property) {}

    /// <summary>
    /// Initializes a new instance of this type.
    /// </summary>
    /// <param name='property'>
    /// An expression, indicating the property that this mapping is for.
    /// </param>
    public CustomPropertyKeyMapping(Expression<Func<TObject, TProperty>> property)
    {
      this.Property = StaticReflectionUtility.GetProperty<TObject,TProperty>(property);
      this.Key = this.Property.Name;
    }

    #endregion
  }
}

