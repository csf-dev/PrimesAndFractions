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
  /// Immutable specification type for a validation test, to be used with an <see cref="IValidator"/>.
  /// </summary>
  public class ValidationTest<TTarget, TValue> : IValidationTest<TTarget>
  {
    #region fields
    
    private ValidationFunction<TValue> _test;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets the member associated with this test.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this 
    /// </para>
    /// </remarks>
    /// <value>
    /// The member.
    /// </value>
    public MemberInfo Member
    {
      get;
      private set;
    }
    
    public ValidationFunction<TValue> Test
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
    
    public object Identifier
    {
      get;
      private set;
    }
    
    #endregion
    
    #region methods
    
    public bool Execute(TTarget target)
    {
      // TODO: Write this implementation
      throw new NotImplementedException ();
    }
    
    #endregion
    
    #region constructor
    
    public ValidationTest(ValidationFunction<TValue> test, object identifier) : this(test, null, identifier) {}
    
    public ValidationTest(ValidationFunction<TValue> test, MemberInfo member, object identifier)
    {
      this.Test = test;
      this.Member = member;
      this.Identifier = identifier;
    }
    
    #endregion
  }
}

