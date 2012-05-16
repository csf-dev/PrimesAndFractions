//  
//  RegisteredService.cs
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
  /// Type for describing a registered service implementation within a <see cref="ServiceLocator"/>.
  /// </summary>
  public class RegisteredService
  {
    #region fields
    
    private Func<object> _serviceFactory;
    private ServiceLifetime _lifespan;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets or sets a factory method that creates instances of the service implementation.
    /// </summary>
    /// <value>
    /// The service factory.
    /// </value>
    public Func<object> ServiceFactory
    {
      get {
        return _serviceFactory;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _serviceFactory = value;
      }
    }
    
    /// <summary>
    /// Gets or sets the lifespan of created service instances.
    /// </summary>
    /// <value>
    /// The lifespan.
    /// </value>
    public ServiceLifetime Lifespan
    {
      get {
        return _lifespan;
      }
      set {
        if(!Enum.IsDefined(typeof(ServiceLifetime), value))
        {
          throw new ArgumentException("Unrecognised service lifespan.");
        }
        
        _lifespan = value;
      }
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.IoC.RegisteredService"/> class.
    /// </summary>
    /// <param name='serviceFactory'>
    /// Service factory.
    /// </param>
    public RegisteredService(Func<object> serviceFactory)
    {
      this.ServiceFactory = serviceFactory;
    }
    
    #endregion
  }
}

