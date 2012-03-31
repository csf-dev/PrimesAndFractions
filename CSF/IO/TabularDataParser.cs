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
      IList<IList<string>> output;
      
      using(TextReader reader = new StringReader(stringData))
      {
        output = this.Read(reader);
      }
      
      return output;
    }
    
    /// <summary>
    /// Reads the specified string-based data and returns a collection representing the tabular data.
    /// </summary>
    /// <param name='stringDataReader'>
    /// A <see cref="TextReader"/> that reads tabular data, formatted as a string.
    /// </param>
    public virtual IList<IList<string>> Read(TextReader stringDataReader)
    {
      ITabularDataStream readHelper = this.GetDataStream(stringDataReader);
      int currentRow = 0;
      IList<IList<string>> output = null;
      
      foreach(IList<string> row in readHelper)
      {
        try
        {
          if(output == null)
          {
            output = new TabularDataList(row.Count);
          }
          output.Add(row);
        }
        catch(ArgumentException ex)
        {
          string message = String.Format("An error was encountered whilst parsing row {0}.", currentRow);
          throw new InvalidOperationException(message, ex);
        }
        currentRow++;
      }
      
      return output;
    }
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The tabular data structure.
    /// </param>
    public virtual string Write(IList<IList<string>> data)
    {
      StringBuilder output = new StringBuilder();
      
      using(TextWriter writer = new StringWriter(output))
      {
        this.Write(data, writer);
      }
      
      return output.ToString();
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
      if(data == null)
      {
        throw new ArgumentNullException ("data");
      }
      
      for(int rowNumber = 0; rowNumber < data.Count; rowNumber++)
      {
        IList<string> row = data[rowNumber];
        this.Write(row, stringDataWriter);
        if(rowNumber < data.Count - 1)
        {
          stringDataWriter.Write(this.RowDelimiter);
        }
      }
    }
    
    /// <summary>
    /// Gets an instance of a helper type that assists in the parsing of data from a <paramref name="reader"/>.
    /// </summary>
    /// <returns>
    /// The data stream helper type.
    /// </returns>
    /// <param name='reader'>
    /// A <see cref="TextReader"/>.
    /// </param>
    protected abstract ITabularDataStream GetDataStream(TextReader reader);
    
    /// <summary>
    /// Writes a single row of data to the <paramref name="writer"/>.
    /// </summary>
    /// <param name='row'>
    /// The row to write
    /// </param>
    /// <param name='writer'>
    /// The <see cref="TextWriter"/> to write to
    /// </param>
    protected abstract void Write(IList<string> row, TextWriter writer);
    
    #endregion
    
    #region contained types
    
    /// <summary>
    /// Interface for a stream of tabular data
    /// </summary>
    public interface ITabularDataStream : IEnumerable<IList<string>> {}
    
    #endregion
  }
}

