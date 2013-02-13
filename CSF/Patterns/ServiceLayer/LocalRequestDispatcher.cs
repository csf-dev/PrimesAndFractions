//
//  LocalRequestDispatcher.cs
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

namespace CSF.Patterns.ServiceLayer
{
  /// <summary>
  /// Implementation of <see cref="IRequestDispatcher"/> that is suited to handle local/in-process requests and
  /// responses.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This implementation is best-used when there is no remoting going on and the requests are all coming from the same
  /// process as will be hosting the request dispatcher itself.
  /// </para>
  /// </remarks>
  public class LocalRequestDispatcher : RequestDispatcherBase
  {
    #region properties

    /// <summary>
    /// Gets the registered handlers, indexed by the request types that they handle.
    /// </summary>
    /// <value>
    /// The handlers.
    /// </value>
    protected virtual IDictionary<Type, IRequestHandler> Handlers
    {
      get;
      private set;
    }

    #endregion

    #region implemented abstract members of CSF.Patterns.ServiceLayer.RequestDispatcherBase

    /// <summary>
    ///  Dispatch the specified request and get the response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to dispatch. 
    /// </param>
    public override IResponse Dispatch(IRequest request)
    {
      IResponse output = null;
      Type requestType;

      if(request == null)
      {
        throw new RequestDispatchException();
      }

      requestType = request.GetType();

      if(!this.Handlers.ContainsKey(requestType))
      {
        throw new RequestDispatchException(request);
      }

      try
      {
        output = this.Handlers[requestType].Handle(request);
      }
      catch(Exception ex)
      {
        throw new RequestDispatchException(request, ex);
      }

      if(output == null)
      {
        string message = String.Format("Handler for `{0}' returned a null response.  This is invalid behaviour.",
                                       requestType.FullName);
        throw new RequestDispatchException(request, message);
      }

      return output;
    }

    /// <summary>
    ///  Dispatch the specified request using a request-only mechaism, which will not return a response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to dispatch. 
    /// </param>
    public override void DispatchRequestOnly(IRequest request)
    {
      Type requestType;

      if(request == null)
      {
        throw new RequestDispatchException();
      }

      requestType = request.GetType();

      if(!this.Handlers.ContainsKey(requestType))
      {
        throw new RequestDispatchException(request);
      }

      try
      {
        this.Handlers[requestType].HandleRequestOnly(request);
      }
      catch(Exception ex)
      {
        throw new RequestDispatchException(request, ex);
      }
    }

    /// <summary>
    ///  Determines whether this instance can dispatch the specified request. 
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance can dispatch the specified requestType; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='requestType'>
    /// The request type to test.
    /// </param>
    public override bool CanDispatch(Type requestType)
    {
      return (this.Handlers.ContainsKey(requestType)
              && this.Handlers[requestType] != null);
    }

    /// <summary>
    ///  Gets a read-only indexed collection of the registered request types and the request handlers that requests of
    /// that type would be dispatched to. 
    /// </summary>
    /// <returns>
    ///  The registered handlers. 
    /// </returns>
    public override IDictionary<Type, IRequestHandler> GetRegisteredHandlers()
    {
      IDictionary<Type, IRequestHandler> output = new Dictionary<Type, IRequestHandler>();

      foreach(KeyValuePair<Type, IRequestHandler> registration in this.Handlers)
      {
        output.Add(registration);
      }

      return output;
    }

    /// <summary>
    ///  Registers that the specified handler should be used for requests of the specified type. 
    /// </summary>
    /// <param name='requestType'>
    ///  The type of request that we are registering a handler for. 
    /// </param>
    /// <param name='handler'>
    ///  The handler to use for requests of the given type. 
    /// </param>
    public override IRequestDispatcher Register(Type requestType, IRequestHandler handler)
    {
      if(requestType == null)
      {
        throw new ArgumentNullException("requestType");
      }
      else if(handler == null)
      {
        throw new ArgumentNullException("handler");
      }

      this.Handlers.Add(requestType, handler);
      return this;
    }

    /// <summary>
    ///  Removes a registration for a request handler that would handle the specified type of request. 
    /// </summary>
    /// <param name='requestType'>
    ///  The type of request for which to 'unregister' its handler. 
    /// </param>
    public override IRequestDispatcher Unregister(Type requestType)
    {
      if(requestType == null)
      {
        throw new ArgumentNullException("requestType");
      }

      this.Handlers.Remove(requestType);
      return this;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.LocalRequestDispatcher"/> class.
    /// </summary>
    public LocalRequestDispatcher()
    {
      this.Handlers = new Dictionary<Type, IRequestHandler>();
    }

    #endregion
  }
}

