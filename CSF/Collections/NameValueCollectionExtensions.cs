//
//  NameValueCollectionExtensions.cs
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
using System.Collections.Specialized;

namespace CSF.Collections
{
  /// <summary>
  /// Extension methods to <see cref="System.Collections.Specialized.NameValueCollection"/>
  /// </summary>
  public static class NameValueCollectionExtensions
  {
    #region methods

    /// <summary>
    /// Returns a clone (deep copy) of the <see cref="NameValueCollection"/> as an <c>IDictionary</c> of strings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The new/output collection is not synchronised with the source collection.  If either is altered after this
    /// method has been called, the other is left unchanged.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A dictionary of strings with the same content as the source collection.
    /// </returns>
    /// <param name='source'>
    /// A <see cref="NameValueCollection"/>.
    /// </param>
    public static IDictionary<string,string> ToDictionary(this NameValueCollection source)
    {
      IDictionary<string,string> output = new Dictionary<string, string>();

      if(source == null)
      {
        throw new ArgumentNullException("source");
      }

      foreach(string key in source.Keys)
      {
        output.Add(key, source[key]);
      }

      return output;
    }

    #endregion
  }
}

