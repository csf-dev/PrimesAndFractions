//  
//  IServiceInstanceCache.cs
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

namespace CSF.Patterns.IoC
{
  /// <summary>
  /// Interface for a caching module to hold instances of services.
  /// </summary>
  public interface IServiceInstanceCache : IDisposable
  {
    #region methods
    
    /// <summary>
    /// Add the specified service implementation to this cache.
    /// </summary>
    /// <typeparam name="TService">
    /// The type of the service to add.
    /// </typeparam>
    /// <param name='serviceInstance'>
    /// The service implementation instance.
    /// </param>
    void Add<TService>(TService serviceInstance) where TService : class;
    
    /// <summary>
    /// Remove the specified service implementation from the cache.
    /// </summary>
    /// <typeparam name="TService">
    /// The type of the service to remove.
    /// </typeparam>
    void Remove<TService>() where TService : class;
    
    /// <summary>
    /// Determines wheter this cache instance contains a service implementation instance for the given interface.
    /// </summary>
    /// <typeparam name="TService">
    /// The type of the service to check for.
    /// </typeparam>
    bool Contains<TService>() where TService : class;
    
    /// <summary>
    /// Gets a contained service implementation.
    /// </summary>
    /// <typeparam name='TService'>
    /// The type of the service to retrieve.
    /// </typeparam>
    TService Get<TService>() where TService : class;
    
    #endregion
  }
}

