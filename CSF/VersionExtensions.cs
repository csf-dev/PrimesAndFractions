//  
//  VersionExtensions.cs
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

namespace CSF
{
  /// <summary>
  /// Container for extension methods to the <see cref="System.Version"/> type.
  /// </summary>
  public static class VersionExtensions
  {
    /// <summary>
    /// Returns the version number formatted as a "semantic versioning" tag-name.
    /// </summary>
    /// <remarks>
    /// See <c>http://semver.org/</c> for more information about the semantic versioning specification.
    /// </remarks>
    /// <returns>
    /// The semantic version number.
    /// </returns>
    /// <param name='version'>
    /// A <see cref="System.Version"/> to operate upon.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public static string ToSemanticVersion(this Version version)
    {
      if(version == null)
      {
        throw new ArgumentNullException ("version");
      }
      
      return String.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
    }
  }
}

