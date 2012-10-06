using System;
using CSF.Entities;
using NHibernate;
using CSF.Patterns.IoC;

namespace CSF.Data.NHibernate
{
  /// <summary>
  /// Implementation of <see cref="IIdentityUnwrappingService"/> that uses an NHibernate ISession to unwrap the
  /// identity.
  /// </summary>
  public class NHibernateIdentityUnwrappingService : IIdentityUnwrappingService
  {
    #region IIdentityUnwrappingService implementation

    /// <summary>
    ///  Unwrap the specified identity returning the entity that the identity represents. 
    /// </summary>
    /// <param name='identity'>
    /// The identity to unwrap.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The expected type of the entity to unwrap.
    /// </typeparam>
    public TEntity Unwrap<TEntity>(IIdentity<TEntity> identity) where TEntity : IEntity
    {
      if(identity == null)
      {
        throw new ArgumentNullException("identity");
      }

      ISession session = ServiceLocator.Get<ISession>();
      return session.Load<TEntity>(identity.Value);
    }

    /// <summary>
    ///  Unwrap the specified identity returning the entity that the identity represents. 
    /// </summary>
    /// <param name='identity'>
    /// The identity to unwrap.
    /// </param>
    public IEntity Unwrap(IIdentity identity)
    {
      if(identity == null)
      {
        throw new ArgumentNullException("identity");
      }

      ISession session = ServiceLocator.Get<ISession>();
      return (IEntity) session.Load(identity.EntityType, identity.Value);
    }

    #endregion
  }
}

