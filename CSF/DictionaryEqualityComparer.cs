//
//  DictionaryEqualityComparer.cs
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

namespace CSF
{
  /// <summary>
  /// Provides an <see cref="IEqualityComparer"/> designed to compare two objects that implement the generic
  /// <c>IDictionary</c> interface.
  /// </summary>
  public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer
  {
    #region methods

    /// <summary>
    /// Tests for equality between the two objects (which should both be dictionaries) of the appropriate generic types.
    /// </summary>
    /// <param name='obj1'>
    /// A dictionary instance
    /// </param>
    /// <param name='obj2'>
    /// A dictionary instance
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
          IDictionary<TKey, TValue>
            dictionary1 = (IDictionary<TKey, TValue>) obj1,
            dictionary2 = (IDictionary<TKey, TValue>) obj2;

          output = (dictionary1.Count == dictionary2.Count);

          if(output)
          {
            foreach(TKey key in dictionary1.Keys)
            {
              if(!dictionary2.ContainsKey(key)
                 || !dictionary2[key].Equals(dictionary1[key]))
              {
                output = false;
                break;
              }
            }
          }
        }
        catch(InvalidCastException)
        {
          output = obj1.Equals(obj2);
        }
      }

      return output;
    }

    /// <summary>
    /// Unsupported method, would get the hash code of a DateTime.
    /// </summary>
    /// <param name='obj'>
    /// An object
    /// </param>
    public int GetHashCode(object obj)
    {
      throw new NotImplementedException("This method is unsupported");
    }

    #endregion

    #region explicit interface implementation

    bool IEqualityComparer.Equals(object obj1, object obj2)
    {
      return this.AreEqual(obj1, obj2);
    }

    #endregion
  }
}

