//
//  MappingBase.cs
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

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// Base class for types that implement <see cref="IMapping"/>.
  /// </summary>
  public abstract class MappingBase : IMapping
  {
    #region fields

    private IKeyNamingPolicy _namingRule;
    private bool _permitNullParent, _permitNullProperty;

    #endregion

    #region protected properties

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Collections.Serialization.MappingModel.MappingBase"/>
    /// permits null parent mappings.
    /// </summary>
    /// <value>
    /// <c>true</c> if null parent mappings are permitted; otherwise, <c>false</c>.
    /// </value>
    protected virtual bool PermitNullParent
    {
      get {
        return _permitNullParent;
      }
      set {
        _permitNullParent = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Collections.Serialization.MappingModel.MappingBase"/>
    /// permits null properties.
    /// </summary>
    /// <value>
    /// <c>true</c> if null properties are permitted; otherwise, <c>false</c>.
    /// </value>
    protected virtual bool PermitNullProperty
    {
      get {
        return _permitNullProperty;
      }
      set {
        _permitNullProperty = value;
      }
    }

    #endregion

    #region IMapping implementation

    /// <summary>
    /// Gets the 'parent' mapping that 'contains' the current mapping.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property should be non-null for everything but the root of the serialization hierarchy.  For every other
    /// mapping this property contains the mapping that is the immediate parent in the hierarchy.
    /// </para>
    /// <list type="bullets">
    /// <item>
    /// For a mapping of a property that exists on a class-like mapping, the parent is the mapping of that
    /// class-like item.
    /// </item>
    /// <item>
    /// For the mapping of composite components, the parent is the composite property mapping.
    /// </item>
    /// <item>
    /// For the mapping of collection items (either reference or value type), the parent is the collection mapping.
    /// </item>
    /// </list>
    /// </remarks>
    /// <value>
    /// The parent mapping.
    /// </value>
    public virtual IMapping ParentMapping
    {
      get;
      private set;
    }

    /// <summary>
    ///  Gets the property that this mapping relates to. 
    /// </summary>
    /// <value>
    ///  The property. 
    /// </value>
    public virtual PropertyInfo Property
    {
      get;
      private set;
    }

    /// <summary>
    ///  Gets or sets the naming rule for this property, which is possibly only a default naming rule. 
    /// </summary>
    /// <value>
    ///  The naming rule. 
    /// </value>
    public virtual IKeyNamingPolicy KeyNamingPolicy
    {
      get {
        return _namingRule;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        else if(!Object.ReferenceEquals(this, value.AssociatedMapping))
        {
          throw new ArgumentException("The given key naming policy is not associated with this mapping.");
        }

        _namingRule = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="CSF.Collections.Serialization.MappingModel.IMapping"/>
    /// is mandatory.
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
    ///  Gets a value that indicates a flag-key if it is in-use for this mapping. 
    /// </summary>
    /// <value>
    /// The flag key.
    /// </value>
    public virtual string FlagKey
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the flag value.
    /// </summary>
    /// <value>
    /// The flag value.
    /// </value>
    public virtual string FlagValue
    {
      get;
      set;
    }

    /// <summary>
    ///  Validates this mapping instance. 
    /// </summary>
    public virtual void Validate ()
    {
      if(!String.IsNullOrEmpty(this.FlagValue) && String.IsNullOrEmpty(this.FlagKey))
      {
        throw new InvalidOperationException("A required flag value is specified but not a flag key.  This is invalid.");
      }
      else if(!this.PermitNullParent && this.ParentMapping == null)
      {
        throw new InvalidOperationException("Parent mapping is null but this scenario is not permitted.");
      }
      else if(!this.PermitNullProperty && this.Property == null)
      {
        throw new InvalidOperationException("Associated property is null but this scenario is not permitted.");
      }
      else if(this.ParentMapping != null && this.Property == null)
      {
        throw new InvalidOperationException("The current mapping has a parent (IE: it is not the root of the " +
                                            "hierarchy) but it does not have an associated property.  This is " +
                                            "invalid.");
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Collections.Serialization.MappingModel.MappingBase"/> class.
    /// </summary>
    public MappingBase(IMapping parentMapping, PropertyInfo property)
    {
      this.PermitNullParent = false;
      this.PermitNullProperty = false;

      this.ParentMapping = parentMapping;
      this.Property = property;

      this.KeyNamingPolicy = new KeyNamingPolicy(this);
      this.Mandatory = false;
      this.FlagKey = null;
      this.FlagValue = null;
    }

    #endregion
  }
}

