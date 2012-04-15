//  
//  IniSection.cs
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
using System.Text.RegularExpressions;

namespace CSF.IO
{
  /// <summary>
  /// <para>Represents a section of configuration settings within an <see cref="IniDocument"/>.</para>
  /// </summary>
  public class IniSection : Dictionary<string, string>, IIniSection
  {
    #region constants
    
    private const string VALID_KEY = @"^\S(.*\S)?$";
    private static readonly Regex ValidKey = new Regex(VALID_KEY, RegexOptions.Compiled);
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Gets and sets settings by their key.</para>
    /// </summary>
    /// <param name="key">
    /// A <see cref="System.String"/>
    /// </param>
    public new string this[string key]
    {
      get {
        ValidateKey(key);
        return base[key];
      }
      set {
        ValidateKey(key);
        base[key] = value;
      }
    }
    
    #endregion
    
    #region publicMethods
    
    /// <summary>
    /// <para>Adds a new setting to this instance.</para>
    /// </summary>
    /// <param name="key">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="value">
    /// A <see cref="System.String"/>
    /// </param>
    public new void Add (string key, string value)
    {
      ValidateKey(key);
      base.Add(key, value);
    }
    
    /// <summary>
    /// <para>Removes a key from the current instance.</para>
    /// </summary>
    /// <param name="key">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public new bool Remove (string key)
    {
      ValidateKey(key);
      return base.Remove(key);
    }
    
    #endregion
    
    #region privateMethods
    
    /// <summary>
    /// Validates a string key.
    /// </summary>
    /// <param name='key'>
    /// The key to validate
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown if <paramref name="key"/> is null.
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown if <paramref name="key"/> is not a valid key within an INI document.
    /// </exception>
    private void ValidateKey(string key)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }
      
      if(!ValidKey.Match(key).Success)
      {
        throw new ArgumentException(String.Format("Invalid INI key: '{0}'", key));
      }
    }
    
    #endregion
  
    #region constructor
    
    /// <summary>
    /// <para>Initialises the current instance.</para>
    /// </summary>
    public IniSection() : base() {}
  
    #endregion
  }
}
