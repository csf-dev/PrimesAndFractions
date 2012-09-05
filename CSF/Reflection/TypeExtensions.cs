//  
//  TypeExtensions.cs
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
using System.Reflection;

namespace CSF.Reflection
{
  /// <summary>
  /// Helper type containing extension methods for <see cref="System.Type"/>.
  /// </summary>
  public static class TypeExtensions
  {
    #region extension methods
    
    /// <summary>
    /// Gets a collection of <see cref="System.Type"/> that are subclasses of the given <paramref name="type"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method only searches for subclasses within a specific assembly.  This overload looks within the assembly
    /// that defines <paramref name="type"/>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A collection of <see cref="System.Type"/>.
    /// </returns>
    /// <param name='type'>
    /// The type for which to find subclasses.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public static IList<Type> GetSubclasses(this Type type)
    {
      if(type == null)
      {
        throw new ArgumentNullException ("type");
      }
      
      return type.GetSubclasses(Assembly.GetAssembly(type));
    }
    
    /// <summary>
    /// Gets a collection of <see cref="System.Type"/> that are subclasses of the given <paramref name="type"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method only searches for subclasses within a specific assembly.  This overload looks within a given
    /// <paramref name="searchAssembly"/>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A collection of <see cref="System.Type"/>.
    /// </returns>
    /// <param name='type'>
    /// The type for which to find subclasses.
    /// </param>
    /// <param name='searchAssembly'>
    /// The <see cref="Assembly"/> in which to search for subclasses of <paramref name="type"/>.
    /// </param>
    public static IList<Type> GetSubclasses(this Type type, Assembly searchAssembly)
    {
      if(searchAssembly == null)
      {
        throw new ArgumentNullException ("searchAssembly");
      }
      else if(type == null)
      {
        throw new ArgumentNullException ("type");
      }
      
      return searchAssembly.GetTypes().Where(x => x.IsSubclassOf(type)).ToList();
    }

    #endregion
  }
}

