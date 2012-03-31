//  
//  TabularDataParser.cs
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

/*
 * This type was originally inspired by the following work by Andreas Knab:
 * * http://knab.ws/blog/index.php?/archives/3-CSV-file-parser-and-writer-in-C-Part-1.html
 * * http://knab.ws/blog/index.php?url=archives/10-CSV-file-parser-and-writer-in-C-Part-2.html
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSF.IO
{
  /// <summary>
  /// Represents a concrete <see cref="ITabularDataParser"/> that is able to read from &amp; write to tabular string data
  /// structures.
  /// </summary>
  public abstract class TabularDataParser : ITabularDataParser
  {
    #region fields
    
    private string _columnDelimiter, _rowDelimiter;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets or sets the character that separates fields of data within a row.
    /// </summary>
    /// <value>
    /// The column delimiter.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    protected virtual string ColumnDelimiter
    {
      get {
        return _columnDelimiter;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException ("value");
        }
        
        _columnDelimiter = value;
      }
    }
    
    /// <summary>
    /// Gets or sets the character that separates rows of data.
    /// </summary>
    /// <value>
    /// The row delimiter.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    protected virtual string RowDelimiter
    {
      get {
        return _rowDelimiter;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException ("value");
        }
        
        _rowDelimiter = value;
      }
    }
    
    #endregion
    
    #region ITabularDataParser implementation
    
    /// <summary>
    /// Reads the specified string-based data and returns a collection representing the tabular data.
    /// </summary>
    /// <param name='stringData'>
    /// Tabular data formatted as a string.
    /// </param>
    public virtual IList<IList<string>> Read(string stringData)
    {
      // TODO: Write this implementation
      throw new NotImplementedException ();
    }
    
    /// <summary>
    /// Reads the specified string-based data and returns a collection representing the tabular data.
    /// </summary>
    /// <param name='stringDataReader'>
    /// A <see cref="TextReader"/> that reads tabular data, formatted as a string.
    /// </param>
    public virtual IList<IList<string>> Read(TextReader stringDataReader)
    {
      // TODO: Write this implementation
      throw new NotImplementedException ();
    }
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The tabular data structure.
    /// </param>
    public virtual string Write(IList<IList<string>> data)
    {
      // TODO: Write this implementation
      throw new NotImplementedException ();
    }
    
    /// <summary>
    /// Write the specified data to a given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='data'>
    /// Data.
    /// </param>
    /// <param name='stringDataWriter'>
    /// String data writer.
    /// </param>
    public virtual void Write(IList<IList<string>> data, TextWriter stringDataWriter)
    {
      // TODO: Write this implementation
      throw new NotImplementedException ();
    }
    
    #endregion
  }
}

