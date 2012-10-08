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
  /// Extension methods for the <see cref="IIdentity"/> type.
  /// </summary>
  public static class IIdentityExtensions
  {
    /// <summary>
    /// Unwrap the specified identity returning the entity that the identity represents.
    /// </summary>
    /// <param name='identity'>
    /// An <see cref="IIdentity"/> instance.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of entity expected by the unwrapping action.
    /// </typeparam>
    public static TEntity Unwrap<TEntity>(this IIdentity identity) where TEntity : IEntity
    {
      IIdentityUnwrappingService service = ServiceLocator.Get<IIdentityUnwrappingService>();
      return service.Unwrap<TEntity>(identity);
    }

    /// <summary>
    /// Unwrap the specified identity returning the entity that the identity represents.
    /// </summary>
    /// <param name='identity'>
    /// An <see cref="IIdentity"/> instance.
    /// </param>
    public static object Unwrap(this IIdentity identity)
    {
      IIdentityUnwrappingService service = ServiceLocator.Get<IIdentityUnwrappingService>();
      return service.Unwrap(identity);
    }
  }
}

