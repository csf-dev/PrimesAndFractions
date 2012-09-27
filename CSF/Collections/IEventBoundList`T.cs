//
//  IEventWrappedList.cs
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
using System.Collections;

namespace CSF.Collections
{
  /// <summary>
  /// Interface for a list/collection type that additionally has associated events/actions perform before/after item
  /// addition/removal (and that can prevent item addition/removal).
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is very heavily based on the excellent work found at:
  /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
  /// </para>
  /// <para>
  /// An important explanation as to the rationale for the <c>class</c> constraint on the generic type of the list
  /// items:  This is entirely because the contained items within the list must be reference types and not value types.
  /// If the list items were value types then the actions (before and after) would not be able to modify these instances
  /// in their own method bodies.
  /// </para>
  /// <example>
  /// For example:  A list is configured with a <see cref="BeforeAdd"/> handler which sets a property on the item that
  /// is to be added to the list.  However, if this item were a value type then it would be passed in as a value - the
  /// property would be set but only on that 'copy' of the value passed into the anonymous method.  The original value
  /// would remain unchanged and the entire purpose of the action will have been lost.
  /// </example>
  /// </remarks>
  public interface IEventBoundList<T> : IList<T>, ICollection where T : class
  {
    // See the remarks for this type for an important rationale discussion for the generic constraint 'class'.

    /// <summary>
    /// Gets or sets the action to perform after an item is added to this collection.
    /// </summary>
    /// <value>
    /// The action to perform after item addition.
    /// </value>
    Action<IList<T>> AfterAdd { get; set; }

    /// <summary>
    /// Gets or sets the action to perform after an item is removed from this collection.
    /// </summary>
    /// <value>
    /// The action to perform after item removal.
    /// </value>
    Action<IList<T>> AfterRemove { get; set; }

    /// <summary>
    /// Gets or sets a function to execute before an item is added to this collection.  If the return value of that
    /// function is false then adding the item is aborted.
    /// </summary>
    /// <value>
    /// The function to execute before item addition.
    /// </value>
    Func<IList<T>, T, bool> BeforeAdd { get; set; }

    /// <summary>
    /// Gets or sets a function to execute before an item is removed from this collection.  If the return value of that
    /// function is false then removing the item is aborted.
    /// </summary>
    /// <value>
    /// The function to execute before item removal.
    /// </value>
    Func<IList<T>, T, bool> BeforeRemove { get; set; }

    /// <summary>
    /// Detaches the specified item from the list as if it had been removed but does not actually remove it from the
    /// underlying list.
    /// </summary>
    /// <param name='item'>
    /// The item to detach
    /// </param>
    void Detach(T item);

    /// <summary>
    /// Detaches the item at the specified index from the list as if it had been removed but does not actually remove
    /// it from the underlying list.
    /// </summary>
    /// <param name='index'>
    /// The numeric index of the item to detach
    /// </param>
    void DetachAt(int index);

    /// <summary>
    /// Detaches all contained items from the list as if they had been removed, without actually removing
    /// them from the underlying list.
    /// </summary>
    void DetachAll();
  }
}

