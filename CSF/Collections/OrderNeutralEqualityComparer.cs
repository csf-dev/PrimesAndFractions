//
//  OrderNeutralEqualityComparer.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Collections
{
  /// <summary>
  /// Determines whether two enumerations containing a given type are equal.  The order of items in the collections
  /// does not matter for this comparison.
  /// </summary>
  public class OrderNeutralEqualityComparer<TCollection> : IEqualityComparer
  {
    #region methods

    /// <summary>
    /// Determines whether two objects are equal, assuming that they are enumerable collections, ignoring the order of
    /// elements.
    /// </summary>
    /// <returns>
    /// Whether or not the objects are equal.
    /// </returns>
    /// <param name='obj1'>
    /// The first object to compare.
    /// </param>
    /// <param name='obj2'>
    /// The second object to compare.
    /// </param>
    public bool AreEqual(object obj1, object obj2)
    {
      bool output;

      if(Object.ReferenceEquals(obj1, obj2))
      {
        output = true;
      }
      else if(obj1 == null)
      {
        output = false;
      }
      else
      {
        try
        {
          IEnumerable<TCollection>
            enumerable1 = (IEnumerable<TCollection>) obj1,
            enumerable2 = (IEnumerable<TCollection>) obj2;

          output = (enumerable1.Count() == enumerable2.Count()
                    && enumerable1.Except(enumerable2).Count() == 0);
        }
        catch(InvalidCastException)
        {
          output = obj1.Equals(obj2);
        }
      }

      return output;
    }

    /// <summary>
    /// Unsupported method would get the hash code for an object.
    /// </summary>
    /// <param name='obj1'>
    /// Obj1.
    /// </param>
    public int GetHashCode(object obj1)
    {
      throw new NotSupportedException();
    }

    #endregion

    #region IEqualityComparer implementation

    bool IEqualityComparer.Equals(object obj1, object obj2)
    {
      return this.AreEqual(obj1, obj2);
    }

    #endregion
  }
}

