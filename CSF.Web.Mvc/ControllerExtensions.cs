using System;
using System.Web.Mvc;

namespace CSF.Web.Mvc
{
  /// <summary>
  /// Extension methods for an MVC controller.
  /// </summary>
  public static class ControllerExtensions
  {
    /// <summary>
    /// Creates a new <see cref="SeeOtherResult"/> from a URL.
    /// </summary>
    /// <returns>
    /// A new <see cref="SeeOtherResult"/>.
    /// </returns>
    /// <param name='controller'>
    /// The controller from which to create the result.
    /// </param>
    /// <param name='url'>
    /// The URL to redirect to.
    /// </param>
    public static SeeOtherResult SeeOther(this Controller controller, string url)
    {
      if(url == null)
      {
        throw new ArgumentNullException("url");
      }

      return new SeeOtherResult(url);
    }
  }
}

