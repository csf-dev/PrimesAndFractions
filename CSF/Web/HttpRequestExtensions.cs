//
//  HttpRequestExtensions.cs
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
using System.Web;

namespace CSF.Web
{
  /// <summary>
  /// Extension methods for <see cref="HttpRequest"/>
  /// </summary>
  public static class HttpRequestExtensions
  {
    /// <summary>
    /// Determines whether this request represents an HTTP POST request.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is an HTTP POST; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='request'>
    /// The HttpRequest to inspect.
    /// </param>
    public static bool IsPost(this HttpRequest request)
    {
      if(request == null)
      {
        throw new ArgumentNullException("request");
      }

      return request.HttpMethod == System.Net.WebRequestMethods.Http.Post;
    }

    /// <summary>
    /// Determines whether this request represents an HTTP GET request.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is an HTTP GET; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='request'>
    /// The HttpRequest to inspect.
    /// </param>
    public static bool IsGet(this HttpRequest request)
    {
      if(request == null)
      {
        throw new ArgumentNullException("request");
      }

      return request.HttpMethod == System.Net.WebRequestMethods.Http.Get;
    }
  }
}

