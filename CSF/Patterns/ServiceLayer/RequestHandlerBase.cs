//
//  RequestHandlerBase.cs
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

namespace CSF.Patterns.ServiceLayer
{
  /// <summary>
  /// Abstract generic base class for a strongly-typed <see cref="IRequestHandler"/>.
  /// </summary>
  public abstract class RequestHandlerBase<TRequest,TResponse> : IRequestHandler<TRequest,TResponse>, IRequestHandler
    where TRequest : IRequest
    where TResponse : IResponse
  {
    #region non-generic IRequestHandler implementation

    IResponse IRequestHandler.Handle (IRequest request)
    {
      return this.Handle((TRequest) request);
    }

    void IRequestHandler.HandleRequestOnly (IRequest request)
    {
      this.HandleRequestOnly((TRequest) request);
    }

    #endregion

    #region generic IRequestHandler implementation

    /// <summary>
    ///  Handles a standard/typical request, in which a response is returned to the caller. 
    /// </summary>
    /// <param name='request'>
    ///  The request to handle. 
    /// </param>
    public virtual TResponse Handle (TRequest request)
    {
      throw new NotSupportedException("This handler type does not include an implementation for 'Handle'.");
    }

    /// <summary>
    ///  Handles a one-way/fire-and-forget request. This method does not return any kind of response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to handle. 
    /// </param>
    public virtual void HandleRequestOnly (TRequest request)
    {
      throw new NotSupportedException("This handler type does not include an implementation for 'HandleRequestOnly'.");
    }

    #endregion
  }
}

