//  
//  PropertyKeyAssociation.cs
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
  /// An association between a <see cref="PropertyInfo"/> and a <see cref="System.String"/> key that indicates that
  /// property's identifier when making use of a <c>IKeyValueSerializer</c> to serialize/deserialize instances
  /// of an object.
  /// </summary>
  public class SimplePropertyKeyMapping<TObject> : IPropertyKeyMapping<TObject>
  {
    #region fields
    
    private PropertyInfo _property;
    private string _key;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets or sets the <see cref="System.String"/> key that indicates the property's name.
    /// </summary>
    /// <value>
    /// The key.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public virtual string Key
    {
      get {
        return _key;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _key = value;
      }
    }
  
    /// <summary>
    /// Gets or sets a value indicating whether this association is mandatory.
    /// </summary>
    /// <value>
    /// <c>true</c> if mandatory; otherwise, <c>false</c>.
    /// </value>
    public virtual bool Mandatory
    {
      get;
      set;
    }
  
    /// <summary>
    /// Gets or sets a reference to the property that this association is for.
    /// </summary>
    /// <value>
    /// The property.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public virtual PropertyInfo Property
    {
      get {
        return _property;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        if(!value.CanRead || !value.CanWrite)
        {
          throw new ArgumentException(String.Format("Property '{0}' must be readable and writable.", value.Name));
        }
        
        _property = value;
      }
    }
    
    #endregion

    #region methods

    /// <summary>
    /// Deserializes a single value for the property associated with the current instance, storing it into the target
    /// object.
    /// </summary>
    /// <param name='target'>
    /// The target object, upon which to store the deserialized value.
    /// </param>
    /// <param name='value'>
    /// The value to deserialize.
    /// </param>
    public virtual void DeserializeValue(TObject target, string value)
    {
      this.Property.SetValue(target, Convert.ChangeType(value, this.Property.PropertyType), null);
    }

    /// <summary>
    /// Serializes and returns a single value for the property associated with the current instance.
    /// </summary>
    /// <returns>
    /// The string representation of the value.
    /// </returns>
    /// <param name='value'>
    /// The object to serialize.
    /// </param>
    public virtual string SerializeValue(TObject value)
    {
      object propertyValue = this.Property.GetValue(value, null);

      if(propertyValue == null && this.Mandatory)
      {
        string message = String.Format("Received a null value from property '{0}', but this property is marked as " +
                                       "mandatory.",
                                       this.Property.Name);
        throw new InvalidOperationException(message);
      }

      return (propertyValue != null)? propertyValue.ToString() : null;
    }

    #endregion
    
    #region constructor

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    protected SimplePropertyKeyMapping()
    {
      this.Mandatory = false;
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    /// <param name='property'>
    /// The property that this mapping is for.
    /// </param>
    public SimplePropertyKeyMapping(PropertyInfo property) : this()
    {
      this.Property = property;
      this.Key = this.Property.Name;
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    /// <param name='property'>
    /// An expression, indicating the property that this mapping is for.
    /// </param>
    public SimplePropertyKeyMapping(Expression<Func<TObject, object>> property) : this()
    {
      this.Property = StaticReflectionUtility.GetProperty<TObject>(property);
      this.Key = this.Property.Name;
    }

    #endregion
  }
}

