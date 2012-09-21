using System;
using FluentNHibernate.Cfg.Db;
using CSF.Reflection;

namespace CSF.Data.NHibernate
{
  /// <summary>
  /// Extension methods to the SQLiteConfiguration NHibernate type.
  /// </summary>
  public static class SQLiteConfigurationExtensions
  {
    /// <summary>
    /// Convenience method to select the 'correct' driver for SQLite databases.
    /// </summary>
    /// <remarks>
    /// The rationale for this method is that - when running on the Mono framework instead of the .NET framework - we
    /// will generally prefer to use Mono's builtin driver for SQLite, instead of using a locally-installed SQLite
    /// driver.  This method will use <see cref="StaticReflectionUtility.IsUsingMonoFramework"/> and, if we are on Mono,
    /// substitute the default driver that NHibernate would select with Mono's builtin.  If we are not running on Mono
    /// then this method does nothing.
    /// </remarks>
    /// <returns>
    /// The correct driver for the platform.
    /// </returns>
    /// <param name='config'>
    /// A <see cref="SQLiteConfiguration"/> instance.
    /// </param>
    public static SQLiteConfiguration SelectDriver(this SQLiteConfiguration config)
    {
      if(config == null)
      {
        throw new ArgumentNullException("config");
      }

      return StaticReflectionUtility.IsUsingMonoFramework()? config.Driver<MonoNHibernateSqlLiteDriver>() : config;
    }
  }
}

