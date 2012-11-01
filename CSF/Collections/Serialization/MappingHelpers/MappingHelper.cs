//
//  MappingHelper.cs
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
using System.Reflection;

namespace CSF.Collections.Serialization.MappingHelpers
{
  /// <summary>
  /// Base functionality for mapping helpers.
  /// </summary>
  public class MappingHelper
  {
    #region static methods

    /// <summary>
    /// Creates an implementation of <see cref="IKeyNamingPolicy"/> of the given type using the default factory method.
    /// </summary>
    /// <returns>
    /// The naming policy.
    /// </returns>
    /// <param name='mapping'>
    /// The mapping that this naming policy is associated with.
    /// </param>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    public static IKeyNamingPolicy CreateNamingPolicy<TPolicy>(IMapping mapping)
      where TPolicy : IKeyNamingPolicy
    {
      Func<IMapping,TPolicy> factoryMethod = map => {
        if(map == null)
        {
          string message = "The mapping instance passed to this factory method (for an IKeyNamingPolicy) must not " +
                           "be null.";
          throw new ArgumentNullException("map", message);
        }

        ConstructorInfo ctor = typeof(TPolicy).GetConstructor(new Type[] { typeof(IMapping) });

        if(ctor == null)
        {
          string message = String.Format("Cannot use the default factory method to initialise key-naming-policy type " +
                                         "`{0}' as does not expose a constructor that takes a single IMapping " +
                                         "instance.  Did you mean to use a custom factory method?",
                                         typeof(TPolicy).FullName);
          throw new InvalidOperationException(message);
        }

        return (TPolicy) ctor.Invoke(new object[] { map });
      };

      return CreateNamingPolicy(mapping, factoryMethod);
    }

    /// <summary>
    /// Creates an implementation of <see cref="IKeyNamingPolicy"/> of the given type using a custom factory method.
    /// </summary>
    /// <returns>
    /// The naming policy.
    /// </returns>
    /// <param name='mapping'>
    /// The mapping that this naming policy is associated with.
    /// </param>
    /// <param name='factoryMethod'>
    /// The factory method to use in creating the policy.
    /// </param>
    /// <typeparam name='TPolicy'>
    /// The type of <see cref="IKeyNamingPolicy"/> desired.
    /// </typeparam>
    public static IKeyNamingPolicy CreateNamingPolicy<TPolicy>(IMapping mapping, Func<IMapping,TPolicy> factoryMethod)
      where TPolicy : IKeyNamingPolicy
    {
      if(factoryMethod == null)
      {
        throw new ArgumentNullException("factoryMethod");
      }

      return factoryMethod(mapping);
    }

    #endregion
  }
}

