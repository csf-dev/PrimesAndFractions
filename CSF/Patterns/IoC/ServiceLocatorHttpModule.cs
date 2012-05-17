//  
//  ServiceLocatorHttpModule.cs
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
using System.Web;
using System.Collections.Generic;

namespace CSF.Patterns.IoC
{
  /// <summary>
  /// Implementation of a <see cref="IHttpModule"/> for the caching of services within the current HttpContext.
  /// </summary>
  public class ServiceLocatorHttpModule : IHttpModule
  {
    #region constants
    
    /// <summary>
    /// Gets the string 'key' at which the <see cref="IServiceInstanceCache"/> is stored within
    /// <c>HttpContext.Current.Items</c>.
    /// </summary>
    public static readonly string HttpContextCacheKey = typeof(ServiceLocator).FullName;
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Initialises the module</para>
    /// </summary>
    /// <param name="application">
    /// A <see cref="HttpApplication"/>
    /// </param>
    public void Init(HttpApplication application)
    {
      application.PreRequestHandlerExecute += CreateServiceCache;
      application.EndRequest += DisposeServiceCache;
    }
    
    /// <summary>
    /// <para>Performs disposal of the current instance</para>
    /// </summary>
    public void Dispose () { /* Intentional no-operation, no disposal required */ }
    
    /// <summary>
    /// <para>
    /// Creates a <see cref="ServiceLocator"/> cache instance within the current HTTP context to provide access
    /// to the repositories during the lifecycle of the request.
    /// </para>
    /// </summary>
    /// <param name="sender">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <param name="ev">
    /// A <see cref="EventArgs"/>
    /// </param>
    protected void CreateServiceCache(object sender, EventArgs ev)
    {
      HttpContext.Current.Items[HttpContextCacheKey] = new ServiceInstanceCache();
    }
    
    /// <summary>
    /// <para>Disposes of the <see cref="ServiceLocator"/> cache in the current HTTP context.</para>
    /// </summary>
    /// <param name="sender">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <param name="ev">
    /// A <see cref="EventArgs"/>
    /// </param>
    protected void DisposeServiceCache(object sender, EventArgs ev)
    {
      IServiceInstanceCache cache = HttpContext.Current.Items[HttpContextCacheKey] as IServiceInstanceCache;
      
      if(cache != null)
      {
        cache.Dispose();
        HttpContext.Current.Items.Remove(HttpContextCacheKey);
      }
    }
    
    #endregion
  }
}

