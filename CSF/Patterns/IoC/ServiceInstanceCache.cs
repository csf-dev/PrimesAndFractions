//  
//  ServiceInstanceCache.cs
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
using System.Collections.Generic;

namespace CSF.Patterns.IoC
{
  /// <summary>
  /// A cache for service instances.
  /// </summary>
  [Serializable]
  public class ServiceInstanceCache : IServiceInstanceCache
  {
    #region fields
    
    private Dictionary<Type, object> _underlyingCache;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets the underlying cache of service instances.
    /// </summary>
    /// <value>
    /// The underlying cache.
    /// </value>
    protected Dictionary<Type, object> UnderlyingCache
    {
      get {
        return _underlyingCache;
      }
    }
    
    #endregion

    #region IServiceInstanceCache implementation
    
    /// <summary>
    /// Add the specified service implementation to this cache.
    /// </summary>
    /// <typeparam name="TService">
    /// The type of the service to add.
    /// </typeparam>
    /// <param name='serviceInstance'>
    /// The service implementation instance.
    /// </param>
    public void Add<TService>(TService serviceInstance) where TService : class
    {
      if(serviceInstance == null)
      {
        throw new ArgumentNullException("serviceInstance");
      }
      
      this.UnderlyingCache[typeof(TService)] = serviceInstance;
    }

    /// <summary>
    /// Remove the specified service implementation from the cache.
    /// </summary>
    /// <typeparam name="TService">
    /// The type of the service to remove.
    /// </typeparam>
    public void Remove<TService>() where TService : class
    {
      Type serviceType = typeof(TService);

      // If the cached service instance implements IDisposable then properly dispose of it before removing it
      if(this.UnderlyingCache.ContainsKey(serviceType))
      {
        IDisposable disposableCachedService = this.UnderlyingCache[serviceType] as IDisposable;

        if(disposableCachedService != null)
        {
          disposableCachedService.Dispose();
        }
      }

      this.UnderlyingCache.Remove(serviceType);
    }

    /// <summary>
    /// Determines wheter this cache instance contains a service implementation instance for the given interface.
    /// </summary>
    /// <typeparam name="TService">
    /// The type of the service to check for.
    /// </typeparam>
    public bool Contains<TService>() where TService : class
    {
      return (this.UnderlyingCache.ContainsKey(typeof(TService))
              && (this.UnderlyingCache[typeof(TService)] is TService));
    }

    /// <summary>
    /// Gets a contained service implementation.
    /// </summary>
    /// <typeparam name='TService'>
    /// The type of the service to retrieve.
    /// </typeparam>
    public TService Get<TService>() where TService : class
    {
      Type serviceType = typeof(TService);
      TService output;
      
      if(!this.UnderlyingCache.ContainsKey(serviceType))
      {
        throw new InvalidOperationException(String.Format("The current service cache instance does not contain a " +
                                                          "service of type '{0}'",
                                                          serviceType.FullName));
      }
      
      output = this.UnderlyingCache[serviceType] as TService;
      
      if(output == null)
      {
        throw new InvalidOperationException(String.Format("Cached service implementation does not match service " +
                                                          "type '{0}'",
                                                          serviceType.FullName));
      }
      
      return output;
    }
    
    #endregion
    
    #region IDisposable implementation
    
    /// <summary>
    /// Releases all resource used by the <see cref="CSF.Patterns.IoC.ServiceInstanceCache"/> object.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Dispose"/> when you are finished using the <see cref="CSF.Patterns.IoC.ServiceInstanceCache"/>.
    /// The <see cref="Dispose"/> method leaves the <see cref="CSF.Patterns.IoC.ServiceInstanceCache"/> in an unusable
    /// state. After calling <see cref="Dispose"/>, you must release all references to the
    /// <see cref="CSF.Patterns.IoC.ServiceInstanceCache"/> so the garbage collector can reclaim the memory that the
    /// <see cref="CSF.Patterns.IoC.ServiceInstanceCache"/> was occupying.
    /// </remarks>
    public void Dispose ()
    {
      foreach(object service in this.UnderlyingCache.Values)
      {
        IDisposable disposableService = service as IDisposable;
        if(disposableService != null)
        {
          disposableService.Dispose();
        }
      }
    }
    
    #endregion
  
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.IoC.ServiceInstanceCache"/> class.
    /// </summary>
    public ServiceInstanceCache()
    {
      _underlyingCache = new Dictionary<Type, object>();
    }
    
    #endregion
  }
}

