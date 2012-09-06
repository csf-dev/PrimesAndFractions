//  
//  TypeExtensions.cs
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
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CSF.Reflection
{
  /// <summary>
  /// Helper type containing extension methods for <see cref="System.Type"/>.
  /// </summary>
  public static class TypeExtensions
  {
    #region constants

    private static readonly Regex NameMangler = new Regex(@"(`\d+)\[.+\]$", RegexOptions.Compiled);

    #endregion

    #region extension methods
    
    /// <summary>
    /// Gets a collection of <see cref="System.Type"/> that are subclasses of the given <paramref name="type"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method only searches for subclasses within a specific assembly.  This overload looks within the assembly
    /// that defines <paramref name="type"/>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A collection of <see cref="System.Type"/>.
    /// </returns>
    /// <param name='type'>
    /// The type for which to find subclasses.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public static IList<Type> GetSubclasses(this Type type)
    {
      if(type == null)
      {
        throw new ArgumentNullException ("type");
      }
      
      return type.GetSubclasses(Assembly.GetAssembly(type));
    }
    
    /// <summary>
    /// Gets a collection of <see cref="System.Type"/> that are subclasses of the given <paramref name="type"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method only searches for subclasses within a specific assembly.  This overload looks within a given
    /// <paramref name="searchAssembly"/>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A collection of <see cref="System.Type"/>.
    /// </returns>
    /// <param name='type'>
    /// The type for which to find subclasses.
    /// </param>
    /// <param name='searchAssembly'>
    /// The <see cref="Assembly"/> in which to search for subclasses of <paramref name="type"/>.
    /// </param>
    public static IList<Type> GetSubclasses(this Type type, Assembly searchAssembly)
    {
      if(searchAssembly == null)
      {
        throw new ArgumentNullException ("searchAssembly");
      }
      else if(type == null)
      {
        throw new ArgumentNullException ("type");
      }
      
      return searchAssembly.GetTypes().Where(x => x.IsSubclassOf(type)).ToList();
    }

    /// <summary>
    /// Gets an interface potentially implemented by the current type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Beware when using this method with generic interfaces.  Due to FCL framework limitations, this method will find
    /// the interface as long as the type implements an interface of the same name with the same number of generic
    /// parameters.  It does NOT check the types of those generic parameters in the case of a 'constructed' generic
    /// interface.
    /// </para>
    /// <example>
    /// For example, the following could occur:
    /// <code>
    /// interface GenericInterface&lt;T&gt; {}
    /// class MyClass : GenericInterface&lt;int&gt; {}
    /// 
    /// typeof(MyClass).GetInterface&lt;GenericInterface&lt;string&gt;&gt;() // Does not return null!
    /// typeof(MyClass).ImplementsInterface&lt;GenericInterface&lt;string&gt;&gt;() // Returns true!
    /// </code>
    /// </example>
    /// </remarks>
    /// <returns>
    /// The interface type, or a null reference if the type does not implement the desired interface.
    /// </returns>
    /// <param name='type'>
    /// The type upon which to search for the interface
    /// </param>
    /// <typeparam name='TInterface'>
    /// The type of the interface to search for.
    /// </typeparam>
    public static Type GetInterface<TInterface>(this Type type)
    {
      return type.GetInterface(typeof(TInterface));
    }

    /// <summary>
    /// Gets an interface potentially implemented by the current type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Beware when using this method with generic interfaces.  Due to FCL framework limitations, this method will find
    /// the interface as long as the type implements an interface of the same name with the same number of generic
    /// parameters.  It does NOT check the types of those generic parameters in the case of a 'constructed' generic
    /// interface.
    /// </para>
    /// <example>
    /// For example, the following could occur:
    /// <code>
    /// interface GenericInterface&lt;T&gt; {}
    /// class MyClass : GenericInterface&lt;int&gt; {}
    /// 
    /// typeof(MyClass).GetInterface&lt;GenericInterface&lt;string&gt;&gt;() // Does not return null!
    /// typeof(MyClass).ImplementsInterface&lt;GenericInterface&lt;string&gt;&gt;() // Returns true!
    /// </code>
    /// </example>
    /// </remarks>
    /// <returns>
    /// The interface type, or a null reference if the type does not implement the desired interface.
    /// </returns>
    /// <param name='type'>
    /// The type upon which to search for the interface
    /// </param>
    /// <param name='interfaceType'>
    /// The type of the interface to search for.
    /// </param>
    public static Type GetInterface(this Type type, Type interfaceType)
    {
      if(interfaceType == null)
      {
        throw new ArgumentNullException("interfaceType");
      }
      else if(!interfaceType.IsInterface)
      {
        throw new ArgumentException("Interface type is not an interface!");
      }

      // FIXME: There is a bug described in the remarks to this method, it should be fixed!

      return type.GetInterface(interfaceType.GetMangledName(), false);
    }

    /// <summary>
    /// Determines whether the current type implements the specified interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Beware when using this method with generic interfaces.  Due to FCL framework limitations, this method will find
    /// the interface as long as the type implements an interface of the same name with the same number of generic
    /// parameters.  It does NOT check the types of those generic parameters in the case of a 'constructed' generic
    /// interface.
    /// </para>
    /// <example>
    /// For example, the following could occur:
    /// <code>
    /// interface GenericInterface&lt;T&gt; {}
    /// class MyClass : GenericInterface&lt;int&gt; {}
    /// 
    /// typeof(MyClass).GetInterface&lt;GenericInterface&lt;string&gt;&gt;() // Does not return null!
    /// typeof(MyClass).ImplementsInterface&lt;GenericInterface&lt;string&gt;&gt;() // Returns true!
    /// </code>
    /// </example>
    /// </remarks>
    /// <returns>
    /// True if the <paramref name="type"/> implements the desired interface, false otherwise.
    /// </returns>
    /// <param name='type'>
    /// The type upon which to search for the interface
    /// </param>
    /// <typeparam name='TInterface'>
    /// The type of the interface to search for.
    /// </typeparam>
    public static bool ImplementsInterface<TInterface>(this Type type)
    {
      return (type.GetInterface(typeof(TInterface)) != null);
    }

    /// <summary>
    /// Determines whether the current type implements the specified interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Beware when using this method with generic interfaces.  Due to FCL framework limitations, this method will find
    /// the interface as long as the type implements an interface of the same name with the same number of generic
    /// parameters.  It does NOT check the types of those generic parameters in the case of a 'constructed' generic
    /// interface.
    /// </para>
    /// <example>
    /// For example, the following could occur:
    /// <code>
    /// interface GenericInterface&lt;T&gt; {}
    /// class MyClass : GenericInterface&lt;int&gt; {}
    /// 
    /// typeof(MyClass).GetInterface&lt;GenericInterface&lt;string&gt;&gt;() // Does not return null!
    /// typeof(MyClass).ImplementsInterface&lt;GenericInterface&lt;string&gt;&gt;() // Returns true!
    /// </code>
    /// </example>
    /// </remarks>
    /// <returns>
    /// True if the <paramref name="type"/> implements the desired interface, false otherwise.
    /// </returns>
    /// <param name='type'>
    /// The type upon which to search for the interface
    /// </param>
    /// <param name='interfaceType'>
    /// The type of the interface to search for.
    /// </param>
    public static bool ImplementsInterface(this Type type, Type interfaceType)
    {
      return (type.GetInterface(interfaceType) != null);
    }

    /// <summary>
    /// Gets the 'mangled' name of a type (removing any generic parameter types, leaving only the number of generic
    /// parameters).
    /// </summary>
    /// <returns>
    /// The mangled name.
    /// </returns>
    /// <param name='type'>
    /// The type for which to get the mangled name.
    /// </param>
    public static string GetMangledName(this Type type)
    {
      string output = type.FullName;

      if(NameMangler.IsMatch(output))
      {
        output = NameMangler.Replace(output, @"$1");
      }

      return output;
    }

    #endregion
  }
}

