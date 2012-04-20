//  
//  ReflectionHelper.cs
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
using System.Linq.Expressions;
using System.Reflection;

namespace CSF.Reflection
{
  /// <summary>
  /// Helper class for reflection-related tasks.
  /// </summary>
  public class StaticReflectionUtility
  {
    /// <summary>
    /// Gets a <see cref="MemberInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The member information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MemberInfo GetMember<TObject>(Expression<Func<TObject, object>> expression)
    {
      MemberExpression memberExpression = null;
      
      if(expression == null)
      {
        throw new ArgumentNullException ("expression");
      }
      
      if(expression.Body.NodeType == ExpressionType.Convert)
      {
        UnaryExpression unary = (UnaryExpression) expression.Body;
        memberExpression = unary.Operand as MemberExpression;
      }
      else if(expression.Body.NodeType == ExpressionType.MemberAccess)
      {
        memberExpression = (MemberExpression) expression.Body;
      }
      
      if(memberExpression == null)
      {
        throw new ArgumentException("The expression is not a MemberExpression");
      }
      
      return memberExpression.Member;
    }
    
    /// <summary>
    /// Gets a <see cref="PropertyInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The property information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static PropertyInfo GetProperty<TObject>(Expression<Func<TObject, object>> expression)
    {
      return GetMember<TObject>(expression) as PropertyInfo;
    }
  }
}

