//
//  AssemblyExtensions.cs
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
using System.Reflection;
using System.IO;

namespace CSF.Reflection
{
  /// <summary>
  /// Extension methods for the Assembly type.
  /// </summary>
  public static class AssemblyExtensions
  {
    /// <summary>
    /// Extension method reads a text-based resource stored within an assembly.
    /// </summary>
    /// <returns>
    /// The manifest resource text.
    /// </returns>
    /// <param name='assembly'>
    /// The assembly
    /// </param>
    /// <param name='resourceName'>
    /// Resource name.
    /// </param>
    public static string GetManifestResourceText(this Assembly assembly, string resourceName)
    {
      string output;

      if(assembly == null)
      {
        throw new ArgumentNullException("assembly");
      }

      using(Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
      {
        if(resourceStream == null)
        {
          string message = String.Format("Manifest resource '{0}' was not found in the assembly '{1}'",
                                         resourceName,
                                         assembly.FullName);
          throw new InvalidOperationException(message);
        }

        output = GetResourceText(resourceStream);
      }

      return output;
    }

    /// <summary>
    /// Extension method reads a text-based resource stored within an assembly.
    /// </summary>
    /// <returns>
    /// The manifest resource text.
    /// </returns>
    /// <param name='assembly'>
    /// Assembly.
    /// </param>
    /// <param name='type'>
    /// Type.
    /// </param>
    /// <param name='resourceName'>
    /// Resource name.
    /// </param>
    public static string GetManifestResourceText(this Assembly assembly, Type type, string resourceName)
    {
      // TODO: Write this implementation
      throw new NotImplementedException("This method is not yet supported");
    }

    private static string GetResourceText(Stream resourceStream)
    {
      string output;

      using(TextReader reader = new StreamReader(resourceStream))
      {
        output = reader.ReadToEnd();
      }

      return output;
    }
  }
}

