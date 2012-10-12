//
//  SeeOtherResult.cs
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
using System.Web.Mvc;
using System.Net;

namespace CSF.Web.Mvc
{
  /// <summary>
  /// A form of redirect result that uses the HTTP 'see other' response type.
  /// </summary>
  public class SeeOtherResult : ActionResult
  {
    #region properties

    /// <summary>
    /// Gets or sets the URL that the redirect will target.
    /// </summary>
    /// <value>
    /// The redirct URL.
    /// </value>
    public virtual string Url
    {
      get;
      set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Executes the result - performing a redirect to the <see cref="Url"/> using an HTTP 'see other' response.
    /// </summary>
    /// <param name='context'>
    /// The controller context.
    /// </param>
    public override void ExecuteResult (ControllerContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException ("context");
      }
      if (context.IsChildAction)
      {
        throw new InvalidOperationException ("Cannot redirect in a child action.");
      }

      string url = UrlHelper.GenerateContentUrl(this.Url, context.HttpContext);
      context.Controller.TempData.Keep();
      context.HttpContext.Response.Redirect(url, HttpStatusCode.SeeOther);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Web.Mvc.SeeOtherResult"/> class.
    /// </summary>
    /// <param name='url'>
    /// URL.
    /// </param>
    public SeeOtherResult(string url)
    {
      if(url == null)
      {
        throw new ArgumentNullException("url");
      }

      this.Url = url;
    }

    #endregion
  }
}

