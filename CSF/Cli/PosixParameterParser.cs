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
      // HACK: This method works but it could use some clean-up
      
      Type parameterType = typeof(TParameterType);
      Type[] constructorParams = new Type[] { typeof(IDictionary<object, IParameter>), typeof(IList<string>) };
      ConstructorInfo constructor = parameterType.GetConstructor(constructorParams);
      
      if(constructor == null)
      {
        string message = String.Format("The parameter result/output type `{0}' does not contain a constructor that" +
                                       "takes IDictionary<object, IParameter> and IList<string>.",
                                       parameterType.FullName);
        throw new InvalidOperationException(message);
      }
      
      foreach(MemberInfo member in parameterType.GetMembers())
      {
        PropertyInfo property = member as PropertyInfo;
        FieldInfo field = member as FieldInfo;
        
        if(property == null && field == null)
        {
          continue;
        }
        
        object[] allAttributes = member.GetCustomAttributes(typeof(ParameterAttribute), true);
        
        if(allAttributes.Length != 1)
        {
          continue;
        }
        
        ParameterAttribute attribute = (ParameterAttribute) allAttributes[0];
        
        List<string> shortNames = new List<string>();
        List<string> longNames = new List<string>();
        Type memberType;
        
        if(property != null)
        {
          memberType = property.PropertyType;
        }
        else
        {
          memberType = field.FieldType;
        }
        
        if(attribute.ShortName != null)
        {
          shortNames.Add(attribute.ShortName);
        }
        if(attribute.LongName != null)
        {
          longNames.Add(attribute.LongName);
        }
        
        foreach(ParameterNameAttribute name in member.GetCustomAttributes(typeof(ParameterNameAttribute), true))
        {
          if(name.IsLongName)
          {
            longNames.Add(name.Name);
          }
          else
          {
            shortNames.Add(name.Name);
          }
        }
        
        Type parameterValueType = typeof(Parameter<>).MakeGenericType(new Type[] { memberType });
        IParameter createdParameter = (IParameter) parameterValueType.GetConstructor(Type.EmptyTypes).Invoke(null);
        
        createdParameter.Behaviour = attribute.Behaviour;
        createdParameter.Identifier = member;
        createdParameter.ShortNames = shortNames;
        createdParameter.LongNames = longNames;
        
        this.RegisteredParameters.Add(createdParameter);
      }
      
      return this;
    }
    
    #endregion
    
    #region parsing

    public IParsedParameters Parse (IList<string> commandLineArguments)
    {
      throw new NotImplementedException ();
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

