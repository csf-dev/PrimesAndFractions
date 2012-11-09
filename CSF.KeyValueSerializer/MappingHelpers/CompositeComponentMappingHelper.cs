//
//  CompositeComponentMappingHelper.cs
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
using CSF.KeyValueSerializer.MappingModel;

namespace CSF.KeyValueSerializer.MappingHelpers
{
  /// <summary>
  /// Mapping helper for a single component of a composite mapping.
  /// </summary>
  public class CompositeComponentMappingHelper<TObject,TValue> : ICompositeComponentMappingHelper<TObject,TValue>
    where TObject : class
  {
    #region fields

    private ICompositeComponentMapping<TValue> _mapping;

    #endregion

    #region properties

    /// <summary>
    /// Gets the mapping associated with the current instance.
    /// </summary>
    /// <value>
    /// The mapping.
    /// </value>
    public ICompositeComponentMapping<TValue> Mapping
    {
      get {
        return _mapping;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }

        _mapping = value;
      }
    }

    #endregion

    #region mapping methods

    /// <summary>
    /// Facilitates the provision of a serialization function.
    /// </summary>
    /// <param name='serializationFunction'>
    /// A method body containing the serialization function.
    /// </param>
    public void Serialize(Func<TValue,string> serializationFunction)
    {
      this.Mapping.SerializationFunction = serializationFunction;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the composite component mapping helper type.
    /// </summary>
    /// <param name='mapping'>
    /// Mapping.
    /// </param>
    public CompositeComponentMappingHelper(ICompositeComponentMapping<TValue> mapping)
    {
      this.Mapping = mapping;
    }

    #endregion
  }
}

