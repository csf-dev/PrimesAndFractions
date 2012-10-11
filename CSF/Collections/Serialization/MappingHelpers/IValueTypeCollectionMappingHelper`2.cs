//
//  IValueCollectionMappingPart.cs
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

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Interface for a fluent-interface helper that maps struct-like (value type) items within a collection.
  /// </summary>
  public interface IValueTypeCollectionMappingHelper<TObject,TCollectionItem> : ICollectionMappingHelper
    where TObject : class
    where TCollectionItem : struct
  {
    /// <summary>
    /// Maps the collection items as a simple value with its data contained in a single collection key.
    /// </summary>
    ISimpleMappingHelper<TObject,TCollectionItem> Value();
    
    /// <summary>
    /// Maps the collection items as a composite value, with data spread across multiple keys.
    /// </summary>
    ICompositeMappingHelper<TObject,TCollectionItem> Composite();
  }
}

