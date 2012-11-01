//
//  MappingHelper.cs
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
using CSF.Collections.Serialization.MappingModel;
using System.Reflection;

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Base mapping helper type.
  /// </summary>
  public abstract class MappingHelper<TMapping> : IMappingHelper
    where TMapping : IMapping
  {
    #region fields

    private TMapping _mapping;

    #endregion

    #region properties

    /// <summary>
    /// Gets the mapping associated with the current instance.
    /// </summary>
    /// <value>
    /// The mapping.
    /// </value>
    public TMapping Mapping
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

    #region methods

    /// <summary>
    ///  Indicates that this item (type or property) is mandatory. 
    /// </summary>
    public void Mandatory()
    {
      this.Mapping.Mandatory = true;
    }

    /// <summary>
    ///  Indicates that when serializing/deserializing this instance, a 'flag' value should be used. 
    /// </summary>
    /// <param name='key'>
    /// Key.
    /// </param>
    public void UsingFlag(string key)
    {
      this.UsingFlag(key, null);
    }

    /// <summary>
    ///  Indicates that when serializing/deserializing this instance, a 'flag' value should be used. 
    /// </summary>
    /// <param name='key'>
    /// Key.
    /// </param>
    /// <param name='value'>
    /// Value.
    /// </param>
    public void UsingFlag(string key, string value)
    {
      this.Mapping.FlagKey = key;
      this.Mapping.FlagValue = value;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the mapping helper type.
    /// </summary>
    /// <param name='mapping'>
    /// Mapping.
    /// </param>
    public MappingHelper(TMapping mapping)
    {
      this.Mapping = mapping;
    }

    #endregion

  }
}

