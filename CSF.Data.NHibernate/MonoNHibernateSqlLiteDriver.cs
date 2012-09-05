using System;
using NHibernate.Driver;
using Mono.Data.Sqlite;

namespace CSF.Data.NHibernate
{
  /// <summary>
  /// Exposes a <see cref="ReflectionBasedDriver"/> for Sqlite database connections, making use of the
  /// <c>Mono.Data.Sqlite</c> namespace, instead of the default Sqlite driver namespace.
  /// </summary>
  /// <remarks>
  /// This driver would be used when using the Mono framework with its native/built-in Sqlite libraries, instead of
  /// using .NET and the separate Sqlite driver.
  /// </remarks>
  public class MonoNHibernateSqlLiteDriver : ReflectionBasedDriver
  {
    #region constants

    private static readonly Type
      CONNECTION_TYPE                   = typeof(SqliteConnection),
      COMMAND_TYPE                      = typeof(SqliteCommand);

    #endregion

    #region properties

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Data.NHibernate.MonoNHibernateSqlLiteDriver"/> uses named
    /// prefixes in parameters.
    /// </summary>
    /// <value>
    /// <c>true</c> if this driver uses named prefix in parameters; otherwise, <c>false</c>.
    /// </value>
    public override bool UseNamedPrefixInParameter
    {
      get { return true; }
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Data.NHibernate.MonoNHibernateSqlLiteDriver"/> uses named
    /// prefixes in sql.
    /// </summary>
    /// <value>
    /// <c>true</c> if this driver uses named prefixes in sql; otherwise, <c>false</c>.
    /// </value>
    public override bool UseNamedPrefixInSql
    {
      get { return true; }
    }

    /// <summary>
    /// Gets the named parameter prefix.
    /// </summary>
    /// <value>
    /// The named parameter prefix.
    /// </value>
    public override string NamedPrefix
    {
      get { return "@"; }
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Data.NHibernate.MonoNHibernateSqlLiteDriver"/> supports
    /// multiple open readers.
    /// </summary>
    /// <value>
    /// <c>true</c> if this driver supports multiple open readers; otherwise, <c>false</c>.
    /// </value>
    public override bool SupportsMultipleOpenReaders
    {
      get { return false; }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Data.NHibernate.MonoNHibernateSqlLiteDriver"/> class.
    /// </summary>
    public MonoNHibernateSqlLiteDriver () : base(CONNECTION_TYPE.Namespace,
                                                 CONNECTION_TYPE.Namespace,
                                                 CONNECTION_TYPE.FullName,
                                                 COMMAND_TYPE.FullName)
    {
    }

    #endregion
  }
}

