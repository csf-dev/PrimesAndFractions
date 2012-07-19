//  
//  HttpResponseExtensions.cs
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
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace CSF.Web
{
  /// <summary>
  /// Extension methods for <see cref="HttpResponse"/>.
  /// </summary>
  public static class HttpResponseExtensions
  {
    #region extension methods
    
    /// <summary>
    /// Redirect the response to a given url, using the given HTTP status code.
    /// </summary>
    /// <param name='response'>
    /// The response to redirect
    /// </param>
    /// <param name='url'>
    /// The URL to redirect to
    /// </param>
    /// <param name='statusCode'>
    /// The HTTP status code to use.
    /// </param>
    public static void Redirect (this HttpResponse response, string url, HttpStatusCode statusCode)
    {
      response.Redirect(url, (int) statusCode);
    }
    
    /// <summary>
    /// Redirect the response to a given url, using the given HTTP status code.
    /// </summary>
    /// <param name='response'>
    /// The response to redirect
    /// </param>
    /// <param name='url'>
    /// The URL to redirect to
    /// </param>
    /// <param name='statusCode'>
    /// The HTTP status code number to use.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public static void Redirect (this HttpResponse response, string url, int statusCode)
    {
      if (response == null)
      {
        throw new ArgumentNullException ("response");
      }
      else if (url == null)
      {
        throw new ArgumentNullException ("redirectUrl");
      }
      
      url = response.ApplyAppPathModifier (url);
      
      response.ClearHeaders();
      response.ClearContent();
      response.StatusCode = statusCode;
      response.RedirectLocation = url;
      
      response.Write(String.Format(@"<html><head><title>{0}</title></head>
<body>
  <h1>{0}</h1>
  <p>You are being redirected to <a href=""{1}"">{1}</a>.</p>
</body>
</html>",
                                   GetRedirectDescription(statusCode),
                                   url));
      
      response.End();
    }
    
    #endregion
    
    #region private methods
    
    private static string GetRedirectDescription(int statusCode)
    {
      string output;
      
      switch(statusCode)
      {
      case 301:
        output = "Moved Permanently";
        break;
      case 302:
        output = "Found";
        break;
      case 303:
        output = "See Other";
        break;
      default:
        output = "Redirection";
        break;
      }
      
      return output;
    }
    
    #endregion
  }
}

