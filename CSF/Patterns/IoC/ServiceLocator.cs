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
    
    private Dictionary<Type, RegisteredService> _registeredServiceFactories;
    
    private static Dictionary<Type, object> _singletonServices;
    
    [ThreadStatic]
    private static Dictionary<Type, object> _threadServices;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets the registered service factories within this type.
    /// </summary>
    /// <value>
    /// The registered service factories.
    /// </value>
    protected Dictionary<Type, RegisteredService> RegisteredServiceFactories
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
    protected Dictionary<Type, object> SingletonServices
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
    protected Dictionary<Type, object> ThreadServices
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
    protected Dictionary<Type, object> HttpContextServices
    {
      get {
        Dictionary<Type, object> output = null;
        
        if(HttpContext.Current != null
           && HttpContext.Current.Items[this.GetType().FullName] != null)
        {
          output = HttpContext.Current.Items[this.GetType().FullName] as Dictionary<Type, object>;
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
    public virtual void SelectService<TInterface> (Func<object> factoryFunction) where TInterface : class
    {
      this.SelectService<TInterface>(factoryFunction, DefaultLifetime);
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
    public virtual void SelectService<TInterface>(Func<object> factoryFunction,
                                                  ServiceLifetime lifespan) where TInterface : class
    {
      Type interfaceType = typeof(TInterface);
      
      RegisteredService service = new RegisteredService(factoryFunction) {
        Lifespan = lifespan
      };
      
      if(this.RegisteredServiceFactories.ContainsKey(interfaceType))
      {
        Dictionary<Type, object> serviceCache;
        serviceCache = this.GetServiceCache(this.RegisteredServiceFactories[interfaceType].Lifespan);
        
        if(serviceCache != null)
        {
          serviceCache.Remove(interfaceType);
        }
      }
      
      this.RegisteredServiceFactories[interfaceType] = service;
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
      
      RegisteredService service = this.RegisteredServiceFactories[interfaceType];
      Dictionary<Type, object> serviceCache = this.GetServiceCache(service.Lifespan);
      
      if(serviceCache != null
         && serviceCache.ContainsKey(interfaceType)
         && serviceCache[interfaceType] != null)
      {
        object cachedOutput = serviceCache[interfaceType];
        
        output = cachedOutput as TInterface;
        if(output == null)
        {
          throw new InvalidOperationException(String.Format("Got a cached service implementation of type '{0}', " +
                                                            "however this does not implement interface type '{1}'",
                                                            cachedOutput.GetType().FullName,
                                                            interfaceType.FullName));
        }
      }
      else
      {
        object tempOutput = service.ServiceFactory();
        
        if(tempOutput == null)
        {
          throw new InvalidOperationException(String.Format("Factory method for service '{0}' returned null output.",
                                                            interfaceType.FullName));
        }
        
        output = tempOutput as TInterface;
        
        if(output == null)
        {
          throw new InvalidOperationException(String.Format("Factory method created an implementation of type '{0}', " +
                                                            "however this does not implement interface type '{1}'",
                                                            tempOutput.GetType().FullName,
                                                            interfaceType.FullName));
        }
        
        if(serviceCache != null)
        {
          serviceCache[interfaceType] = output;
        }
      }
      
      return output;
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
    protected Dictionary<Type, object> GetServiceCache(ServiceLifetime lifetime)
    {
      Dictionary<Type, object> output;
      
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
      _registeredServiceFactories = new Dictionary<Type, RegisteredService>();
    }
    
    /// <summary>
    /// Initializes the <see cref="CSF.Patterns.IoC.ServiceLocator"/> class.
    /// </summary>
    static ServiceLocator()
    {
      DefaultInstance = new ServiceLocator();
      
      _singletonServices = new Dictionary<Type, object>();
      _threadServices = new Dictionary<Type, object>();
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
    /// Registers a service implementation using a factory function.
    /// </summary>
    /// <param name='factoryFunction'>
    /// A factory function that will instantiate an instance of the service.
    /// </param>
    /// <typeparam name='TInterface'>
    /// An interface for the service that is being registered.
    /// </typeparam>
    public static void Select<TInterface>(Func<object> factoryFunction) where TInterface : class
    {
      DefaultInstance.SelectService<TInterface>(factoryFunction);
    }

    /// <summary>
    /// Registers a service implementation using a factory function.
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
    public static void Select<TInterface>(Func<object> factoryFunction,
                                          ServiceLifetime lifespan) where TInterface : class
    {
      DefaultInstance.SelectService<TInterface>(factoryFunction, lifespan);
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
    
    #endregion
  }
}

