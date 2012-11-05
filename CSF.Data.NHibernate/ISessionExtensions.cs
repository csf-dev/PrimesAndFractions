using System;
using NHibernate;
using CSF.Entities;

namespace CSF.Data.NHibernate
{
  /// <summary>
  /// Extension methods for an NHibernate ISession.
  /// </summary>
  public static class ISessionExtensions
  {
    /// <summary>
    /// Convenience method, similar to <c>ISession.Get</c> that specifically gets entity-based types.
    /// </summary>
    /// <returns>
    /// An entity instance, or a null reference.
    /// </returns>
    /// <param name='session'>
    /// An NHibernate ISession instance.
    /// </param>
    /// <param name='identity'>
    /// An identity that refers to a specific entity type.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of entity that will be returned by this method.
    /// </typeparam>
    public static TEntity GetEntity<TEntity>(this ISession session, IIdentity<TEntity> identity)
      where TEntity : IEntity
    {
      if(session == null)
      {
        throw new ArgumentNullException("session");
      }
      else if(identity == null)
      {
        throw new ArgumentNullException("identity");
      }

      return session.Get<TEntity>(identity.Value);
    }

    /// <summary>
    /// Convenience method, similar to <c>ISession.Load</c> that specifically gets entity-based types.
    /// </summary>
    /// <returns>
    /// An entity instance, or a proxy instance that refers to that entity.
    /// </returns>
    /// <param name='session'>
    /// An NHibernate ISession instance.
    /// </param>
    /// <param name='identity'>
    /// An identity that refers to a specific entity type.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of entity that will be returned by this method.
    /// </typeparam>
    public static TEntity LoadEntity<TEntity>(this ISession session, IIdentity<TEntity> identity)
      where TEntity : IEntity
    {
      if(session == null)
      {
        throw new ArgumentNullException("session");
      }
      else if(identity == null)
      {
        throw new ArgumentNullException("identity");
      }

      return session.Load<TEntity>(identity.Value);
    }
  }
}

