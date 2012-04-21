//  
//  IValidationTest.cs
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
  /// Interface for a validation test; a single test that may be performed against an object, which returns either
  /// <c>true</c> or <c>false</c> (pass or fail).
  /// </summary>
  public interface IValidationTest<TTarget>
  {
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
    /// If this property is non-null then the test will be performed upon the value of the member.
    /// </para>
    /// </remarks>
    /// <value>
    /// The member.
    /// </value>
    MemberInfo Member { get; }
    
    /// <summary>
    /// Gets an identifier that may be used to distinguish this test from others.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    object Identifier { get; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Executes/performs this test against the specified target object instance.
    /// </summary>
    /// <param name='target'>
    /// The object instance to perform this test against.
    /// </param>
    bool Execute(TTarget target);
    
    #endregion
  }
}

