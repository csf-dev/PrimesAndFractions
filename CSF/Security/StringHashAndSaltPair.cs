//  
//  HashAndSaltPair.cs
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

namespace CSF.Security
{
  /// <summary>
  /// Immutable type represents a password hash and salt pair based on <see cref="System.String"/>s only.
  /// </summary>
  public class StringHashAndSaltPair : IHashAndSaltPair
  {
    #region fields
    
    private string _hash, _salt;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets the password hash.
    /// </summary>
    /// <value>
    /// The password hash.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public string Hash
    {
      get {
        return _hash;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _hash = value;
      }
    }
  
    /// <summary>
    /// Gets the password hash.
    /// </summary>
    /// <value>
    /// The password hash.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public string Salt
    {
      get {
        return _salt;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _salt = value;
      }
    }
    
    #endregion
    
    #region IHashAndSaltPair implementation
    
    string IHashAndSaltPair.GetHash()
    {
      return this.Hash;
    }
    
    string IHashAndSaltPair.GetSalt()
    {
      return this.Salt;
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Security.StringHashAndSaltPair"/> class.
    /// </summary>
    /// <param name='hash'>
    /// The hash
    /// </param>
    /// <param name='salt'>
    /// The salt
    /// </param>
    public StringHashAndSaltPair(string hash, string salt)
    {
      this.Hash = hash;
      this.Salt = salt;
    }
    
    #endregion
  }
}

