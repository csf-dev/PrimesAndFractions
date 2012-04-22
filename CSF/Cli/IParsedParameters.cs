//  
//  IParameters.cs
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
using System.Linq.Expressions;

namespace CSF.Cli
{
  /// <summary>
  /// Interface for the output of an <see cref="IParameterParser"/>'s parsing functionality.
  /// </summary>
  public interface IParsedParameters
  {
    #region properties
    
    /// <summary>
    /// Gets a collection of the remaining arguments; these are command-line arguments that have not been treated as
    /// parameters.
    /// </summary>
    /// <value>
    /// The remaining arguments.
    /// </value>
    IEnumerable<string> RemainingArguments { get; }
    
    /// <summary>
    /// Gets a collection of the <see cref="IParameter"/>s that have been successfully parsed.
    /// </summary>
    /// <value>
    /// The parameters.
    /// </value>
    IEnumerable<IParameter> Parameters { get; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Gets a value that indicates whether the parameter (identified using the specified <paramref name="identifier"/>)
    /// is contained within this instance.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    bool Contains(object identifier);
    
    /// <summary>
    /// Gets a parameter using the specified identifier.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    IParameter Get(object identifier);
    
    /// <summary>
    /// Gets a strongly-typed parameter using the specified identifier.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    /// <typeparam name='TParameterValue'>
    /// The value-type for the parameter.
    /// </typeparam>
    IParameter<TParameterValue> Get<TParameterValue>(object identifier);
    
    #endregion
  }
}

