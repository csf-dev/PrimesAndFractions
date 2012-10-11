//
//  With.cs
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

namespace CSF.Collections.Serialization
{
  /// <summary>
  /// Static helper class creates instances of <see cref="IKeyNamingPolicy"/> using a fluent-style syntax.
  /// </summary>
  public static class KeyName
  {
    /// <summary>
    /// Creates and returns a default key-naming-rule that does not affect the default/underlying naming strategy.
    /// </summary>
    /// <returns>
    /// A key-naming rule instance.
    /// </returns>
    public static IKeyNamingPolicy Default()
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Creates and returns a key-naming-rule for a directly-specified name.
    /// </summary>
    /// <returns>
    /// A key-naming rule instance.
    /// </returns>
    /// <param name='name'>
    /// The name for the key.
    /// </param>
    public static IKeyNamingPolicy Is(string name)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Creates and returns a key-naming-rule in which the name of the key is derived from the name of the property
    /// and a given prefix.
    /// </summary>
    /// <returns>
    /// A key-naming rule instance.
    /// </returns>
    /// <param name='prefix'>
    /// The prefix for the key.
    /// </param>
    public static IKeyNamingPolicy WithPrefix(string prefix)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Creates and returns a key-naming-rule in which the name of the key is derived from the name of the property
    /// and a given suffix.
    /// </summary>
    /// <returns>
    /// A key-naming rule instance.
    /// </returns>
    /// <param name='suffix'>
    /// The suffix for the key.
    /// </param>
    public static IKeyNamingPolicy WithSuffix(string suffix)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Creates and returns a key-naming-rule in which the name of the key is derived from the name of the property,
    /// prefixed by the string representation of the component identifier.  Only applicable for the mapping of composite
    /// components.
    /// </summary>
    /// <returns>
    /// A key-naming rule instance.
    /// </returns>
    /// <param name='separator'>
    /// A character that separates the body of the key-name and the component identifier.
    /// </param>
    public static IKeyNamingPolicy PrefixComponentIdentifier(char separator)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Creates and returns a key-naming-rule in which the name of the key is derived from the name of the property,
    /// prefixed by the string representation of the component identifier.  Only applicable for the mapping of composite
    /// components.
    /// </summary>
    /// <returns>
    /// A key-naming rule instance.
    /// </returns>
    /// <param name='separator'>
    /// A character that separates the body of the key-name and the component identifier.
    /// </param>
    public static IKeyNamingPolicy SuffixComponentIdentifier(char separator)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }
  }
}

