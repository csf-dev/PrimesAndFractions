//  
//  ParsedParametersT.cs
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
using System.Linq.Expressions;
using System.Collections.Generic;
using CSF.Reflection;

namespace CSF.Cli
{
  /// <summary>
  /// Generic implementation of <see cref="ParsedParameters"/> that is designed to be subclassed.  It adds some
  /// protected helper methods that derived classes will find useful for handling parameters.
  /// </summary>
  public abstract class ParsedParameters<TParameterContainer> : ParsedParameters, IParsedParameters
  {
    #region methods
    
    /// <summary>
    /// Gets a value that indicates whether the parameter (identified using the specified <paramref name="identifier"/>)
    /// is contained within this instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a convenience method intended for use by subclassing types that are created in conjunction with
    /// <see cref="ParameterAttribute"/>.  They provide a simple way of getting parameter values based on the member
    /// that the attribute was defined upon.
    /// </para>
    /// </remarks>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    protected bool Contains(Expression<Func<TParameterContainer, object>> identifier)
    {
      return this.Contains(Reflect.Member<TParameterContainer>(identifier));
    }
    
    /// <summary>
    /// Gets a parameter using the specified identifier.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    protected IParameter Get(Expression<Func<TParameterContainer, object>> identifier)
    {
      return this.Get(Reflect.Member<TParameterContainer>(identifier));
    }
    
    /// <summary>
    /// Gets a strongly-typed parameter using the specified identifier.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    /// <typeparam name='TParameterValue'>
    /// The value-type for the parameter.
    /// </typeparam>
    protected IParameter<TParameterValue> Get<TParameterValue>(Expression<Func<TParameterContainer, object>> identifier)
    {
      return this.Get<TParameterValue>(Reflect.Member<TParameterContainer>(identifier));
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.ParsedParameters"/> class.
    /// </summary>
    /// <param name='parameters'>
    /// The parameters that were successfuly parsed in the parsing operation.
    /// </param>
    /// <param name='remainingArguments'>
    /// The remaining arguments.
    /// </param>
    public ParsedParameters(IDictionary<object, IParameter> parameters,
                            IList<string> remainingArguments) : base(parameters, remainingArguments) {}
    
    #endregion
  }
}

