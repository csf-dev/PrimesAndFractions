//  
//  ValidationTests.cs
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
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace CSF.Validation
{
  /// <summary>
  /// Helper type containing pre-written validation tests.
  /// </summary>
  public static class ValidationTests
  {
    #region fields
    
    /// <summary>
    /// Gets validation tests that may be performed on <see cref="System.String"/> members.
    /// </summary>
    public static readonly StringValidationTests String = new StringValidationTests();
    
    #endregion
    
    #region contained types
    
    /// <summary>
    /// String validation tests.
    /// </summary>
    public class StringValidationTests
    {
      private const string EMAIL_PATTERN = @"^[a-zA-Z0-9_+%-]+([a-zA-Z0-9._%+-]+[a-zA-Z0-9_+%-]+)?@([a-zA-Z0-9-]+\.)+[a-zA-Z0-9]{1,4}$";
      private static readonly Regex EmailAddressRegex = new Regex(EMAIL_PATTERN, RegexOptions.Compiled);
      
      /// <summary>
      /// Pre-written validation test for validating <see cref="System.String"/> email addresses.
      /// </summary>
      /// <value>
      /// A delegate that validates email addresses.
      /// </value>
      public ValidationFunction<string> EmailAddress
      {
        get {
          return (string x) => {
            bool output;
            
            if(x == null)
            {
              output = false;
            }
            else
            {
              try
              {
                /* Make an attempt to construct a MailAddress instance - if this throws an exception then it can't
                 * possibly be valid!
                 */
                new MailAddress(x);
                output = EmailAddressRegex.IsMatch(x);
              }
              catch(ArgumentException)
              {
                output = false;
              }
              catch(FormatException)
              {
                output = false;
              }
            }
            
            return output;
          };
        }
      }
    }
    
    #endregion  
  }
}

