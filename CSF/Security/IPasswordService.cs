//  
//  IPasswordService.cs
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
  /// Interface for a service that provides password hashing and checking.
  /// </summary>
  public interface IPasswordService
  {
    #region properties
    
    /// <summary>
    /// Gets the name of the hashing algorithm that will be used by this instance.
    /// </summary>
    /// <value>
    /// The name of the algorithm.
    /// </value>
    string AlgorithmName { get; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Generates a new password hash and salt pair from a password, using the specified length of password salt.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The meaning of <paramref name="saltLength"/> is implementation-dependent.  It could mean (for example) the
    /// number of printable characters that comprise the salt, or it could mean the number of bytes of data.
    /// </para>
    /// <para>
    /// The password salt is randomly-generated.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A hash-and-salt pair corresponding to the <paramref name="password"/>.
    /// </returns>
    /// <param name='password'>
    /// The password to hash.
    /// </param>
    /// <param name='saltLength'>
    /// The length of salt to use, please read the remarks for more info.
    /// </param>
    IHashAndSaltPair GenerateHash(string password, int saltLength);
    
    /// <summary>
    /// Determines whether the specified password matches the specified hash and salt.
    /// </summary>
    /// <returns>
    /// Whether or not the password matches the hash/salt pair or not.
    /// </returns>
    /// <param name='password'>
    /// The password to test.
    /// </param>
    /// <param name='hashAndSalt'>
    /// The hash/salt pair to test against.
    /// </param>
    bool PasswordMatches(string password, IHashAndSaltPair hashAndSalt);
    
    /// <summary>
    /// Generates a randomised string within a specified dictionary.
    /// </summary>
    /// <returns>
    /// The random string.
    /// </returns>
    /// <param name='dictionary'>
    /// A string that indicates a 'dictionary' of characters that may be used.
    /// </param>
    /// <param name='length'>
    /// The length of randomised string to create.
    /// </param>
    string GenerateRandomString(string dictionary, int length);
    
    /// <summary>
    /// Generates a randomised string within a specified dictionary.
    /// </summary>
    /// <returns>
    /// The random string.
    /// </returns>
    /// <param name='dictionary'>
    /// A character-array that indicates a 'dictionary' of characters that may be used.
    /// </param>
    /// <param name='length'>
    /// The length of randomised string to create.
    /// </param>
    string GenerateRandomString(char[] dictionary, int length);
    
    #endregion
  }
}

