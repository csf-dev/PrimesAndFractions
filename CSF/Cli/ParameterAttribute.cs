//  
//  ParameterAttribute.cs
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
  /// Attribute used to decorate members of a type that implements <see cref="IParsedParameters"/>, providing
  /// information about the behaviour of the parameter.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class ParameterAttribute : Attribute
  {
    #region fields
    
    private ParameterBehaviour _behaviour;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets or sets a short name associated with the parameter.
    /// </summary>
    /// <value>
    /// The short name.
    /// </value>
    public string ShortName
    {
      get;
      set;
    }
    
    /// <summary>
    /// Gets or sets a long name associated with the parameter.
    /// </summary>
    /// <value>
    /// The long name.
    /// </value>
    public string LongName
    {
      get;
      set;
    }
    
    /// <summary>
    /// Gets or sets the behaviour of the parameter.
    /// </summary>
    /// <value>
    /// The behaviour.
    /// </value>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public ParameterBehaviour Behaviour
    {
      get {
        return _behaviour;
      }
      set {
        if(!Enum.IsDefined(typeof(ParameterBehaviour), value))
        {
          throw new ArgumentException("Unrecognised parameter behaviour.");
        }
        
        _behaviour = value;
      }
    }
    
    #endregion
    
    #region constructor
  
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.ParameterAttribute"/> class.
    /// </summary>
    /// <param name='behaviour'>
    /// The parameter behaviour.
    /// </param>
    public ParameterAttribute(ParameterBehaviour behaviour)
    {
      this.Behaviour = behaviour;
      this.ShortName = null;
      this.LongName = null;
    }
    
    #endregion
  }
}

