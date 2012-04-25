//  
//  IParameterSpecification.cs
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

namespace CSF.Cli
{
  /// <summary>
  /// Interface for a parameter specification (and later a parsed parameter).
  /// </summary>
  public interface IParameter
  {
    #region properties
    
    /// <summary>
    /// Gets or sets a unique identifier by which this parameter may be identified.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    object Identifier { get; set; }
    
    /// <summary>
    /// Gets or sets a collection of 'short' names (typically only 1 character long each) for this parameter.
    /// </summary>
    /// <value>
    /// The short names.
    /// </value>
    IList<string> ShortNames { get; set; }
    
    /// <summary>
    /// Gets or sets a collection of 'long' names (may be over 1 character each, but with no spaces) for this parameter.
    /// </summary>
    /// <value>
    /// The long names.
    /// </value>
    IList<string> LongNames { get; set; }
    
    /// <summary>
    /// Gets or sets the behaviour of this parameter.
    /// </summary>
    /// <value>
    /// The behaviour.
    /// </value>
    ParameterBehaviour Behaviour { get; set; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Reads the value associated with this parameter.
    /// </summary>
    /// <returns>
    /// The value.
    /// </returns>
    object GetValue();
    
    /// <summary>
    /// Sets a value into the current instance.
    /// </summary>
    /// <param name='value'>
    /// The value to store.
    /// </param>
    void SetValue(string value);
    
    #endregion
  }
}

