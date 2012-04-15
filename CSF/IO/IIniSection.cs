//  
//  IIniSection.cs
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
using System.IO;

namespace CSF.IO
{
  /// <summary>
  /// Interface for an INI file section.
  /// </summary>
  public interface IIniSection : IDictionary<string, string>
  {
    /// <summary>
    /// Writes this instance to the given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='writer'>
    /// A <see cref="TextWriter"/> to write the output to.
    /// </param>
    /// <param name='name'>
    /// The name of this section.
    /// </param>
    void WriteTo(TextWriter writer, string name);
  }
}

