//  
//  ParsedParameters.cs
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
using CSF.Reflection;

namespace CSF.Cli
{
  /// <summary>
  /// Represents the results of a <see cref="IParameterParser"/>'s parsing activity.  Contains the parameters that were
  /// parsed and their values.
  /// </summary>
  public class ParsedParameters : IParsedParameters
  {
    #region fields
    
    private IDictionary<object, IParameter> _parameters;
    private IList<string> _remainingArguments;
    
    #endregion
    
    #region properties
    
    
    /// <summary>
    /// Gets a collection of the remaining arguments; these are command-line arguments that have not been treated as
    /// parameters.
    /// </summary>
    /// <value>
    /// The remaining arguments.
    /// </value>
    public IEnumerable<string> RemainingArguments
    {
      get {
        string[] output = new string[_remainingArguments.Count];
        _remainingArguments.CopyTo(output, 0);
        return output;
      }
    }
    
    /// <summary>
    /// Gets a collection of the <see cref="IParameter"/>s that have been successfully parsed.
    /// </summary>
    /// <value>
    /// The parameters.
    /// </value>
    public IEnumerable<IParameter> Parameters
    {
      get {
        return this.BaseParameters.Values;
      }
    }
    
    /// <summary>
    /// Gets or sets the parameters that were successfully parsed.
    /// </summary>
    /// <value>
    /// The base parameters.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    protected IDictionary<object, IParameter> BaseParameters
    {
      get {
        return _parameters;
      }
      private set {
        if (value == null) {
          throw new ArgumentNullException ("value");
        }
        
        _parameters = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Gets a value that indicates whether the parameter (identified using the specified <paramref name="identifier"/>)
    /// is contained within this instance.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    public bool Contains(object identifier)
    {
      return this.BaseParameters.ContainsKey(identifier);
    }
    
    /// <summary>
    /// Gets a parameter using the specified identifier.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    public IParameter Get(object identifier)
    {
      return this.BaseParameters[identifier];
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
    public IParameter<TParameterValue> Get<TParameterValue>(object identifier)
    {
      return this.Get(identifier) as IParameter<TParameterValue>;
    }
    
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
    protected bool Contains<TParsedParameters>(Expression<Func<TParsedParameters, object>> identifier)
    {
      return this.Contains(StaticReflectionUtility.GetMember<TParsedParameters>(identifier));
    }
    
    /// <summary>
    /// Gets a parameter using the specified identifier.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    protected IParameter Get<TParsedParameters>(Expression<Func<TParsedParameters, object>> identifier)
    {
      return this.Get(StaticReflectionUtility.GetMember<TParsedParameters>(identifier));
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
    protected IParameter<TParameterValue> Get<TParsedParameters, TParameterValue>(Expression<Func<TParsedParameters, object>> identifier)
    {
      return this.Get<TParameterValue>(StaticReflectionUtility.GetMember<TParsedParameters>(identifier));
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
    public ParsedParameters(IDictionary<object, IParameter> parameters, IList<string> remainingArguments)
    {
      this.BaseParameters = parameters;
      _remainingArguments = remainingArguments;
    }
    
    #endregion
  }
}

