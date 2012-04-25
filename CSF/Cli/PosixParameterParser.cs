//  
//  PosixParameterParser.cs
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
using System.Reflection;

namespace CSF.Cli
{
  /// <summary>
  /// Implementation of an <see cref="IParameterParser"/> that deals with POSIX-style parameters.
  /// </summary>
  public class PosixParameterParser : IParameterParser
  {
    #region constants
    
    /// <summary>
    /// The parameters that make up the method signature for the constructor that is required of any type that
    /// implements <see cref="IParsedParameters"/>.
    /// </summary>
    private static readonly Type[]
      IParsedParametersCtorSignature = new Type[] { typeof(IDictionary<object, IParameter>), typeof(IList<string>) };
    
    #endregion
    
    #region fields
    
    private IList<IParameter> _registeredParameters;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets or sets a collection of the parameters registered with this instance.
    /// </summary>
    /// <value>
    /// The registered parameters.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public IList<IParameter> RegisteredParameters
    {
      get {
        return _registeredParameters;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _registeredParameters = value;
      }
    }
    
    #endregion
    
    #region registering parameters
    
    /// <summary>
    /// Adds/registers a single parameter with the parser.  The parameter implicitly has the
    /// <see cref="ParameterBehaviour.Switch"/> behaviour.
    /// </summary>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <param name='shortName'>
    /// The short name of the parameter (typically one character long).
    /// </param>
    /// <param name='longName'>
    /// The long name of the parameter (multiple characters, with no spaces)
    /// </param>
    /// <param name='identifier'>
    /// A unique identifier for the parameter.
    /// </param>
    public IParameterParser AddParameter(string shortName,
                                         string longName,
                                         object identifier)
    {
      return this.AddParameter<bool>(shortName, longName, ParameterBehaviour.Switch, identifier);
    }

    /// <summary>
    /// Adds/registers a single parameter with the parser.  The parameter implicitly has the
    /// <see cref="ParameterBehaviour.Switch"/> behaviour.
    /// </summary>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <param name='shortNames'>
    /// A collection of the short names for the parameter (typically each is one character long).
    /// </param>
    /// <param name='longNames'>
    /// A collection of the long names for the parameter (each may be multiple characters, with no spaces)
    /// </param>
    /// <param name='identifier'>
    /// A unique identifier for the parameter.
    /// </param>
    public IParameterParser AddParameter(IList<string> shortNames,
                                         IList<string> longNames,
                                         object identifier)
    {
      return this.AddParameter<bool>(shortNames, longNames, ParameterBehaviour.Switch, identifier);
    }

    /// <summary>
    /// Adds/registers a single parameter with the parser.
    /// </summary>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <param name='shortName'>
    /// The short name of the parameter (typically one character long).
    /// </param>
    /// <param name='longName'>
    /// The long name of the parameter (multiple characters, with no spaces)
    /// </param>
    /// <param name='behaviour'>
    /// The behaviour of the parameter.
    /// </param>
    /// <param name='identifier'>
    /// A unique identifier for the parameter.
    /// </param>
    /// <typeparam name='TParameterValue'>
    /// The expected return value of the parameter.
    /// </typeparam>
    public IParameterParser AddParameter<TParameterValue>(string shortName,
                                                          string longName,
                                                          ParameterBehaviour behaviour,
                                                          object identifier)
    {
      IList<string> shortNames, longNames;
      
      shortNames = (shortName != null)? new string[] { shortName } : new string[0];
      longNames = (longName != null)? new string[] { longName } : new string[0];
      
      return this.AddParameter<TParameterValue>(shortNames, longNames, behaviour, identifier);
    }

    /// <summary>
    /// Adds/registers a single parameter with the parser.
    /// </summary>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <param name='shortNames'>
    /// A collection of the short names for the parameter (typically each is one character long).
    /// </param>
    /// <param name='longNames'>
    /// A collection of the long names for the parameter (each may be multiple characters, with no spaces)
    /// </param>
    /// <param name='behaviour'>
    /// The behaviour of the parameter.
    /// </param>
    /// <param name='identifier'>
    /// A unique identifier for the parameter.
    /// </param>
    /// <typeparam name='TParameterValue'>
    /// The expected return value of the parameter.
    /// </typeparam>
    public IParameterParser AddParameter<TParameterValue>(IList<string> shortNames,
                                                          IList<string> longNames,
                                                          ParameterBehaviour behaviour,
                                                          object identifier)
    {
      this.RegisteredParameters.Add(new Parameter<TParameterValue>() {
        ShortNames = shortNames,
        LongNames = longNames,
        Behaviour = behaviour,
        Identifier = identifier
      });
      return this;
    }

    /// <summary>
    /// Registers multiple parameters with the parser, using the properties and fields from the type
    /// <typeparamref name="TParameterType"/> to detect the properties of each parameter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method inspects the members of the <typeparamref name="TParameterType"/> for those decorated with 
    /// <see cref="ParameterAttribute"/> in order to discover parameter specifications.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <typeparam name='TParameterType'>
    /// A type that implements <see cref="IParsedParameters"/> from which to read the parameter specifications.
    /// </typeparam>
    public IParameterParser AddParameters<TParameterType>() where TParameterType : IParsedParameters
    {
      Type parameterType = typeof(TParameterType);
      ConstructorInfo constructor = parameterType.GetConstructor(IParsedParametersCtorSignature);
      
      if(constructor == null)
      {
        string message = String.Format("The parameter result/output type `{0}' does not contain a constructor with" +
                                       "the required signature: (IDictionary<object, IParameter>, IList<string>)",
                                       parameterType.FullName);
        throw new InvalidOperationException(message);
      }
      
      foreach(PropertyInfo property in parameterType.GetProperties())
      {
        object[] allAttributes = property.GetCustomAttributes(typeof(ParameterAttribute), true);
        if(allAttributes.Length == 1)
        {
          this.AddParameter(property, (ParameterAttribute) allAttributes[0]);
        }
      }
      
      return this;
    }
    
    /// <summary>
    /// Adds a single parameter to the current instance.
    /// </summary>
    /// <param name='property'>
    /// The property for which to add the parameter.
    /// </param>
    /// <param name='attribute'>
    /// The <see cref="ParameterAttribute"/> found on that property.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    private void AddParameter(PropertyInfo property, ParameterAttribute attribute)
    {
      if(property == null)
      {
        throw new ArgumentNullException ("property");
      }
      if(attribute == null)
      {
        throw new ArgumentNullException ("attribute");
      }
          
      Type parameterGenericType = typeof(Parameter<>).MakeGenericType(new Type[] { property.PropertyType });
      IParameter parameter = (IParameter) parameterGenericType.GetConstructor(Type.EmptyTypes).Invoke(null);
      
      parameter.Behaviour = attribute.Behaviour;
      parameter.Identifier = property;
      
      if(attribute.ShortName != null)
      {
        parameter.ShortNames.Add(attribute.ShortName);
      }
      if(attribute.LongName != null)
      {
        parameter.LongNames.Add(attribute.LongName);
      }
      
      foreach(ParameterNameAttribute name in property.GetCustomAttributes(typeof(ParameterNameAttribute), true))
      {
        var paramList = name.IsLongName? parameter.LongNames : parameter.ShortNames;
        paramList.Add(name.Name);
      }
      
      this.RegisteredParameters.Add(parameter);
    }
    
    #endregion
    
    #region parsing

    public IParsedParameters Parse(IList<string> commandLineArguments)
    {
      return this.Parse<ParsedParameters>(commandLineArguments);
    }

    public TOutput Parse<TOutput>(IList<string> commandLineArguments) where TOutput : IParsedParameters
    {
      throw new NotImplementedException ();
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.PosixParameterParser"/> class.
    /// </summary>
    public PosixParameterParser ()
    {
      this.RegisteredParameters = new List<IParameter>();
    }
    
    #endregion
  }
}

