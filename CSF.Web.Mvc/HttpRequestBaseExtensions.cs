using System;
using System.Web;

namespace CSF.Web.Mvc
{
  /// <summary>
  /// Duplicate of <see cref="CSF.Web.HttpRequestExtensions"/> that works with an <c>HttpResponseBase</c> from
  /// the <c>System.Web.Abstractions</c> assembly, instead of the standard <see cref="HttpRequest"/> type.
  /// </summary>
  public static class HttpRequestBaseExtensions
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
    public static bool IsPost(this HttpRequestBase request)
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
    public static bool IsGet(this HttpRequestBase request)
    {
      if(request == null)
      {
        throw new ArgumentNullException("request");
      }

      return request.HttpMethod == System.Net.WebRequestMethods.Http.Get;
    }
  }
}

