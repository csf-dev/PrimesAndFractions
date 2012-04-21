//  
//  ValidationTest.cs
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
using System.Reflection;

namespace CSF.Validation
{
  /// <summary>
  /// Immutable specification type for a validation test, to be used with an <c>IValidator&lt;TTarget&gt;</c>
  /// </summary>
  public class ValidationTest<TTarget> : IValidationTest<TTarget>
  {
    #region fields
    
    private ValidationFunction<TTarget> _test;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets the member associated with this test.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this test is associated with a specific member then this will be non-null.  If the test is unassociated and
    /// is performed on the overall object instance then this property will return <c>null</c>.
    /// </para>
    /// <para>
    /// If this property is non-null then the <see cref="Test"/> function will be performed upon the value of the
    /// member.
    /// </para>
    /// </remarks>
    /// <value>
    /// The member.
    /// </value>
    public virtual MemberInfo Member
    {
      get;
      protected set;
    }
    
    /// <summary>
    /// Gets the test function that this instance performs.
    /// </summary>
    /// <value>
    /// The test function.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public virtual ValidationFunction<TTarget> Test
    {
      get {
        return _test;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _test = value;
      }
    }
    
    /// <summary>
    /// Gets an identifier that may be used to distinguish this test from others.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public virtual object Identifier
    {
      get;
      protected set;
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Executes/performs this test against the specified target object instance.
    /// </summary>
    /// <param name='target'>
    /// The object instance to perform this test against.
    /// </param>
    public virtual bool Execute(TTarget target)
    {
      if(target == null)
      {
        throw new ArgumentNullException ("target");
      }
      
      return this.Test(target);
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new validation test instance.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this test, to distinguish it from other tests.
    /// </param>
    /// <param name='member'>
    /// The member that this test is associated with.
    /// </param>
    protected ValidationTest(object identifier, MemberInfo member)
    {
      this.Identifier = identifier;
      this.Member = member;
    }
    
    /// <summary>
    /// Initializes a new validation test instance.
    /// </summary>
    /// <param name='test'>
    /// The test function that this instance performs.
    /// </param>
    /// <param name='identifier'>
    /// The identifier for this test, to distinguish it from other tests.
    /// </param>
    public ValidationTest(ValidationFunction<TTarget> test, object identifier) : this(identifier, null)
    {
      this.Test = test;
    }
    
    #endregion
  }
}

