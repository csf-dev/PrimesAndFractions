//
//  CompositeMappingHelper.cs
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
using System.Collections.Generic;

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Mapping helper for composite many-keys-to-one-property mappings.
  /// </summary>
  public class CompositeMappingHelper<TObject,TValue>
    : MappingHelper<ICompositeMapping<TValue>>, ICompositeMappingHelper<TObject,TValue>
    where TObject : class
  {
    #region ICompositeMappingHelper implementation

    /// <summary>
    ///  Facilitates the setting of a key-naming-policy into the associated mapping. 
    /// </summary>
    /// <returns>
    ///  The current mapping helper instance, to facilitate chaining of methods. 
    /// </returns>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    public ICompositeMappingHelper<TObject,TValue> NamingPolicy<TPolicy>()
      where TPolicy : IKeyNamingPolicy
    {
      this.Mapping.AttachKeyNamingPolicy<TPolicy>();
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
    public ICompositeMappingHelper<TObject,TValue> NamingPolicy<TPolicy>(Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy
    {
      this.Mapping.AttachKeyNamingPolicy<TPolicy>(factoryMethod);
      return this;
    }

    /// <summary>
    ///  Specifies a component of this composite mapping, facilitating the mapping of that component. 
    /// </summary>
    /// <param name='identifier'>
    ///  The identifier for that component, within this composite mapping. The identifier may be any object and is
    /// distinguished from other identifiers by its default equality comparer. 
    /// </param>
    /// <param name='mappingMethod'>
    ///  A method body that indicates how the component should be mapped. 
    /// </param>
    public ICompositeMappingHelper<TObject, TValue> Component (object identifier,
                                                               Action<ICompositeComponentMappingHelper<TObject, TValue>> mappingMethod)
    {
      if(!this.Mapping.Components.ContainsKey(identifier))
      {
        this.Mapping.Components[identifier] = new CompositeComponentMapping<TValue>(this.Mapping, identifier);
      }

      ICompositeComponentMappingHelper<TObject,TValue> helper;
      helper = new CompositeComponentMappingHelper<TObject, TValue>(this.Mapping.Components[identifier]);
      mappingMethod(helper);

      return this;
    }

    /// <summary>
    ///  A method body that indicates how instances of the mapped type should be deserialized from the component values. 
    /// </summary>
    /// <param name='deserializationFunction'>
    ///  A method body containing the deserialization function. 
    /// </param>
    public ICompositeMappingHelper<TObject, TValue> Deserialize (Func<IDictionary<object, string>, TValue> deserializationFunction)
    {
      this.Mapping.DeserializationFunction = deserializationFunction;
      return this;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the composite mapping helper type.
    /// </summary>
    /// <param name='mapping'>
    /// Mapping.
    /// </param>
    public CompositeMappingHelper(ICompositeMapping<TValue> mapping) : base(mapping) {}

    #endregion
  }
}

