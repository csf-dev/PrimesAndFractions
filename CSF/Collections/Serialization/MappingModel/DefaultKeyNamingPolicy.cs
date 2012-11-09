//
//  NullKeyNamingRule.cs
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
  /// A default/no-op key-naming policy that provides no options and always formats key names using a hard-coded
  /// default policy.
  /// </summary>
  public class DefaultKeyNamingPolicy : IKeyNamingPolicy
  {
    #region fields

    private IMapping _associatedMapping;

    #endregion

    #region properties

    /// <summary>
    /// Gets the associated mapping.
    /// </summary>
    /// <value>
    /// The associated mapping.
    /// </value>
    public virtual IMapping AssociatedMapping
    {
      get {
        return _associatedMapping;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _associatedMapping = value;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Resolves and returns the name of the key for the <see cref="AssociatedMapping"/>.
    /// </summary>
    /// <returns>
    /// The key name.
    /// </returns>
    /// <param name='collectionIndices'>
    /// Zero or more indices for collection-type mappings that are passed-through when traversing from the root of the
    /// serialization hierarchy to the associated mapping.
    /// </param>
    public virtual string GetKeyName(params int[] collectionIndices)
    {
      int currentCollectionNumber = 0;
      return this.GetKeyName(collectionIndices?? new int[0], ref currentCollectionNumber);
    }

    /// <summary>
    /// Resolves and returns the name of the key for the <see cref="AssociatedMapping"/>.
    /// </summary>
    /// <returns>
    /// The key name.
    /// </returns>
    /// <param name='collectionIndices'>
    /// Zero or more indices for collection-type mappings that are passed-through when traversing from the root of the
    /// serialization hierarchy to the associated mapping.
    /// </param>
    /// <param name='currentCollectionNumber'>
    /// The current collection number that is being dealt with.  Essentially the current index within
    /// <paramref name="collectionIndices"/>
    /// </param>
    public virtual string GetKeyName(int[] collectionIndices, ref int currentCollectionNumber)
    {
      string output = String.Empty;

      if(this.AssociatedMapping.ParentMapping != null)
      {
        string parentKey = this.AssociatedMapping.ParentMapping.KeyNamingPolicy.GetKeyName(collectionIndices,
                                                                                           ref currentCollectionNumber);
        if(!String.IsNullOrEmpty(parentKey))
        {
          output = parentKey;
        }
      }

      if(!String.IsNullOrEmpty(output) && this.AssociatedMapping.Property != null)
      {
        output = String.Concat(output, this.GetPropertySeparator());
      }

      if((this.AssociatedMapping is ICollectionMapping)
         && ((ICollectionMapping) this.AssociatedMapping).CollectionKeyType == CollectionKeyType.Separate)

      {
        if(this.AssociatedMapping.Property != null)
        {
          output = String.Concat(output,
                                 this.FormatProperty(this.AssociatedMapping.Property,
                                                     collectionIndices[currentCollectionNumber++]));
        }
        else
        {
          output = String.Concat(output, this.FormatProperty(collectionIndices[currentCollectionNumber++]));
        }
      }
      else if(this.AssociatedMapping.Property != null)
      {
        output = String.Concat(output, this.FormatProperty(this.AssociatedMapping.Property));
      }

      return output;
    }

    /// <summary>
    /// Formats a key name fragment using a singular property name.
    /// </summary>
    /// <returns>
    /// A key name fragment.
    /// </returns>
    /// <param name='property'>
    /// The property from which to create the fragment
    /// </param>
    protected virtual string FormatProperty(PropertyInfo property)
    {
      if(property == null)
      {
        throw new ArgumentNullException("property");
      }

      return property.Name;
    }

    /// <summary>
    /// Formats a key name fragment using a property name and a collection index.
    /// </summary>
    /// <returns>
    /// A key name fragment.
    /// </returns>
    /// <param name='property'>
    /// The property from which to create the fragment
    /// </param>
    /// <param name='index'>
    /// The numeric collection index
    /// </param>
    protected virtual string FormatProperty(PropertyInfo property, int index)
    {
      return String.Concat(this.FormatProperty(property), this.FormatProperty(index));
    }

    /// <summary>
    /// Formats a key name fragment using just a collection index.
    /// </summary>
    /// <returns>
    /// A key name fragment.
    /// </returns>
    /// <param name='index'>
    /// The numeric collection index
    /// </param>
    protected virtual string FormatProperty(int index)
    {
      return String.Format("[{0}]", index);
    }

    /// <summary>
    /// Gets the separator string for separating/delimiting properties.
    /// </summary>
    /// <returns>
    /// The property separator.
    /// </returns>
    protected virtual string GetPropertySeparator()
    {
      return ".";
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Collections.Serialization.MappingModel.DefaultKeyNamingPolicy"/>
    /// class.
    /// </summary>
    /// <param name='associatedMapping'>
    /// Associated mapping.
    /// </param>
    public DefaultKeyNamingPolicy(IMapping associatedMapping)
    {
      this.AssociatedMapping = associatedMapping;
    }

    #endregion
  }
}

