//
//  MandatorySerializationException.cs
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

namespace CSF.Collections.Serialization.MappingModel
{
  /// <summary>
  /// Exception represents a failure to serialize or deserialize a mandatory mapping.
  /// </summary>
  public class MandatorySerializationException : Exception
  {
    #region constants

    private const string DEFAULT_MESSAGE = "A mandatory mapping was not fulfilled whilst performing serialization " +
                                           "or deserialization.";

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the mapping.
    /// </summary>
    /// <value>
    /// The mapping.
    /// </value>
    public virtual IMapping Mapping
    {
      get {
        return this.Data["Mapping"] as IMapping;
      }
      private set {
        this.Data["Mapping"] = value;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Collections.Serialization.MappingModel.MandatorySerializationException"/> class.
    /// </summary>
    /// <param name='mapping'>
    /// The associated mapping.
    /// </param>
    public MandatorySerializationException(IMapping mapping) : base(DEFAULT_MESSAGE)
    {
      this.Mapping = mapping;
    }

    #endregion
  }
}

