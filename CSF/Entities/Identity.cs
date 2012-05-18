//  
//  Identity.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 CSF Software Limited
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

namespace CSF.Entities
{
  /// <summary>
  /// <para>
  /// A static/helper class that creates <see cref="IIdentity"/> instances from <see cref="IEntity"/> types.
  /// </para>
  /// </summary>
  public static class Identity
  {
    #region static factory method
    
    /// <summary>
    /// <para>Static convenience method creates a new <see cref="IIdentity"/> for a generic type.</para>
    /// </summary>
    /// <param name="identifier">
    /// An identifier value.
    /// </param>
    /// <returns>
    /// An <see cref="IIdentity"/>
    /// </returns>
    public static IIdentity Create<TEntity>(object identifier) where TEntity : IEntity
    {
      Type entityBase = typeof(TEntity);
      Type[] genericArguments;
      
      while(entityBase.BaseType != typeof(Object))
      {
        entityBase = entityBase.BaseType;
      }
      
      if(!entityBase.IsGenericType)
      {
        throw new ArgumentException("Base class for the entity type is not an Entity<T> (it is not generic)");
      }
      
      genericArguments = entityBase.GetGenericArguments();
      if(genericArguments.Length != 1)
      {
        throw new ArgumentException("Base class for the entity type is not an Entity<T> (wrong count of generic arguments)");
      }
      
      return (IIdentity) (typeof(Identity<>)
                          .MakeGenericType(genericArguments)
                          .GetConstructor(new Type[] { typeof(Type), genericArguments[0] })
                          .Invoke(new object[] { typeof(TEntity), Convert.ChangeType(identifier, genericArguments[0]) }));
    }
    
    #endregion
  }
}

