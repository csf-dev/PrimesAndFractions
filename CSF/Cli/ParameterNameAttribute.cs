//  
//  ParameterShortNameAttribute.cs
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

namespace CSF.Cli
{
  /// <summary>
  /// Attribute that complements a <see cref="ParameterAttribute"/>, providing an alternate short or long name for the
  /// parameter.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
  public class ParameterNameAttribute : Attribute
  {
    #region fields
    
    private string _name;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets or sets a value indicating whether this instance is a long name.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is a long name; otherwise, <c>false</c>.
    /// </value>
    public bool IsLongName
    {
      get;
      set;
    }
  
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public string Name
    {
      get {
        return _name;
      }
      set {
        if (value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _name = value;
      }
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.ParameterNameAttribute"/> class.
    /// </summary>
    /// <param name='name'>
    /// Name.
    /// </param>
    public ParameterNameAttribute(string name)
    {
      this.Name = name;
      this.IsLongName = false;
    }
    
    #endregion
  }
}

