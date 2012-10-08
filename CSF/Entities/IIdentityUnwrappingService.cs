//
//  IIdentityUnwrappingService.cs
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

namespace CSF.Entities
{
  /// <summary>
  /// Interface for a service that is able to 'unwrap' an <see cref="IIdentity"/> into its original entity, using some
  /// kind of retreival mechanism.
  /// </summary>
  public interface IIdentityUnwrappingService
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
    TEntity Unwrap<TEntity>(IIdentity identity) where TEntity : IEntity;

    /// <summary>
    /// Unwrap the specified identity returning the entity that the identity represents.
    /// </summary>
    /// <param name='identity'>
    /// An <see cref="IIdentity"/> instance.
    /// </param>
    object Unwrap(IIdentity identity);
  }
}

