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
  public class PropertyKeyAssociation<TObject>
  {
    #region fields
    
    PropertyInfo _property;
    string _key;
    bool _mandatory;
    
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
    public string Key
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
    public bool Mandatory
    {
      get {
        return _mandatory;
      }
      set {
        _mandatory = value;
      }
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
    public PropertyInfo Property
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
    /// Sets the value of <see cref="Property"/> by using a delegate that points to the <see cref="PropertyInfo"/>.
    /// </summary>
    /// <param name='expression'>
    /// Property.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type of the object that contains this property.
    /// </typeparam>
    public void SetProperty(Expression<Func<TObject, object>> expression)
    {
      this.Property = ReflectionHelper.GetProperty<TObject>(expression);
    }
    
    #endregion
  }
}

