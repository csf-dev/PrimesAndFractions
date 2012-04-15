//  
//  IIniDocument.cs
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
using System.IO;
using System.Collections.Generic;

namespace CSF.IO
{
  /// <summary>
  /// Interface for a document that implements the INI file specification.
  /// </summary>
  public interface IIniDocument : IIniSection
  {
    #region properties
    
    /// <summary>
    /// Gets or sets the sections that this document contains.
    /// </summary>
    /// <value>
    /// A collection of the sections.
    /// </value>
    IDictionary<string, IIniSection> Sections { get; set; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Write this instance (as an INI-formatted file) to the specified path.
    /// </summary>
    /// <param name='filePath'>
    /// The path to save the written file.
    /// </param>
    void Write(string filePath);
    
    /// <summary>
    /// Write this instance to a string (as INI-formatted data).
    /// </summary>
    string Write();
    
    /// <summary>
    /// Write this instance (as INI-formatted data) to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='writer'>
    /// A <see cref="TextWriter"/>.
    /// </param>
    void Write(TextWriter writer);
    
    #endregion
  }
}

