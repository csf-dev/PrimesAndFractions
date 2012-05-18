//  
//  ConnectionStringSettingsExtensions.cs
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
using System.Data;
using System.Configuration;
using System.Data.Common;

namespace CSF.Configuration
{
  /// <summary>
  /// Extension methods for the <see cref="ConnectionStringSettings"/> type.
  /// </summary>
  public static class ConnectionStringSettingsExtensions
  {
    /// <summary>
    /// Opens the connection.
    /// </summary>
    /// <returns>
    /// The connection.
    /// </returns>
    /// <param name='settings'>
    /// Settings.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown if <paramref name="settings"/> is null.
    /// </exception>
    /// <exception cref="ConfigurationErrorsException">
    /// If <paramref name="settings"/> refers to a provider invariant name that is not registered with the .NET
    /// framework.
    /// </exception>
    public static IDbConnection CreateAndOpenConnection(this ConnectionStringSettings settings)
    {
      IDbConnection output;
      DbProviderFactory factory;
      
      if(settings == null)
      {
        throw new ArgumentNullException ("settings");
      }
      
      factory = DbProviderFactories.GetFactory(settings.ProviderName);
      output = factory.CreateConnection();
      output.ConnectionString = settings.ConnectionString;
      output.Open();
      
      return output;      
    }
  }
}

