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
  public static class ReflectionHelper
  {
    /// <summary>
    /// Gets a property from a type by using a lambda expression.
    /// </summary>
    /// <returns>
    /// The property.
    /// </returns>
    /// <param name='expression'>
    /// A lambda expression referring to the property.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type of the object that contains the property.
    /// </typeparam>
    public static PropertyInfo GetProperty<TObject>(Expression<Func<TObject, object>> expression)
    {
      Expression body = expression;
      
      while(body is LambdaExpression)
      {
        body = ((LambdaExpression) body).Body;
      }
      
      if(body.NodeType != ExpressionType.MemberAccess)
      {
        throw new ArgumentException("Expression does not represent a property node.");
      }
      
      return (PropertyInfo) (((MemberExpression) body).Member);
    }
  }
}

