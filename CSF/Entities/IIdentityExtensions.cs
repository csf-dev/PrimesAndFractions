//
//  IIdentityExtensions.cs
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
using CSF.Patterns.IoC;

namespace CSF.Entities
{
  /// <summary>
  /// Extension methods for types that implement <see cref="IIdentity"/> (and its related generic interfaces).
  /// </summary>
  public static class IIdentityExtensions
  {
    /// <summary>
    /// Unwraps the current identity instance returning the entity that the identity represents.  This method uses the
    /// <see cref="ServiceLocator"/> to find a default implementation of <see cref="IIdentityUnwrappingService"/>.
    /// </summary>
    /// <param name='identity'>
    /// The identity instance to unwrap.
    /// </param>
    public static IEntity Unwrap(this IIdentity identity)
    {
      IIdentityUnwrappingService service = ServiceLocator.Get<IIdentityUnwrappingService>();
      return identity.Unwrap(service);
    }

    /// <summary>
    /// Unwraps the current identity instance returning the entity that the identity represents.  This method uses the
    /// <see cref="ServiceLocator"/> to find a default implementation of <see cref="IIdentityUnwrappingService"/>.
    /// </summary>
    /// <param name='identity'>
    /// The identity instance to unwrap.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of <see cref="IEntity"/> expected.
    /// </typeparam>
    public static TEntity Unwrap<TEntity>(this IIdentity<TEntity> identity) where TEntity : IEntity
    {
      IIdentityUnwrappingService service = ServiceLocator.Get<IIdentityUnwrappingService>();
      return identity.Unwrap(service);
    }

    /// <summary>
    /// Unwraps the current identity instance returning the entity that the identity represents.
    /// </summary>
    /// <param name='identity'>
    /// The identity instance to unwrap.
    /// </param>
    /// <param name='service'>
    /// The implementation of <see cref="IIdentityUnwrappingService"/> to use for the unwrapping.
    /// </param>
    public static IEntity Unwrap(this IIdentity identity, IIdentityUnwrappingService service)
    {
      if(service == null)
      {
        throw new ArgumentNullException("service");
      }
      else if(identity == null)
      {
        throw new ArgumentNullException("identity");
      }

      return service.Unwrap(identity);
    }

    /// <summary>
    /// Unwraps the current identity instance returning the entity that the identity represents.
    /// </summary>
    /// <param name='identity'>
    /// The identity instance to unwrap.
    /// </param>
    /// <param name='service'>
    /// The implementation of <see cref="IIdentityUnwrappingService"/> to use for the unwrapping.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of <see cref="IEntity"/> expected.
    /// </typeparam>
    public static TEntity Unwrap<TEntity>(this IIdentity<TEntity> identity, IIdentityUnwrappingService service)
                                          where TEntity : IEntity
    {
      if(service == null)
      {
        throw new ArgumentNullException("service");
      }
      else if(identity == null)
      {
        throw new ArgumentNullException("identity");
      }

      return service.Unwrap(identity);
    }
  }
}

