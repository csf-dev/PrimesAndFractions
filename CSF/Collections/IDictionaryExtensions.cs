//
//  IDictionaryExtensions.cs
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
using System.Collections.Specialized;
using System.Collections.Generic;

namespace CSF.Collections
{
  /// <summary>
  /// Extension methods for dictionary types.
  /// </summary>
  public static class IDictionaryExtensions
  {
    /// <summary>
    /// Converts a dictionary of strings (string keys and string values) into a <c>NameValueCollection</c> containing
    /// the same data.
    /// </summary>
    /// <returns>
    /// The name value collection.
    /// </returns>
    /// <param name='dictionary'>
    /// The dictionary to convert.
    /// </param>
    public static NameValueCollection ToNameValueCollection(this IDictionary<string,string> dictionary)
    {
      if(dictionary == null)
      {
        throw new ArgumentNullException("dictionary");
      }

      NameValueCollection output = new NameValueCollection();

      foreach(string key in dictionary.Keys)
      {
        output.Add(key, dictionary[key]);
      }

      return output;
    }
  }
}

