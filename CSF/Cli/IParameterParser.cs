//  
//  IParameterParser.cs
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
  /// Interface for a commandline parameter parser.
  /// </summary>
  public interface IParameterParser
  {
    #region adding definitions
    
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
    IParameterParser AddParameter(string shortName,
                                  string longName,
                                  object identifier);
    
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
    IParameterParser AddParameter(IList<string> shortNames,
                                  IList<string> longNames,
                                  object identifier);
    
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
    IParameterParser AddParameter<TParameterValue>(string shortName,
                                                   string longName,
                                                   ParameterBehaviour behaviour,
                                                   object identifier);
    
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
    IParameterParser AddParameter<TParameterValue>(IList<string> shortNames,
                                                   IList<string> longNames,
                                                   ParameterBehaviour behaviour,
                                                   object identifier);
    
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
    IParameterParser AddParameters<TParameterType>() where TParameterType : IParsedParameters;
    
    #endregion
    
    #region parsing
    
    /// <summary>
    /// Parse the specified command line arguments, returning an object that contains the results of the parsing.
    /// </summary>
    /// <param name='commandLineArguments'>
    /// The input command line arguments from which to parse parameters.
    /// </param>
    IParsedParameters Parse(IList<string> commandLineArguments);
    
    /// <summary>
    /// Parse the specified command line arguments, returning an object that contains the results of the parsing.
    /// </summary>
    /// <param name='commandLineArguments'>
    /// The input command line arguments from which to parse parameters.
    /// </param>
    /// <typeparam name='TOutput'>
    /// A type that implements <see cref="IParsedParameters"/>, providing a strongly-typed container for the parsing
    /// results.
    /// </typeparam>
    TOutput Parse<TOutput>(IList<string> commandLineArguments) where TOutput : IParsedParameters;
    
    #endregion
  }
}

