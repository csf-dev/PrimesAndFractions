//
//  ValueTypeCollectionMappingHelper.cs
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
using CSF.Collections.Serialization.MappingModel;

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Mapping helper type for a collection of value-type items.
  /// </summary>
  public class ValueTypeCollectionMappingHelper<TObject,TCollectionItem>
    : CollectionMappingHelper<IValueTypeCollectionMapping<TCollectionItem>>, IValueTypeCollectionMappingHelper<TObject,TCollectionItem>
    where TObject : class
    where TCollectionItem : struct
  {
    #region IValueTypeCollectionMappingHelper implementation
    
    /// <summary>
    ///  Maps the collection items as a simple value with its data contained in a single collection key. 
    /// </summary>
    public ISimpleMappingHelper<TObject, TCollectionItem> Simple ()
    {
      this.Mapping.MapAs = new SimplePropertyMapping<TCollectionItem>(this.Mapping, null, true);

      return new SimpleMappingHelper<TObject, TCollectionItem>((ISimpleMapping<TCollectionItem>) this.Mapping.MapAs);
    }
    
    /// <summary>
    ///  Maps the collection items as a composite value, with data spread across multiple keys. 
    /// </summary>
    public ICompositeMappingHelper<TObject, TCollectionItem> Composite ()
    {
      this.Mapping.MapAs = new CompositePropertyMapping<TCollectionItem>(this.Mapping, null, true);

      return new CompositeMappingHelper<TObject, TCollectionItem>((ICompositeMapping<TCollectionItem>) this.Mapping.MapAs);
    }
    
    /// <summary>
    ///  Facilitates the setting of a key-naming-policy into the associated mapping. 
    /// </summary>
    /// <returns>
    ///  The current mapping helper instance, to facilitate chaining of methods. 
    /// </returns>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    public IValueTypeCollectionMappingHelper<TObject,TCollectionItem> NamingPolicy<TPolicy>()
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
    public IValueTypeCollectionMappingHelper<TObject,TCollectionItem> NamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy
    {
      this.Mapping.KeyNamingPolicy = MappingHelper.CreateNamingPolicy<TPolicy>(this.Mapping, factoryMethod);
      return this;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the value-type collection mapping helper type.
    /// </summary>
    /// <param name='mapping'>
    /// Mapping.
    /// </param>
    public ValueTypeCollectionMappingHelper(IValueTypeCollectionMapping<TCollectionItem> mapping) : base(mapping) {}

    #endregion
  }
}

