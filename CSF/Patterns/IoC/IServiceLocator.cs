//  
//  IServiceLocator.cs
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
  /// Interface for a Service Locator type that is capable of registering and providing service implementations
  /// on-demand.
  /// </summary>
  public interface IServiceLocator : IDisposable
  {
    #region methods

    /// <summary>
    /// Registers a service implementation with this locator instance using a factory function.
    /// </summary>
    /// <param name='factoryFunction'>
    /// A factory function that will instantiate an instance of the service.
    /// </param>
    /// <typeparam name='TInterface'>
    /// An interface for the service that is being registered.
    /// </typeparam>
    IServiceLocator Select<TInterface>(Func<object> factoryFunction) where TInterface : class;
    
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
    IServiceLocator Select<TInterface>(Func<object> factoryFunction, ServiceLifetime lifespan) where TInterface : class;
    
    /// <summary>
    /// Gets an implementation of the desired service.
    /// </summary>
    /// <typeparam name='TInterface'>
    /// The interface of the desired service.
    /// </typeparam>
    /// <returns>
    /// An implementation of the requested service.
    /// </returns>
    TInterface GetService<TInterface>() where TInterface : class;
    
    #endregion
  }
}

