using System;
using System.Net;
using System.Web;

namespace CSF.Web.Mvc
{
  /// <summary>
  /// Duplicate of <see cref="CSF.Web.HttpResponseExtensions"/> that works with an <c>HttpResponseBase</c> from
  /// the <c>System.Web.Abstractions</c> assembly, instead of the standard <see cref="HttpResponse"/> type.
  /// </summary>
  public static class HttpResponseBaseExtensions
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
    public static void Redirect (this HttpResponseBase response, string url, HttpStatusCode statusCode)
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
    public static void Redirect (this HttpResponseBase response, string url, int statusCode)
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

