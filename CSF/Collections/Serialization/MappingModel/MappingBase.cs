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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// Base class for types that implement <see cref="IMapping"/>.
  /// </summary>
  public abstract class MappingBase : IMapping
  {
    #region fields

    private IKeyNamingPolicy _namingRule;

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
    /// Gets or sets a value indicating whether this instance is the root mapping.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is the root mapping; otherwise, <c>false</c>.
    /// </value>
    public virtual bool IsRootMapping
    {
      get;
      private set;
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
        throw new InvalidMappingException("A required flag value is specified but not a flag key.  This is invalid.");
      }
      else if(!this.IsRootMapping && this.ParentMapping == null)
      {
        throw new InvalidMappingException("This mapping is not marked as the root mapping but it does not have a " +
                                          "parent mapping; this is not permitted.");
      }
    }

    /// <summary>
    ///  Deserialize the specified data as an object instance. 
    /// </summary>
    /// <returns>
    ///  A value that indicates whether deserialization was successful or not. 
    /// </returns>
    /// <param name='data'>
    ///  The dictionary/collection of string data to deserialize from. 
    /// </param>
    /// <param name='result'>
    ///  The output/deserialized object instance. If the return value is false (unsuccessful deserialization) then the
    /// output value of this parameter is undefined. 
    /// </param>
    /// <param name='collectionIndices'>
    ///  A collection of integers, indicating the indices of any collection mappings passed-through during the 
    /// </param>
    public abstract bool Deserialize(IDictionary<string,string> data, out object result, int[] collectionIndices);

    /// <summary>
    /// Serialize the specified data, exposing the result as an output parameter.
    /// </summary>
    /// <param name='data'>
    /// The object (or object graph) to serialize.
    /// </param>
    /// <param name='result'>
    /// The dictionary of string values to contain the serialized data.
    /// </param>
    /// <param name='collectionIndices'>
    /// A collection of integers, indicating the indices of any collection mappings passed-through during the
    /// </param>
    /// <typeparam name='TInput'>
    /// The type of data to serialize.
    /// </typeparam>
    public abstract void Serialize(object data, ref IDictionary<string,string> result, int[] collectionIndices);

    /// <summary>
    /// Gets the mapping for the current item (the map-as).
    /// </summary>
    /// <returns>
    /// The mapping.
    /// </returns>
    public virtual IMapping GetMapping()
    {
      throw new NotSupportedException("This current mapping type does not support a parameterless GetMapping method.");
    }

    /// <summary>
    /// Gets the mapping for the given property.
    /// </summary>
    /// <returns>
    /// The mapping.
    /// </returns>
    /// <param name='property'>
    /// An expression that indicates the property to retrieve the mapping for.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type associated with the current mapping (the type that 'hosts' the property).
    /// </typeparam>
    public virtual IMapping GetMapping<TObject>(Expression<Func<TObject, object>> property)
    {
      throw new NotSupportedException("This current mapping type does not support the property-based GetMapping method.");
    }

    /// <summary>
    /// Gets the name of the 'key' that is used for the current mapping.
    /// </summary>
    /// <returns>
    /// The key name.
    /// </returns>
    /// <param name='collectionIndices'>
    /// A collection of integer 'collection indices' for any collection-type mappings that have been passed-through.
    /// </param>
    public virtual string GetKeyName(params int[] collectionIndices)
    {
      throw new NotSupportedException("This current mapping type does not support the getting of a key-name (did you mean to use a component identifier?).");
    }

    /// <summary>
    /// Gets the name of the 'key' that is used for the current mapping.
    /// </summary>
    /// <returns>
    /// The key name.
    /// </returns>
    /// <param name='componentIdentifier'>
    /// The identifier for a component of a composite mapping.
    /// </param>
    /// <param name='collectionIndices'>
    /// A collection of integer 'collection indices' for any collection-type mappings that have been passed-through.
    /// </param>
    public virtual string GetKeyName(object componentIdentifier, params int[] collectionIndices)
    {
      throw new NotSupportedException("This current mapping type does not support the getting of a key-name with a component identifier.");
    }

    #endregion

    #region protected methods

    /// <summary>
    /// Determines whether or not the current instance may be deserialized.
    /// </summary>
    /// <param name='data'>
    ///  The dictionary/collection of string data to deserialize from. 
    /// </param>
    /// <returns>
    /// The deserialize.
    /// </returns>
    protected bool MayDeserialize(IDictionary<string,string> data)
    {
      bool output = true;

      if(data == null)
      {
        throw new ArgumentNullException("data");
      }

      this.Validate();

      if(!String.IsNullOrEmpty(this.FlagKey))
      {
        if(!data.ContainsKey(this.FlagKey))
        {
          output = false;
        }
        else if(String.IsNullOrEmpty(this.FlagValue))
        {
          output = !String.IsNullOrEmpty(data[this.FlagKey]);
        }
        else
        {
          output = data[this.FlagKey] == this.FlagValue;
        }
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Collections.Serialization.MappingModel.MappingBase"/> class.
    /// </summary>
    /// <param name='parentMapping'>
    /// A reference to the parent mapping instance.
    /// </param>
    /// <param name='property'>
    /// An optional reference to the property that is passed through in this mapping.
    /// </param>
    protected MappingBase (IMapping parentMapping, PropertyInfo property) : this(parentMapping, property, false) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Collections.Serialization.MappingModel.MappingBase"/> class.
    /// </summary>
    /// <param name='parentMapping'>
    /// A reference to the parent mapping instance.
    /// </param>
    /// <param name='property'>
    /// An optional reference to the property that is passed through in this mapping.
    /// </param>
    /// <param name='isRootMapping'>
    /// A value that indicates whether the current instance represents the root of the mapping hierarchy.
    /// </param>
    protected MappingBase(IMapping parentMapping, PropertyInfo property, bool isRootMapping)
    {
      if(isRootMapping
         && (parentMapping != null || property != null))
      {
        throw new ArgumentException("If root mapping is true then both the parent mapping and property should be null.",
                                    "isRootMapping");
      }
      else if(!isRootMapping && parentMapping == null)
      {
        throw new ArgumentException("If root mapping is false then the parent mapping must not be null.",
                                    "isRootMapping");
      }

      this.IsRootMapping = isRootMapping;
      this.ParentMapping = parentMapping;
      this.Property = property;

      this.KeyNamingPolicy = new DefaultKeyNamingPolicy(this);
      this.Mandatory = false;
      this.FlagKey = null;
      this.FlagValue = null;
    }

    #endregion
  }
}

