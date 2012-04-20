//  
//  Validator.cs
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
using System.Linq.Expressions;
using System.Reflection;
using CSF.Reflection;

namespace CSF.Validation
{
  /// <summary>
  /// Validator type that provides validation services for object instances against a registered set of validation
  /// tests.
  /// </summary>
  /// <typeparam name='TTarget'>
  /// The type of object that this validator targets and provides validation for.
  /// </typeparam>
  public class Validator<TTarget> : IValidator<TTarget>
  {
    #region fields
    
    private IList<IValidationTest<TTarget>> _tests;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets or sets a collection of the validation tests defined for the current instance.
    /// </summary>
    /// <value>
    /// A collection of the validation rules.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public IList<IValidationTest<TTarget>> Tests
    {
      get {
        return _tests;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _tests = value;
      }
    }
    
    #endregion
    
    #region test registration
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test will validate an object in general terms, without
    /// an association to a particular member of the instance.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    public IValidator<TTarget> AddTest(ValidationFunction<TTarget> test)
    {
      return this.AddTest(test, null);
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test will validate an object in general terms, without
    /// an association to a particular member of the instance.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <param name='testIdentifier'>
    /// An identifier for the test, that allows it to be distinguished from other tests.
    /// </param>
    public IValidator<TTarget> AddTest(ValidationFunction<TTarget> test,
                                       object testIdentifier)
    {
      this.Tests.Add(new ValidationTest<TTarget, TTarget>(test, testIdentifier));
      return this;
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test is associated with a specific member of the
    /// target object being validated and will be performed against the value of that member.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='member'>
    /// The member that this test is to be associated with.
    /// </param>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <typeparam name='TMember'>
    /// The output/return type of the <paramref name="member"/> that this test is associated with.
    /// </typeparam>
    public IValidator<TTarget> AddTest<TMember>(Expression<Func<TTarget, TMember>> member,
                                                ValidationFunction<TMember> test)
    {
      return this.AddTest<TMember>(member, test, null);
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test is associated with a specific member of the
    /// target object being validated and will be performed against the value of that member.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='member'>
    /// The member that this test is to be associated with.
    /// </param>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <typeparam name='TMember'>
    /// The output/return type of the <paramref name="member"/> that this test is associated with.
    /// </typeparam>
    public IValidator<TTarget> AddTest<TMember>(MemberInfo member,
                                                ValidationFunction<TMember> test)
    {
      return this.AddTest<TMember>(member, test, null);
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test is associated with a specific member of the
    /// target object being validated and will be performed against the value of that member.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='member'>
    /// The member that this test is to be associated with.
    /// </param>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <param name='testIdentifier'>
    /// An identifier for the test, that allows it to be distinguished from other tests.
    /// </param>
    /// <typeparam name='TMember'>
    /// The output/return type of the <paramref name="member"/> that this test is associated with.
    /// </typeparam>
    public IValidator<TTarget> AddTest<TMember>(Expression<Func<TTarget, TMember>> member,
                                                ValidationFunction<TMember> test,
                                                object testIdentifier)
    {
      return this.AddTest<TMember>(StaticReflectionUtility.GetMember<TTarget, TMember>(member), test, testIdentifier);
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test is associated with a specific member of the
    /// target object being validated and will be performed against the value of that member.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='member'>
    /// The member that this test is to be associated with.
    /// </param>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <param name='testIdentifier'>
    /// An identifier for the test, that allows it to be distinguished from other tests.
    /// </param>
    /// <typeparam name='TMember'>
    /// The output/return type of the <paramref name="member"/> that this test is associated with.
    /// </typeparam>
    public IValidator<TTarget> AddTest<TMember>(MemberInfo member,
                                                ValidationFunction<TMember> test,
                                                object testIdentifier)
    {
      this.Tests.Add(new ValidationTest<TTarget, TMember>(test, member, testIdentifier));
      return this;
    }
    
    #endregion
    
    #region validation
    
    public bool Validate (TTarget target)
    {
      throw new NotImplementedException ();
    }

    public bool Validate (TTarget target, bool throwOnFailure)
    {
      throw new NotImplementedException ();
    }

    public bool Validate (TTarget target, out ValidationFailureList<TTarget> failures)
    {
      throw new NotImplementedException ();
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new validator instance.
    /// </summary>
    public Validator()
    {
      this.Tests = new List<IValidationTest<TTarget>>();
    }
    
    #endregion
  }
}

