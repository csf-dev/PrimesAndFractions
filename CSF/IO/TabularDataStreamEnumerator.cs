//  
//  TabularDataStreamEnumerator.cs
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
using System.Text;

namespace CSF.IO
{
  /// <summary>
  /// Enumerator type for a tabular data stream.
  /// </summary>
  public class TabularDataStreamEnumerator : IEnumerator<IList<string>>
  {
    #region fields
    
    private TabularDataStream _stream;
    
    private bool AtEndOfStream;
    private char[] ReadBuffer;
    private int BufferPosition, BufferLength;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets or sets the stream associated with this enumerator.
    /// </summary>
    /// <value>
    /// The stream.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    protected virtual TabularDataStream Stream
    {
      get {
        return _stream;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _stream = value;
      }
    }
    
    /// <summary>
    /// Gets the current row of data.
    /// </summary>
    /// <value>
    /// The current.
    /// </value>
    public IList<string> Current
    {
      get;
      private set;
    }

    object System.Collections.IEnumerator.Current
    {
      get {
        return this.Current;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Moves the current instance to the next row of data.
    /// </summary>
    /// <returns>
    /// A value indicating whether there was another row of data.  If <c>false</c> then there is not.
    /// </returns>
    public bool MoveNext()
    {
      bool output = false;
      
      if(this.AtEndOfStream)
      {
        this.Current = null;
      }
      else
      {
        bool atEndOfRow = false;
        this.Current = new List<string>();
        
        do
        {
          this.Current.Add(this.ReadNextItem(ref atEndOfRow));
        }
        while (!this.AtEndOfStream && !atEndOfRow);
        
        output = true;
      }
      
      return output;
    }
 
    /// <summary>
    /// Resets the current instance to the beginning of the stream (not supported).
    /// </summary>
    /// <exception cref='NotSupportedException'>
    /// Is thrown when an object cannot perform an operation.
    /// </exception>
    public void Reset()
    {
      throw new NotSupportedException();
    }
    
    /// <summary>
    /// Releases all resource used by the <see cref="CSF.IO.TabularDataStreamEnumerator"/> object.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Dispose"/> when you are finished using the <see cref="CSF.IO.TabularDataStreamEnumerator"/>. The
    /// <see cref="Dispose"/> method leaves the <see cref="CSF.IO.TabularDataStreamEnumerator"/> in an unusable state.
    /// After calling <see cref="Dispose"/>, you must release all references to the
    /// <see cref="CSF.IO.TabularDataStreamEnumerator"/> so the garbage collector can reclaim the memory that the
    /// <see cref="CSF.IO.TabularDataStreamEnumerator"/> was occupying.
    /// </remarks>
    public void Dispose()
    {
      // Intentional no-operation - no disposal is required.
    }
    
    /// <summary>
    /// Reads the next item from the CSV stream.
    /// </summary>
    /// <param name='atEndOfRow'>
    /// A value that indicates whether this item is the last value in the current row.
    /// </param>
    private string ReadNextItem(ref bool atEndOfRow)
    {
      StringBuilder output = new StringBuilder();
      bool
        quotesOpened = false,
        quotesClosed = false,
        complete = false;
      char currentCharacter;
      
      for(currentCharacter = this.GetNextCharacter();
          complete == false;
          currentCharacter = this.GetNextCharacter())
      {
        // We have reached the end of the stream, this is the end of the item
        if(currentCharacter == TabularDataParser.EndOfFile)
        {
          complete = true;
          this.AtEndOfStream = true;
          break;
        }
        
        // We have reached the column delimiter and are not inside a quoted value, this is the end of the item
        if(currentCharacter == this.Stream.Parser.Format.ColumnDelimiter
           && (!quotesOpened || (quotesOpened && quotesClosed)))
        {
          complete = true;
          break;
        }
        
        // We have reached a row delimiter and are not inside a quoted value - this is the end of the row and item
        if(((this.Stream.Parser.Format.RowDelimiter.Length == 1
             && currentCharacter.ToString() == this.Stream.Parser.Format.RowDelimiter)
            || (this.Stream.Parser.Format.RowDelimiter.Length == 2
                && currentCharacter == this.Stream.Parser.Format.RowDelimiter[1]
                && output[output.Length - 1] == this.Stream.Parser.Format.RowDelimiter[0]))
           && (!quotesOpened
               || (quotesOpened && quotesClosed)))
        {
          if(this.Stream.Parser.Format.RowDelimiter.Length == 2)
          {
            output.Length --;
          }
          
          complete = true;
          atEndOfRow = true;
          break;
        }
        
        // This character is either leading or trailing whitespace, ignore it and move to the next character
        if(Char.IsWhiteSpace(currentCharacter)
           && ((output.Length == 0 && !quotesOpened))
               || (quotesOpened && quotesClosed))
        {
          continue;
        }
        
        // This character is a quotation mark, there are several things that we might do here
        if(this.Stream.Parser.Format.QuotationCharacter.HasValue
           && currentCharacter == this.Stream.Parser.Format.QuotationCharacter.Value)
        {
          if(!quotesOpened && output.Length == 0)
          {
            // This is the first quote character - mark that quotes are open and continue to the next character
            quotesOpened = true;
            continue;
          }
          else if(quotesOpened && !quotesClosed)
          {
            char nextCharacter = this.GetNextCharacter();
            
            /* If the next character is not a quote then this is the closing quote in a quoting sequence.
             * If it is a quote then this is an escaped double-quote within a quoted string.  Peeking at the next
             * character like this will cause us to drop that 'peeked' character on the floor in a future pass but
             * that's OK, since in every scenario it's what we wanted to do anyway!
             */
            if(nextCharacter != this.Stream.Parser.Format.QuotationCharacter.Value)
            {
              // The only valid scenario here is that the item is now complete and the quotes are closed
              quotesClosed = true;
              complete = true;
              
              if(nextCharacter == this.Stream.Parser.Format.ColumnDelimiter)
              {
                break;
              }
              else if(nextCharacter == TabularDataParser.EndOfFile)
              {
                AtEndOfStream = true;
                break;
              }
              else
              {
                // The only other valid scenario is that we are at the end of the line
                atEndOfRow = true;
                break;
              }
            }
          }
          else
          {
            throw new IOException("Invalid tabular data.  Found a quotation character inside an unquoted string.");
          }
        }
        
        output.Append(currentCharacter);
      }
      
      while(!quotesOpened && output.Length > 0 && Char.IsWhiteSpace(output[output.Length - 1]))
      {
        output.Length --;
      }
      
      return output.ToString();
    }
    
    /// <summary>
    /// Gets the next character from the buffer, populates/refills the buffer if required.
    /// </summary>
    /// <returns>
    /// The next character.
    /// </returns>
    private char GetNextCharacter()
    {
      bool atEndOfStream = false;
      
      if(this.ReadBuffer == null)
      {
        this.ReadBuffer = new char[this.Stream.Parser.ReadBufferSize];
      }
      
      if(this.BufferPosition >= this.BufferLength)
      {
        this.BufferLength = this.Stream.Reader.ReadBlock(this.ReadBuffer, 0, this.ReadBuffer.Length);
        atEndOfStream = (this.BufferLength == 0);
        this.BufferPosition = 0;
      }
      
      return atEndOfStream? TabularDataParser.EndOfFile : this.ReadBuffer[this.BufferPosition ++];
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.TabularDataStreamEnumerator"/> class.
    /// </summary>
    /// <param name='stream'>
    /// Stream.
    /// </param>
    public TabularDataStreamEnumerator(TabularDataStream stream)
    {
      this.Stream = stream;
      this.AtEndOfStream = false;
    }
    
    #endregion
  }
}

