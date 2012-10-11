//  
//  Identity.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 CSF Software Limited
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
  /// <para>
  /// A static/helper class that creates <see cref="IIdentity"/> instances from <see cref="IEntity"/> types.
  /// </para>
  /// </summary>
  public static class Identity
  {
    #region static factory method
    
    /// <summary>
    /// <para>Static convenience method creates a new <see cref="IIdentity"/> for a specified type.</para>
    /// </summary>
    /// <param name="identifier">
    /// An identifier value.
    /// </param>
    /// <typeparam name='TIdentifier'>
    /// The type of the identifier.
    /// </typeparam>
    /// <typeparam name='TEntity'>
    /// The type of the entity that the generated identity relates to.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IIdentity"/>
    /// </returns>
    public static IIdentity<TEntity,TIdentifier> Create<TEntity,TIdentifier>(TIdentifier identifier) where TEntity : IEntity
    {
      return new Identity<TEntity,TIdentifier>(identifier);
    }

    /// <summary>
    /// <para>Static convenience method creates a new <see cref="IIdentity"/> for a given entity type.</para>
    /// </summary>
    /// <param name='identifier'>
    /// An identifier value.
    /// </param>
    /// <param name='entityType'>
    /// The type of the entity that the generated identity relates to.
    /// </param>
    /// <typeparam name='TIdentifier'>
    /// The type of the identifier.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IIdentity"/>
    /// </returns>
    public static IIdentity Create<TIdentifier>(TIdentifier identifier, Type entityType)
    {
      return (IIdentity) typeof(Identity<,>)
                           .MakeGenericType(entityType, typeof(TIdentifier))
                           .GetConstructor(new Type[] { typeof(TIdentifier) })
                           .Invoke(new object[] { identifier });
    }

    /// <summary>
    /// Convenience method creates an identity instance and then immediately unwraps it, yielding an entity.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for the entity.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The entity type expected.
    /// </typeparam>
    public static TEntity Unwrap<TEntity>(object identifier) where TEntity : IEntity
    {
      IIdentityUnwrappingService service = ServiceLocator.Get<IIdentityUnwrappingService>();
      IIdentity<TEntity> identity = new Identity<TEntity,object>(identifier);

      return service.Unwrap<TEntity>(identity);
    }

    /// <summary>
    /// Convenience method creates an identity instance and then immediately unwraps it, yielding an entity.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for the entity.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The entity type expected.
    /// </typeparam>
    /// <typeparam name='TIdentifier'>
    /// The type of the identifier, usually unneeded.
    /// </typeparam>
    public static TEntity Unwrap<TEntity,TIdentifier>(TIdentifier identifier) where TEntity : IEntity
    {
      IIdentityUnwrappingService service = ServiceLocator.Get<IIdentityUnwrappingService>();
      IIdentity<TEntity> identity = new Identity<TEntity,TIdentifier>(identifier);

      return service.Unwrap<TEntity>(identity);
    }

    #endregion
  }
}

