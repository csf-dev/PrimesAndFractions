//  
//  TabularDataStream.cs
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
  /// A data stream for reading tabular data.
  /// </summary>
  public class TabularDataStream : ITabularDataStream
  {
    #region fields
    
    private TextReader _reader;
    private ITabularDataParser _parser;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets the parser that this read operation is associated with.
    /// </summary>
    /// <value>
    /// The parser instance.
    /// </value>
    public ITabularDataParser Parser
    {
      get {
        return _parser;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _parser = value;
      }
    }
  
    /// <summary>
    /// Gets the underlying <see cref="TextReader"/> used for the read operation.
    /// </summary>
    /// <value>
    /// The text reader.
    /// </value>
    public TextReader Reader
    {
      get {
        return _reader;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _reader = value;
      }
    }
    
    #endregion
    
    #region methods
      
    /// <summary>
    /// Gets the next row from the underlying stream.
    /// </summary>
    /// <returns>
    /// The next row.
    /// </returns>
    public IEnumerator<IList<string>> GetEnumerator()
    {
      return new TabularDataStreamEnumerator(this);
    }
    
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.TabularDataStream"/> class.
    /// </summary>
    /// <param name='parser'>
    /// The parser associated with this read operation.
    /// </param>
    /// <param name='reader'>
    /// The underlying <see cref="TextReader"/>.
    /// </param>
    public TabularDataStream(ITabularDataParser parser, TextReader reader)
    {
      this.Parser = parser;
      this.Reader = reader;
    }
    
    #endregion
  }
}

