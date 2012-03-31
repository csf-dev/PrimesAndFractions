//  
//  CsvDataParser.cs
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

namespace CSF.IO
{
  /// <summary>
  /// An implementation of <see cref="TabularDataParser"/> for CSV data.
  /// </summary>
  public class CsvDataParser : TabularDataParser
  {
    #region constants
    
    /// <summary>
    /// Gets the quote character that is used to wrap data values that contain otherwise-invalid characters.
    /// </summary>
    private static char QuoteCharacter = '"';
    
    #endregion
    
    #region fields
    
    private List<char> CharactersToQuote;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets or sets a value indicating whether quotes will be added around all values that are written using the
    /// current instance.  If false then quotes will only be added around data values that require quotes.
    /// </summary>
    /// <value>
    /// <c>true</c> if all values should be quoted upon writing; otherwise, <c>false</c>.
    /// </value>
    public virtual bool ForceQuotes
    {
      get;
      set;
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Determines and returns a collection of the characters to quote when writing data.
    /// </summary>
    /// <returns>
    /// The characters to quote.
    /// </returns>
    private List<char> DetermineCharactersToQuote()
    {
      List<char> output = new List<char>();
      
      output.AddRange(this.ColumnDelimiter.ToCharArray());
      output.AddRange(this.RowDelimiter.ToCharArray());
      output.Add(QuoteCharacter);
      
      return output;
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Data.CsvDataParser"/> class.
    /// </summary>
    public CsvDataParser() : this(Environment.NewLine) {}
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Data.CsvDataParser"/> class.
    /// </summary>
    /// <param name='lineEndings'>
    /// The line ending character sequence to use when reading/writing CSV data.
    /// </param>
    public CsvDataParser(string lineEndings)
    {
      this.ColumnDelimiter = ",";
      this.RowDelimiter = lineEndings;
      this.ForceQuotes = false;
      
      this.CharactersToQuote = DetermineCharactersToQuote();
    }
    
    #endregion
  }
}

