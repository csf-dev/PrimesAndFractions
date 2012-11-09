//
//  IClassMapping.cs
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
using System.Reflection;

namespace CSF.KeyValueSerializer.MappingModel
{
  /// <summary>
  /// Interface for a mapping which describes a complex class-like (reference type) which may have property mappings of
  /// its own.
  /// </summary>
  public interface IClassMapping<TObject> : IMapping
    where TObject : class
  {
    /// <summary>
    /// If non-null then this entire object is mapped for serialization/deserialization using a single mapping instance.
    /// </summary>
    /// <value>
    /// The mapping for this entire type.
    /// </value>
    IMapping MapAs { get; set; }

    /// <summary>
    /// Gets a collection of the mappings for this type.
    /// </summary>
    /// <value>
    /// The mappings.
    /// </value>
    ICollection<IMapping> Mappings { get; set; }

    /// <summary>
    /// Gets the factory method that is used for constructing instances of this type.
    /// </summary>
    /// <value>
    /// The factory method.
    /// </value>
    Func<TObject> FactoryMethod { get; set; }
  }
}

