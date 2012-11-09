//
//  CompositePropertyComponentMapping.cs
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

namespace CSF.KeyValueSerializer.MappingModel
{
  /// <summary>
  /// Implementation of a mapping for a composite component.
  /// </summary>
  public class CompositeComponentMapping<TValue> : ICompositeComponentMapping<TValue>
  {
    #region fields

    private ICompositeMapping<TValue> _parentMapping;
    private object _componentIdentifier;

    #endregion

    #region properties

    /// <summary>
    ///  Gets the 'parent' composite mapping. 
    /// </summary>
    /// <value>
    ///  The parent mapping. 
    /// </value>
    public ICompositeMapping<TValue> ParentMapping
    {
      get {
        return _parentMapping;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _parentMapping = value;
      }
    }

    /// <summary>
    ///  Gets the component identifier for this mapping. 
    /// </summary>
    /// <value>
    ///  The component identifier. 
    /// </value>
    public object ComponentIdentifier
    {
      get {
        return _componentIdentifier;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _componentIdentifier = value;
      }
    }

    /// <summary>
    /// Gets or sets the function used to serialize a single string value (corresponding to this component) from the
    /// property value. 
    /// </summary>
    /// <value>
    /// A method body containing the serialization function. 
    /// </value>
    public Func<TValue, string> SerializationFunction
    {
      get;
      set;
    }

    #endregion

    #region methpds

    /// <summary>
    /// Gets the collection key that corresponds to the data for this component. 
    /// </summary>
    /// <param name='collectionIndices'>
    /// Zero or more indices for collection-type mappings that are passed-through when traversing from the root of the
    /// serialization hierarchy to the associated mapping.
    /// </param>
    /// <returns>
    ///  The collection key. 
    /// </returns>
    public virtual string GetKeyName(params int[] collectionIndices)
    {
      return String.Concat(this.ParentMapping.KeyNamingPolicy.GetKeyName(collectionIndices),
                           this.ComponentIdentifier.ToString());
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the component mapping type.
    /// </summary>
    /// <param name='parentMapping'>
    /// Parent mapping.
    /// </param>
    /// <param name='componentIdentifier'>
    /// Component identifier.
    /// </param>
    public CompositeComponentMapping(ICompositeMapping<TValue> parentMapping, object componentIdentifier)
    {
      this.ParentMapping = parentMapping;
      this.ComponentIdentifier = componentIdentifier;
    }

    #endregion
  }
}

