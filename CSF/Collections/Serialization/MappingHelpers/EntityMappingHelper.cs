//
//  EntityMappingHelper.cs
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
using CSF.Entities;
using CSF.Collections.Serialization.MappingModel;

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Mapping helper for a type that maps entity objects.
  /// </summary>
  public class EntityMappingHelper<TObject,TEntity,TIdentity>
    : SimpleMappingHelper<TObject,TEntity>, IEntityMappingHelper<TObject,TEntity,TIdentity>
    where TObject : class
    where TEntity : IEntity
  {
    #region methods
    
    /// <summary>
    ///  Facilitates the setting of a key-naming-policy into the associated mapping. 
    /// </summary>
    /// <returns>
    ///  The current mapping helper instance, to facilitate chaining of methods. 
    /// </returns>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    public new IEntityMappingHelper<TObject,TEntity,TIdentity> NamingPolicy<TPolicy>()
      where TPolicy : IKeyNamingPolicy
    {
      this.Mapping.KeyNamingPolicy = MappingHelper.CreateNamingPolicy<TPolicy>(this.Mapping);
      return this;
    }

    /// <summary>
    ///  Facilitates the setting of a key-naming-policy into the associated mapping. 
    /// </summary>
    /// <returns>
    ///  The current mapping helper instance, to facilitate chaining of methods. 
    /// </returns>
    /// <param name='factoryMethod'>
    ///  A custom factory method to use when constructing the naming policy. 
    /// </param>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    public new IEntityMappingHelper<TObject,TEntity,TIdentity> NamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy
    {
      this.Mapping.KeyNamingPolicy = MappingHelper.CreateNamingPolicy<TPolicy>(this.Mapping, factoryMethod);
      return this;
    }

    #endregion

    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the entity mapping helper type.
    /// </summary>
    /// <param name='mapping'>
    /// Mapping.
    /// </param>
    public EntityMappingHelper(ISimpleMapping<TEntity> mapping) : base(mapping) 
    {
      this.Mapping.DeserializationFunction = null;
      this.Mapping.SerializationFunction = x => x.GetIdentity().Value.ToString();
    }

    #endregion
  }
}

