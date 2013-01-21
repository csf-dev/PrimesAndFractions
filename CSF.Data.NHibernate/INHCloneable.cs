using System;
using CSF.Entities;
using NHibernate;

namespace CSF.Data.NHibernate
{
  /// <summary>
  /// Interface describing a persistent entity that may be cloned via an NHibernate <c>ISession</c>.
  /// </summary>
  public interface INHCloneable<TClone> where TClone : IEntity
  {
    /// <summary>
    /// Clones the current instance making use of an NHibernate <c>ISession</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method takes a 'detached' object instance (not connected to any kind of ISession) and creates a clone using
    /// the given <paramref name="session"/> to properly configure referenced entities and the like.
    /// </para>
    /// <para>
    /// The implementation of this method is defined as:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// If <see cref="IEntity.HasIdentity"/> is true for the identity, then the cloning must begin by retrievng it from
    /// the session.  If the <paramref name="loadOnly"/> parameter is set to <c>true</c> then
    /// <c>session.Load&lt;&gt;()</c> must be used for this, otherwise <c>session.Get&lt;&gt;()</c> must be used.
    /// </item>
    /// <item>
    /// If the entity does not have an identity then a constructor of factory method should be used to construct a new
    /// instance of the entity.
    /// </item>
    /// <item>
    /// If either the entity did not have an identity OR the <paramref name="loadOnly"/> parameter was set to
    /// <c>false</c>, then each member of the entity should then be copied to the clone.
    /// </item>
    /// <item>
    /// When copying contained members, any members that reference other <c>INHCloneable</c> entities must be cloned
    /// using their own <see cref="NHClone"/> methods, with the <paramref name="loadOnly"/> parameter set to
    /// <c>true</c>.
    /// </item>
    /// </list>
    /// </remarks>
    /// <returns>
    /// A cloned copy of the current instance, suitable for interacting with the NHibernate persistance framework.
    /// </returns>
    /// <param name='session'>
    /// An NHibernate <c>ISession</c>.
    /// </param>
    /// <param name='loadOnly'>
    /// A value that indicates whether this method should only load a minimal reference to the entity (if possible).
    /// See the remarks for more information.
    /// </param>
    TClone NHClone(ISession session, bool loadOnly);
  }
}

