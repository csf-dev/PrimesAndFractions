using System;
using NHibernate;
using CSF.Entities;

namespace CSF.Data.NHibernate
{
  /// <summary>
  /// Extension methods for an NHibernate ISession.
  /// </summary>
  [Obsolete("This type will be removed in a future version, functionality is now available via the INHLinqWrapper")]
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
    [Obsolete("This type will be removed in a future version, functionality is now available via the INHLinqWrapper")]
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
    [Obsolete("This type will be removed in a future version, functionality is now available via the INHLinqWrapper")]
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

    /// <summary>
    /// Attempts to parse an entity identifier and then use that to 'get' a matching entity instance.
    /// </summary>
    /// <returns>
    /// An entity instance, or a null reference if no matching entity could be retrieved.
    /// </returns>
    /// <param name='session'>
    /// An NHibernate ISession
    /// </param>
    /// <param name='identifier'>
    /// The identifier to parse.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of the desired entity.
    /// </typeparam>
    /// <typeparam name='TIdentifier'>
    /// The type of the identifier.
    /// </typeparam>
    [Obsolete("This type will be removed in a future version, functionality is now available via the INHLinqWrapper")]
    public static TEntity GetEntity<TEntity,TIdentifier>(this ISession session, object identifier)
      where TEntity : class,IEntity
    {
      IIdentity<TEntity,TIdentifier> identity;
      TEntity output = null;

      if(Identity.TryParse<TEntity,TIdentifier>(identifier, out identity))
      {
        output = session.GetEntity(identity);
      }

      return output;
    }

    /// <summary>
    /// Attempts to parse an entity identifier and then use that to 'load' a matching entity instance.
    /// </summary>
    /// <returns>
    /// An entity instance, or a null reference if no matching entity could be retrieved.
    /// </returns>
    /// <param name='session'>
    /// An NHibernate ISession
    /// </param>
    /// <param name='identifier'>
    /// The identifier to parse.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of the desired entity.
    /// </typeparam>
    /// <typeparam name='TIdentifier'>
    /// The type of the identifier.
    /// </typeparam>
    [Obsolete("This type will be removed in a future version, functionality is now available via the INHLinqWrapper")]
    public static TEntity LoadEntity<TEntity,TIdentifier>(this ISession session, object identifier)
      where TEntity : class,IEntity
    {
      IIdentity<TEntity,TIdentifier> identity;
      TEntity output = null;

      if(Identity.TryParse<TEntity,TIdentifier>(identifier, out identity))
      {
        output = session.LoadEntity(identity);
      }

      return output;
    }
  }
}

