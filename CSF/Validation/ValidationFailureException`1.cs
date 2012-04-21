//  
//  ValidationFailureException.cs
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
  /// An exception thrown by instances of <c>IValidator&lt;TTarget&gt;</c> when validation fails and the parameters of
  /// the validator indicate that an exception should be thrown.
  /// </summary>
  /// <remarks>
  /// This exception may also be thrown if an individual validation test raises an exception.
  /// </remarks>
  public class ValidationFailureException<TTarget> : ValidationFailureException
  {
    #region properties
    
    /// <summary>
    /// Gets the type of the object that was being validated.
    /// </summary>
    /// <value>
    /// The <see cref="System.Type"/> of the target object.
    /// </value>
    public override Type TargetType
    {
      get {
        return typeof(TTarget);
      }
    }
    
    /// <summary>
    /// Gets a collection of the validation failures that lead to the raising of this exception.
    /// </summary>
    /// <value>
    /// A collection of validation failures.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public ValidationTestList<TTarget> Failures
    {
      get {
        return (ValidationTestList<TTarget>) this.Data["Failures"];
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        this.Data["Failures"] = value;
      }
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name='failures'>
    /// The collection of validation failures.
    /// </param>
    public ValidationFailureException(ValidationTestList<TTarget> failures) : this(failures, null) {}
    
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name='failures'>
    /// The collection of validation failures.
    /// </param>
    /// <param name='inner'>
    /// An <see cref="System.Exception"/> that was encountered whilst performing validation.
    /// </param>
    public ValidationFailureException(ValidationTestList<TTarget> failures, Exception inner) : base(String.Format("Validation failure of a `{0}'",
                                                                                                                  typeof(TTarget).FullName),
                                                                                                    inner)
    {
      this.Failures = failures;
    }
    
    #endregion
  }
}

