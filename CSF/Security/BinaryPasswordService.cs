//  
//  StringPasswordService.cs
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
using System.Security.Cryptography;
using System.Text;

namespace CSF.Security
{
  /// <summary>
  /// An implementation of <see cref="IPasswordService"/> that uses binary data to store its password hashes/salts.
  /// </summary>
  public class BinaryPasswordService : IPasswordService
  {
    #region fields
    
    private static RandomNumberGenerator _randomiser;
    private HashAlgorithm _algorithm;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets the name of the hashing algorithm that will be used by this instance.
    /// </summary>
    /// <value>
    /// The name of the algorithm.
    /// </value>
    public string AlgorithmName
    {
      get {
        return this.Algorithm.GetType().FullName;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        try
        {
          this.Algorithm = HashAlgorithm.Create(value);
        }
        catch(ArgumentNullException ex)
        {
          throw new ArgumentException("Value does not indicate a valid algorithm name", ex);
        }
      }
    }
    
    /// <summary>
    /// Gets the randomiser associated with this type.
    /// </summary>
    /// <value>
    /// The randomiser.
    /// </value>
    protected RandomNumberGenerator Randomiser
    {
      get {
        return _randomiser;
      }
    }
    
    /// <summary>
    /// Gets the hash algorithm that this type uses.
    /// </summary>
    /// <value>
    /// The algorithm.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    protected HashAlgorithm Algorithm
    {
      get {
        return _algorithm;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _algorithm = value;
      }
    }
    
    #endregion
    
    #region IPasswordService implementation
    
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
    public virtual IHashAndSaltPair GenerateHash(string password, int saltLength)
    {
      if(password == null)
      {
        throw new ArgumentNullException ("password");
      }
      
      byte[] randomSalt = this.CreateRandomSalt(saltLength);
      byte[] computedHash = this.GenerateHash(password, randomSalt);
      
      return new BinaryHashAndSaltPair(computedHash, randomSalt);
    }
  
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
    public bool PasswordMatches(string password, IHashAndSaltPair hashAndSalt)
    {
      BinaryHashAndSaltPair typedHashAndSalt = hashAndSalt as BinaryHashAndSaltPair;
      
      if(password == null)
      {
        throw new ArgumentNullException ("password");
      }
      else if(typedHashAndSalt == null)
      {
        throw new ArgumentException("Hash and salt are either null or not a BinaryHashAndSaltPair.");
      }
      
      byte[] generatedHash = this.GenerateHash(password, typedHashAndSalt.Salt);
      return Convert.ToBase64String(generatedHash) == hashAndSalt.GetHash();
    }
  
    /// <summary>
    /// Generates a random string within the given <paramref name="dictionary"/> of characters.
    /// </summary>
    /// <returns>
    /// The random string.
    /// </returns>
    /// <param name='dictionary'>
    /// A 'dictionary' of characters to use in generating the output.
    /// </param>
    /// <param name='length'>
    /// The length of output in characters.
    /// </param>
    public string GenerateRandomString(string dictionary, int length)
    {
      if(dictionary == null)
      {
        throw new ArgumentNullException ("dictionary");
      }
      
      return this.GenerateRandomString(dictionary.ToCharArray(), length);
    }
  
    /// <summary>
    /// Generates a random string within the given <paramref name="dictionary"/> of characters.
    /// </summary>
    /// <returns>
    /// The random string.
    /// </returns>
    /// <param name='dictionary'>
    /// A 'dictionary' of characters to use in generating the output.
    /// </param>
    /// <param name='length'>
    /// The length of output in characters.
    /// </param>
    public string GenerateRandomString(char[] dictionary, int length)
    {
      StringBuilder output = new StringBuilder();
      
      if(dictionary == null)
      {
        throw new ArgumentNullException ("dictionary");
      }
      else if(dictionary.Length < 1)
      {
        throw new ArgumentException("Dictionary does not contain any characters.");
      }
      else if(length < 0)
      {
        throw new ArgumentOutOfRangeException("length", "Output length must not be less than zero.");
      }
      else if(dictionary.Length > UInt16.MaxValue)
      {
        throw new ArgumentException("Dictionary of characters to use is too long.");
      }
      
      for(int i = 0; i < length; i++)
      {
        byte[] randomBytes = new byte[2];
        this.Randomiser.GetBytes(randomBytes);
        
        ushort randomNumber = BitConverter.ToUInt16(randomBytes, 0);
        int index = (int) Math.Round((double) randomNumber * (double) (dictionary.Length - 1) / (double) UInt16.MaxValue);
        
        output.Append(dictionary[index]);
      }
      
      return output.ToString();
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Creates a new randomised password salt of the specified length.
    /// </summary>
    /// <returns>
    /// The random salt.
    /// </returns>
    /// <param name='length'>
    /// The length of salt to return, in bytes.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="length"/> is less than zero.
    /// </exception>
    protected virtual byte[] CreateRandomSalt(int length)
    {
      if(length < 0)
      {
        throw new ArgumentOutOfRangeException("length", "Salt length may not be negative.");
      }
      
      byte[] output = new byte[length];
      this.Randomiser.GetBytes(output);
      
      return output;
    }
    
    /// <summary>
    /// Generates a password hash for the given password and salt.
    /// </summary>
    /// <returns>
    /// The hash.
    /// </returns>
    /// <param name='password'>
    /// Password.
    /// </param>
    /// <param name='salt'>
    /// Salt.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    protected virtual byte[] GenerateHash(string password, byte[] salt)
    {
      if(password == null)
      {
        throw new ArgumentNullException ("password");
      }
      else if(salt == null)
      {
        throw new ArgumentNullException ("salt");
      }
      
      byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
      byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
      
      Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
      Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);
      
      return this.Algorithm.ComputeHash(saltedPassword);
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Security.BinaryPasswordService"/> class.
    /// </summary>
    /// <param name='algorithmName'>
    /// The name of the hashing algorithm to use.
    /// </param>
    public BinaryPasswordService(string algorithmName)
    {
      this.AlgorithmName = algorithmName;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Security.BinaryPasswordService"/> class.
    /// </summary>
    /// <param name='algorithm'>
    /// The algorithm to use.
    /// </param>
    public BinaryPasswordService(HashAlgorithm algorithm)
    {
      this.Algorithm = algorithm;
    }
    
    /// <summary>
    /// Initializes the <see cref="CSF.Security.BinaryPasswordService"/> class.
    /// </summary>
    static BinaryPasswordService()
    {
      _randomiser = new RNGCryptoServiceProvider();
    }
    
    #endregion
  }
}

