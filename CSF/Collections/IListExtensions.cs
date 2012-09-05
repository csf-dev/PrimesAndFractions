//
//  IListExtensions.cs
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

namespace CSF.Collections
{
  /// <summary>
  /// Extension methods to the generic <c>IList</c> type.
  /// </summary>
  public static class IListExtensions
  {
    /// <summary>
    /// Creates a wrapped copy of the <paramref name="sourceList"/> and associates a pair of actions with it that are
    /// executed upon items before they are added/removed to/from the list.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The wrapped copy of the list, with the new behaviour added.
    /// </returns>
    /// <param name='sourceList'>
    /// The source <c>IList</c>, which is unmodified in the process.
    /// </param>
    /// <param name='beforeAdd'>
    /// An action to perform upon an item before it is added to the list.
    /// </param>
    /// <param name='beforeRemove'>
    /// An action to perform upon an item before it is removed from the list.
    /// </param>
    /// <typeparam name='T'>
    /// The type of object contained within the list.
    /// </typeparam>
    public static IEventBoundList<T> WrapWithBeforeActions<T>(this IList<T> sourceList,
                                                              Action<T> beforeAdd,
                                                              Action<T> beforeRemove) where T : class
    {
      IEventBoundList<T> output = (sourceList as IEventBoundList<T>)?? new EventBoundListWrapper<T>(sourceList);

      output.BeforeAdd = (list, item) => {
        beforeAdd(item);
        return true;
      };
      output.BeforeRemove = (list, item) => {
        beforeRemove(item);
        return true;
      };

      return output;
    }

    /// <summary>
    /// Creates a wrapped copy of the <paramref name="sourceList"/> and associates a pair of functions with it that are
    /// executed upon items before they are added/removed to/from the list.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The wrapped copy of the list, with the new behaviour added.
    /// </returns>
    /// <param name='sourceList'>
    /// The source <c>IList</c>, which is unmodified in the process.
    /// </param>
    /// <param name='beforeAdd'>
    /// A function to execute before an item is added to the list.  If the function returns false then addition is
    /// aborted.
    /// </param>
    /// <param name='beforeRemove'>
    /// A function to execute before an item  is removed from the list.  If the function returns false then addition is
    /// aborted.
    /// </param>
    /// <typeparam name='T'>
    /// The type of object contained within the list.
    /// </typeparam>
    public static IEventBoundList<T> WrapWithBeforeActions<T>(this IList<T> sourceList,
                                                              Func<IList<T>, T, bool> beforeAdd,
                                                              Func<IList<T>, T, bool> beforeRemove) where T : class
    {
      IEventBoundList<T> output = (sourceList as IEventBoundList<T>)?? new EventBoundListWrapper<T>(sourceList);

      output.BeforeAdd = beforeAdd;
      output.BeforeRemove = beforeRemove;

      return output;
    }
  }
}

