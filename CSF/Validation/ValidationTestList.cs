//  
//  ValidationFailureCollection.cs
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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CSF.Reflection;

namespace CSF.Validation
{
  /// <summary>
  /// Represents a collection of validation failures.
  /// </summary>
  public class ValidationTestList<TTarget> : List<IValidationTest<TTarget>>
  {
    #region methods
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) are for the specified <paramref name="member"/>.
    /// </summary>
    /// <returns>
    /// A collection of validation failures, possibly empty.
    /// </returns>
    /// <param name='member'>
    /// The member that we are interested in.
    /// </param>
    public ValidationTestList<TTarget> ByMember(Expression<Func<TTarget, object>> member)
    {
      return this.ByMember(StaticReflectionUtility.GetMember<TTarget>(member));
    }
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) are for the specified <paramref name="member"/>.
    /// </summary>
    /// <returns>
    /// A collection of validation failures, possibly empty.
    /// </returns>
    /// <param name='member'>
    /// The member that we are interested in.
    /// </param>
    public ValidationTestList<TTarget> ByMember(MemberInfo member)
    {
      return this.ByIdentifier(null, member);
    }
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) have the specified <paramref name="identifier"/>.
    /// </summary>
    /// <returns>
    /// The identifier.
    /// </returns>
    /// <param name='identifier'>
    /// The identifier value to search for.
    /// </param>
    public ValidationTestList<TTarget> ByIdentifier(object identifier)
    {
      return this.ByIdentifier(identifier, (MemberInfo) null);
    }
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) have the specified
    /// <paramref name="identifier"/> and are for the specified <paramref name="member"/>.
    /// </summary>
    /// <returns>
    /// A collection of validation failures, possibly empty.
    /// </returns>
    /// <param name='identifier'>
    /// The identifier value to search for.
    /// </param>
    /// <param name='member'>
    /// The member that we are interested in.
    /// </param>
    public ValidationTestList<TTarget> ByIdentifier(object identifier, Expression<Func<TTarget, object>> member)
    {
      return this.ByIdentifier(identifier, StaticReflectionUtility.GetMember<TTarget>(member));
    }
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) have the specified
    /// <paramref name="identifier"/> and are for the specified <paramref name="member"/>.
    /// </summary>
    /// <returns>
    /// A collection of validation failures, possibly empty.
    /// </returns>
    /// <param name='identifier'>
    /// The identifier value to search for.
    /// </param>
    /// <param name='member'>
    /// The member that we are interested in.
    /// </param>
    public ValidationTestList<TTarget> ByIdentifier(object identifier, MemberInfo member)
    {
      ValidationTestList<TTarget> output = new ValidationTestList<TTarget>();
      
      if(identifier == null && member == null)
      {
        throw new InvalidOperationException("Both identifier and member cannot be null.");
      }
      
      var subset = (from x in this
                    where
                      (identifier == null || x.Identifier == identifier)
                      && (member == null || x.Member == member)
                    select x);
      output.AddRange(subset);
      
      return output;
    }
    
    #endregion
  }
}

