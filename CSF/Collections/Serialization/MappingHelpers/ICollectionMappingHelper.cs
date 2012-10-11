//
//  ICollectionMappingHelper.cs
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
  /// Interface for a mapping helper that maps a collection of items.
  /// </summary>
  public interface ICollectionMappingHelper : IMappingHelper
  {
    /// <summary>
    /// Indicates that a single value within the serialized collection contains many object values, separated by a given
    /// string.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Storing data in this manner is appropriate where be used where the individual data items are simple and may be
    /// separated in this way.  For example a set of numeric items <c>1,5,9,23</c> could be deserialized in this way
    /// using a collection of values found within a single key.
    /// </para>
    /// </remarks>
    /// <param name='separator'>
    /// The separator string for values.
    /// </param>
    void UsingAggregateSeparator(string separator);
    
    /// <summary>
    /// Indicates that many values are found within multiple keys, each using a numeric prefix in front of the regular
    /// key-name.
    /// </summary>
    /// <param name='separator'>
    /// The string that separates the main part of the key-name from the numeric prefix.
    /// </param>
    void UsingNumericPrefix(string separator);

    /// <summary>
    /// Indicates that many values are found within multiple keys, each using a numeric suffix after the regular
    /// key-name.
    /// </summary>
    /// <param name='separator'>
    /// The string that separates the main part of the key-name from the numeric suffix.
    /// </param>
    void UsingNumericSuffix(string separator);
  }
}

