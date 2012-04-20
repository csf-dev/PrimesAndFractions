//  
//  ValidationFailureException1.cs
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

namespace CSF.Validation
{
  /// <summary>
  /// Base type for an exception that is thrown as a result of a validation failure.
  /// </summary>
  public abstract class ValidationFailureException : Exception
  {
    #region properties
    
    /// <summary>
    /// Gets the type of the object that was being validated.
    /// </summary>
    /// <value>
    /// The <see cref="System.Type"/> of the target object.
    /// </value>
    public abstract Type TargetType { get; }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Validation.ValidationFailureException"/> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    public ValidationFailureException(string message, Exception inner) : base(message, inner) {}
    
    #endregion
  }
}

