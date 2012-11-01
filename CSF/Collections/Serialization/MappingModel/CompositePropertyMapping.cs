//
//  CompositePropertyMapping.cs
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
using System.Reflection;

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// Implementation of a composite property mapping.
  /// </summary>
  /// <remarks>
  /// <para>
  /// In a composite property mapping, the property value is created from values found within multiple keys.  Each of
  /// these keys used is called a 'component'.
  /// </para>
  /// </remarks>
  public class CompositePropertyMapping<TValue> : MappingBase, ICompositeMapping<TValue>
  {
    #region fields

    private IDictionary<object, ICompositeComponentMapping<TValue>> _components;

    #endregion

    #region ICompositeMapping implementation

    /// <summary>
    /// Gets or sets the function used to deserialize the property value from a dictionary of string values, indexed by
    /// the component identifiers from this mapping. 
    /// </summary>
    /// <value>
    /// A method body containing the deserialization function. 
    /// </value>
    public Func<IDictionary<object, string>, TValue> DeserializationFunction
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets a collection of the components that make up this mapping and their respective mappings.
    /// </summary>
    /// <value>
    ///  A collection of the components, indexed by their identifiers. 
    /// </value>
    public IDictionary<object, ICompositeComponentMapping<TValue>> Components
    {
      get {
        return _components;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _components = value;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the composite property mapping class.
    /// </summary>
    /// <param name='parentMapping'>
    /// Parent mapping.
    /// </param>
    /// <param name='property'>
    /// Property.
    /// </param>
    public CompositePropertyMapping(IMapping parentMapping, PropertyInfo property) : base(parentMapping, property)
    {
      this.DeserializationFunction = null;
      this.Components = new Dictionary<object, ICompositeComponentMapping<TValue>>();
    }

    /// <summary>
    /// Initializes a new instance of the composite property mapping class.
    /// </summary>
    /// <param name='property'>
    /// The property that this instance is associated with.
    /// </param>
    /// <param name='parentMapping'>
    /// The parent mapping.
    /// </param>
    /// <param name='classMode'>
    /// A boolean that indicates whether this instance will operate in 'class mode' or not.
    /// </param>
    public CompositePropertyMapping(IMapping parentMapping,
                                    PropertyInfo property,
                                    bool classMode) : this(parentMapping, property)
    {
      if(classMode)
      {
        this.PermitNullParent = true;
        this.PermitNullProperty = true;
      }
    }

    #endregion
  }
}

