//  
//  ServiceLocator.cs
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
using System.Web;
using System.Linq;

namespace CSF.Patterns.IoC
{
  /// <summary>
  /// Service locator type provides an abstract registry for access to service types.
  /// </summary>
  public class ServiceLocator : IServiceLocator
  {
    #region constants
    
    /// <summary>
    /// Constant value represents the default lifetime that registered services will take.
    /// </summary>
    public static readonly ServiceLifetime DefaultLifetime = ServiceLifetime.Singleton;
    
    #endregion
    
    #region fields
    
    private Dictionary<Type, IRegisteredService> _registeredServiceFactories;
    
    private static IServiceInstanceCache _singletonServices;
    
    [ThreadStatic]
    private static IServiceInstanceCache _threadServices;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets the registered service factories within this type.
    /// </summary>
    /// <value>
    /// The registered service factories.
    /// </value>
    protected Dictionary<Type, IRegisteredService> RegisteredServiceFactories
    {
      get {
        return _registeredServiceFactories;
      }
    }
    
    /// <summary>
    /// Gets a collection of singleton services.
    /// </summary>
    /// <value>
    /// The singleton services.
    /// </value>
    protected IServiceInstanceCache SingletonServices
    {
      get {
        return _singletonServices;
      }
    }
    
    /// <summary>
    /// Gets a collection of per-thread services.
    /// </summary>
    /// <value>
    /// The thread services.
    /// </value>
    protected IServiceInstanceCache ThreadServices
    {
      get {
        return _threadServices;
      }
    }
    
    /// <summary>
    /// Gets a collection of per-HttpContext services.
    /// </summary>
    /// <value>
    /// The http context services.
    /// </value>
    protected IServiceInstanceCache HttpContextServices
    {
      get {
        IServiceInstanceCache output = null;
        
        if(HttpContext.Current != null
           && HttpContext.Current.Items[ServiceLocatorHttpModule.HttpContextCacheKey] != null)
        {
          output = HttpContext.Current.Items[ServiceLocatorHttpModule.HttpContextCacheKey] as IServiceInstanceCache;
        }
        
        return output;
      }
    }
    
    #endregion
    
    #region IServiceLocator implementation

    /// <summary>
    /// Registers a service implementation with this locator instance using a factory function.
    /// </summary>
    /// <param name='factoryFunction'>
    /// A factory function that will instantiate an instance of the service.
    /// </param>
    /// <typeparam name='TInterface'>
    /// An interface for the service that is being registered.
    /// </typeparam>
    public virtual IServiceLocator Select<TInterface> (Func<object> factoryFunction) where TInterface : class
    {
      this.Select<TInterface>(factoryFunction, DefaultLifetime);
      return this;
    }

    /// <summary>
    /// Registers a service implementation with this locator instance using a factory function.
    /// </summary>
    /// <param name='factoryFunction'>
    /// A factory function that will instantiate an instance of the service.
    /// </param>
    /// <param name='lifespan'>
    /// A custom lifespan for created implementations.
    /// </param>
    /// <typeparam name='TInterface'>
    /// An interface for the service that is being registered.
    /// </typeparam>
    public virtual IServiceLocator Select<TInterface>(Func<object> factoryFunction,
                                                  ServiceLifetime lifespan) where TInterface : class
    {
      Type interfaceType = typeof(TInterface);
      
      RegisteredService service = new RegisteredService(factoryFunction) {
        Lifespan = lifespan
      };
      
      if(this.RegisteredServiceFactories.ContainsKey(interfaceType))
      {
        IServiceInstanceCache cache = this.GetServiceCache(this.RegisteredServiceFactories[interfaceType].Lifespan);
        
        if(cache != null)
        {
          cache.Remove<TInterface>();
        }
      }
      
      this.RegisteredServiceFactories[interfaceType] = service;
      return this;
    }

    /// <summary>
    /// Gets an implementation of the desired service.
    /// </summary>
    /// <typeparam name='TInterface'>
    /// The interface of the desired service.
    /// </typeparam>
    /// <returns>
    /// An implementation of the requested service.
    /// </returns>
    public virtual TInterface GetService<TInterface>() where TInterface : class
    {
      Type interfaceType = typeof(TInterface);
      TInterface output;
      
      if(!this.RegisteredServiceFactories.ContainsKey(interfaceType))
      {
        throw new InvalidOperationException(String.Format("This service locator contains no implementation for '{0}'",
                                                          interfaceType.FullName));
      }
      
      IRegisteredService service = this.RegisteredServiceFactories[interfaceType];
      IServiceInstanceCache serviceCache = this.GetServiceCache(service.Lifespan);
      
      if(serviceCache != null
         && serviceCache.Contains<TInterface>())
      {
        output = serviceCache.Get<TInterface>();
      }
      else
      {
        output = service.Create<TInterface>();
        if(serviceCache != null)
        {
          serviceCache.Add<TInterface>(output);
        }
      }
      
      return output;
    }

    /// <summary>
    /// Disposes a service implementation and removes it from the appropriate cache, ready for it to be recreated.
    /// </summary>
    /// <typeparam name='TInterface'>
    /// The interface of the desired service.
    /// </typeparam>
    public virtual void DisposeService<TInterface>() where TInterface : class
    {
      Type interfaceType = typeof(TInterface);
      
      if(!this.RegisteredServiceFactories.ContainsKey(interfaceType))
      {
        throw new InvalidOperationException(String.Format("This service locator contains no implementation for '{0}'",
                                                          interfaceType.FullName));
      }
      
      IRegisteredService service = this.RegisteredServiceFactories[interfaceType];
      IServiceInstanceCache serviceCache = this.GetServiceCache(service.Lifespan);
      
      if(serviceCache.Contains<TInterface>())
      {
        serviceCache.Remove<TInterface>();
      }
    }

    
    /// <summary>
    /// Releases all resource used by the <see cref="CSF.Patterns.IoC.ServiceLocator"/> object.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Dispose"/> when you are finished using the <see cref="CSF.Patterns.IoC.ServiceLocator"/>. The
    /// <see cref="Dispose"/> method leaves the <see cref="CSF.Patterns.IoC.ServiceLocator"/> in an unusable state.
    /// After calling <see cref="Dispose"/>, you must release all references to the
    /// <see cref="CSF.Patterns.IoC.ServiceLocator"/> so the garbage collector can reclaim the memory that the
    /// <see cref="CSF.Patterns.IoC.ServiceLocator"/> was occupying.
    /// </remarks>
    public virtual void Dispose ()
    {
      this.SingletonServices.Dispose();
      this.ThreadServices.Dispose();
      if(this.HttpContextServices != null)
      {
        this.HttpContextServices.Dispose();
      }
    }
    
    /// <summary>
    /// Gets the appropriate service cache for the given lifespan.
    /// </summary>
    /// <returns>
    /// The service cache.
    /// </returns>
    /// <param name='lifetime'>
    /// Lifetime.
    /// </param>
    /// <exception cref='InvalidOperationException'>
    /// Is thrown when an operation cannot be performed.
    /// </exception>
    protected IServiceInstanceCache GetServiceCache(ServiceLifetime lifetime)
    {
      IServiceInstanceCache output;
      
      switch(lifetime)
      {
      case ServiceLifetime.Singleton:
        output = this.SingletonServices;
        break;
      case ServiceLifetime.PerThread:
        output = this.ThreadServices;
        break;
      case ServiceLifetime.PerHttpContext:
        output = this.HttpContextServices;
        break;
      case ServiceLifetime.Transient:
        output = null;
        break;
      default:
        throw new InvalidOperationException("Unknown service lifespan.");
      }
      
      return output;
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.IoC.ServiceLocator"/> class.
    /// </summary>
    public ServiceLocator ()
    {
      _registeredServiceFactories = new Dictionary<Type, IRegisteredService>();
    }
    
    /// <summary>
    /// Initializes the <see cref="CSF.Patterns.IoC.ServiceLocator"/> class.
    /// </summary>
    static ServiceLocator()
    {
      DefaultInstance = new ServiceLocator();
      
      _singletonServices = new ServiceInstanceCache();
      _threadServices = new ServiceInstanceCache();
    }
    
    #endregion
    
    #region static fields
    
    private static IServiceLocator _defaultInstance;
    
    #endregion
    
    #region static properties
    
    /// <summary>
    /// Gets or sets the default singleton <see cref="IServiceLocator"/> instance to use.
    /// </summary>
    /// <value>
    /// The default instance.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    protected static IServiceLocator DefaultInstance
    {
      get {
        return _defaultInstance;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _defaultInstance = value;
      }
    }
    
    #endregion
    
    #region static methods

    /// <summary>
    /// Registers one or more service selections by using an anonymous method.
    /// </summary>
    /// <param name='serviceSelections'>
    /// A method that introduces service selections.
    /// </param>
    public static void Select(Action<IServiceLocator> serviceSelections)
    {
      if(serviceSelections == null)
      {
        throw new ArgumentNullException("serviceSelections");
      }

      serviceSelections(DefaultInstance);
    }

    /// <summary>
    /// Gets an implementation of the desired service.
    /// </summary>
    /// <typeparam name='TInterface'>
    /// The interface of the desired service.
    /// </typeparam>
    /// <returns>
    /// An implementation of the requested service.
    /// </returns>
    public static TInterface Get<TInterface>() where TInterface : class
    {
      return DefaultInstance.GetService<TInterface>();
    }

    /// <summary>
    /// Disposes of a given service implementation and clears it.
    /// </summary>
    /// <remarks>
    /// This essentially removes a cached service implementation such that (the next time it is requested), it will
    /// be re-created using the original factory method.
    /// </remarks>
    /// <typeparam name='TInterface'>
    /// The type of service to dispose of.
    /// </typeparam>
    public static void DisposeAndReplace<TInterface>() where TInterface : class
    {
      DefaultInstance.DisposeService<TInterface>();
    }
    
    #endregion
  }
}

