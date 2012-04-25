//  
//  BinaryHashAndSaltPair.cs
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
  /// Immutable type represents a password hash and salt pair based on binary data.
  /// </summary>
  public class BinaryHashAndSaltPair : IHashAndSaltPair
  {
    #region fields
    
    private byte[] _hash, _salt;
    
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
    public byte[] Hash
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
    public byte[] Salt
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
      return Convert.ToBase64String(this.Hash);
    }
    
    string IHashAndSaltPair.GetSalt()
    {
      return Convert.ToBase64String(this.Salt);
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Security.BinaryHashAndSaltPair"/> class.
    /// </summary>
    /// <param name='hash'>
    /// The hash.
    /// </param>
    /// <param name='salt'>
    /// The salt.
    /// </param>
    public BinaryHashAndSaltPair(byte[] hash, byte[] salt)
    {
      this.Hash = hash;
      this.Salt = salt;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Security.BinaryHashAndSaltPair"/> class.
    /// </summary>
    /// <param name='base64Hash'>
    /// The hash, encoded as a Base64 string.
    /// </param>
    /// <param name='base64Salt'>
    /// The salt, encoded as a Base64 string.
    /// </param>
    public BinaryHashAndSaltPair(string base64Hash, string base64Salt)
    {
      this.Hash = Convert.FromBase64String(base64Hash);
      this.Salt = Convert.FromBase64String(base64Salt);
    }
    
    #endregion
  }
}

