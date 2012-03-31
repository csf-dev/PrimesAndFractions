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
using System.IO;
using System.Text;

namespace CSF.IO
{
  /// <summary>
  /// An implementation of <see cref="TabularDataParser"/> for CSV data.
  /// </summary>
  public class CsvDataParser : TabularDataParser
  {
    #region constants
    
    private const char
      Quote         = '"',
      EOF           = '\0';
    private const int DefaultBufferLength = 4096;
    
    #endregion
    
    #region fields
    
    private char[] CharactersToQuote;
    private int _readBufferMaxLength;
    
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
  
    /// <summary>
    /// Gets or sets the maximum length (in bytes) of the read buffer that is used by CSV-reading methods.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It is usually fine to leave this value at its default but if desired it may be changed to any positive
    /// integer value.
    /// </para>
    /// </remarks>
    /// <value>
    /// The maximum length of the read buffer.
    /// </value>
    public int ReadBufferMaxLength
    {
      get {
        return _readBufferMaxLength;
      }
      set {
        if(value < 1)
        {
          throw new ArgumentOutOfRangeException("Maximum buffer length may not be less than one byte.");
        }
        
        _readBufferMaxLength = value;
      }
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
      output.Add(Quote);
      
      return output;
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
    protected override TabularDataParser.ITabularDataStream GetDataStream(TextReader reader)
    {
      return new CsvDataStream(reader) {
        ReadBufferMaxLength = this.ReadBufferMaxLength,
        ColumnDelimiter = this.ColumnDelimiter,
        RowDelimiter = this.RowDelimiter
      };
    }
    
    /// <summary>
    /// Writes a single row of data to the <paramref name="writer"/>.
    /// </summary>
    /// <param name='row'>
    /// The row to write
    /// </param>
    /// <param name='writer'>
    /// The <see cref="TextWriter"/> to write to
    /// </param>
    protected override void Write(IList<string> row, TextWriter writer)
    {
      for(int columnPosition = 0; columnPosition < row.Count; columnPosition++)
      {
        string valueToWrite = row[columnPosition];
        bool writeColumnDelimiter = (columnPosition < row.Count - 1);
        
        if(valueToWrite == null)
        {
          valueToWrite = String.Empty;
        }
        
        if(this.ShouldBeQuoted(valueToWrite))
        {
          writer.Write(String.Format("{0}{1}{0}{2}",
                                     Quote,
                                     valueToWrite.Replace(Quote.ToString(),
                                                          String.Concat(Quote, Quote)),
                                     writeColumnDelimiter? this.ColumnDelimiter : String.Empty));
        }
        else
        {
          writer.Write(String.Format("{0}{1}",
                                     valueToWrite,
                                     writeColumnDelimiter? this.ColumnDelimiter : String.Empty));
        }
      }
    }
    
    /// <summary>
    /// Determines whether the given <paramref name="value"/> should be quoted when writing to a CSV output stream.
    /// </summary>
    /// <returns>
    /// A value that indicates whether the value should be quoted.
    /// </returns>
    /// <param name='value'>
    /// The value to test.
    /// </param>
    private bool ShouldBeQuoted(string value)
    {
      return (this.ForceQuotes
              || value.IndexOfAny(this.CharactersToQuote) > -1
              || value.Trim().Length != value.Length);
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.CsvDataParser"/> class.
    /// </summary>
    public CsvDataParser() : this(Environment.NewLine) {}
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.CsvDataParser"/> class.
    /// </summary>
    /// <param name='lineEndings'>
    /// The line ending character sequence to use when reading/writing CSV data.
    /// </param>
    public CsvDataParser(string lineEndings)
    {
      this.ColumnDelimiter = ",";
      this.RowDelimiter = lineEndings;
      this.ForceQuotes = false;
      this.ReadBufferMaxLength = DefaultBufferLength;
      
      this.CharactersToQuote = DetermineCharactersToQuote().ToArray();
    }
    
    #endregion
    
    #region contained types
    
    /// <summary>
    /// Helper type to assist in the reading of data from a <see cref="TextReader"/> containing CSV data.
    /// </summary>
    public class CsvDataStream : TabularDataParser.ITabularDataStream
    {
      private TextReader _underlyingReader;
      private int _readBufferMaxLength;
      private string _rowDelimiter, _columnDelimiter;
      
      /// <summary>
      /// Gets or sets the underlying <see cref="TextReader"/>.
      /// </summary>
      /// <value>
      /// The underlying reader.
      /// </value>
      /// <exception cref='ArgumentNullException'>
      /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
      /// </exception>
      protected virtual TextReader UnderlyingReader
      {
        get {
          return _underlyingReader;
        }
        set {
          if (value == null) {
            throw new ArgumentNullException ("value");
          }
          
          _underlyingReader = value;
        }
      }
    
      /// <summary>
      /// Gets or sets the maximum length (in bytes) of the read buffer that is used by CSV-reading methods.
      /// </summary>
      /// <remarks>
      /// <para>
      /// It is usually fine to leave this value at its default but if desired it may be changed to any positive
      /// integer value.
      /// </para>
      /// </remarks>
      /// <value>
      /// The maximum length of the read buffer.
      /// </value>
      public int ReadBufferMaxLength
      {
        get {
          return _readBufferMaxLength;
        }
        set {
          if(value < 1)
          {
            throw new ArgumentOutOfRangeException("Maximum buffer length may not be less than one byte.");
          }
          
          _readBufferMaxLength = value;
        }
      } 
      
      /// <summary>
      /// Gets or sets the character that separates fields of data within a row.
      /// </summary>
      /// <value>
      /// The column delimiter.
      /// </value>
      public string ColumnDelimiter
      {
        get {
          return _columnDelimiter;
        }
        set {
          if (value == null)
          {
            throw new ArgumentNullException ("value");
          }
          else if(value.Length != 1)
          {
            throw new ArgumentException("Column delimiter is incorrect length.");
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
      public string RowDelimiter
      {
        get {
          return _rowDelimiter;
        }
        set {
          if (value == null)
          {
            throw new ArgumentNullException ("value");
          }
          else if(value.Length > 2)
          {
            throw new ArgumentException("Row delimiters of more than 2 characters are not supported.");
          }
          
          _rowDelimiter = value;
        }
      }
      
      /// <summary>
      /// Gets the next row from the underlying stream.
      /// </summary>
      /// <returns>
      /// The next row.
      /// </returns>
      public IEnumerator<IList<string>> GetEnumerator()
      {
        return new CsvDataRowEnumerator(this.UnderlyingReader) {
          ReadBufferMaxLength = this.ReadBufferMaxLength,
          RowDelimiter = this.RowDelimiter,
          ColumnDelimiter = this.ColumnDelimiter
        };
      }
      
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
        return this.GetEnumerator();
      }
      
      /// <summary>
      /// Initializes a new instance of the <see cref="CSF.IO.CsvDataParser.CsvDataStream"/> class.
      /// </summary>
      /// <param name='underlyingReader'>
      /// Underlying reader.
      /// </param>
      public CsvDataStream(TextReader underlyingReader)
      {
        this.UnderlyingReader = underlyingReader;
        this.ReadBufferMaxLength = DefaultBufferLength;
      }
    }
    
    /// <summary>
    /// An enumerator type for reading CSV data from a <see cref="TextReader"/>.
    /// </summary>
    public class CsvDataRowEnumerator : IEnumerator<IList<string>>
    {
      private TextReader _underlyingReader;
      private int _readBufferMaxLength;
      
      private bool AtEndOfStream;
      private char[] ReadBuffer;
      private int BufferPosition, BufferLength;
      
      /// <summary>
      /// Gets or sets the underlying <see cref="TextReader"/>.
      /// </summary>
      /// <value>
      /// The underlying reader.
      /// </value>
      /// <exception cref='ArgumentNullException'>
      /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
      /// </exception>
      protected virtual TextReader UnderlyingReader
      {
        get {
          return _underlyingReader;
        }
        set {
          if (value == null) {
            throw new ArgumentNullException ("value");
          }
          
          _underlyingReader = value;
        }
      }
      
      /// <summary>
      /// Gets or sets the maximum length (in bytes) of the read buffer that is used by CSV-reading methods.
      /// </summary>
      /// <remarks>
      /// <para>
      /// It is usually fine to leave this value at its default but if desired it may be changed to any positive
      /// integer value.
      /// </para>
      /// </remarks>
      /// <value>
      /// The maximum length of the read buffer.
      /// </value>
      public int ReadBufferMaxLength
      {
        get {
          return _readBufferMaxLength;
        }
        set {
          if(value < 1)
          {
            throw new ArgumentOutOfRangeException("Maximum buffer length may not be less than one byte.");
          }
          
          _readBufferMaxLength = value;
        }
      }    
      
      /// <summary>
      /// Gets or sets the character that separates fields of data within a row.
      /// </summary>
      /// <value>
      /// The column delimiter.
      /// </value>
      public string ColumnDelimiter
      {
        get;
        set;
      }
      
      /// <summary>
      /// Gets or sets the character that separates rows of data.
      /// </summary>
      /// <value>
      /// The row delimiter.
      /// </value>
      public string RowDelimiter
      {
        get;
        set;
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
          if(currentCharacter == EOF)
          {
            complete = true;
            this.AtEndOfStream = true;
            break;
          }
          
          // We have reached the column delimiter and are not inside a quoted value, this is the end of the item
          if(currentCharacter.ToString() == this.ColumnDelimiter
             && (!quotesOpened || (quotesOpened && quotesClosed)))
          {
            complete = true;
            break;
          }
          
          // We have reached a row delimiter and are not inside a quoted value - this is the end of the row and item
          if(((this.RowDelimiter.Length == 1
               && currentCharacter.ToString() == this.RowDelimiter)
              || (this.RowDelimiter.Length == 2
                  && currentCharacter == this.RowDelimiter[1]
                  && output[output.Length - 1] == this.RowDelimiter[0]))
             && (!quotesOpened
                 || (quotesOpened && quotesClosed)))
          {
            if(this.RowDelimiter.Length == 2)
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
          if(currentCharacter == Quote)
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
              if(nextCharacter != Quote)
              {
                // The only valid scenario here is that the item is now complete and the quotes are closed
                quotesClosed = true;
                complete = true;
                
                if(nextCharacter.ToString() == this.ColumnDelimiter)
                {
                  break;
                }
                else if(nextCharacter == EOF)
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
              throw new IOException("Invalid CSV data.  Found a quotation mark inside an unquoted string value.");
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
          this.ReadBuffer = new char[this.ReadBufferMaxLength];
        }
        
        if(this.BufferPosition >= this.BufferLength)
        {
          this.BufferLength = this.UnderlyingReader.ReadBlock(this.ReadBuffer, 0, this.ReadBuffer.Length);
          atEndOfStream = (this.BufferLength == 0);
          this.BufferPosition = 0;
        }
        
        return atEndOfStream? EOF : this.ReadBuffer[this.BufferPosition ++];
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
      /// Releases all resource used by the <see cref="CSF.IO.CsvDataParser.CsvDataRowEnumerator"/> object.
      /// </summary>
      /// <remarks>
      /// Call <see cref="Dispose"/> when you are finished using the
      /// <see cref="CSF.IO.CsvDataParser.CsvDataRowEnumerator"/>. The <see cref="Dispose"/> method leaves the
      /// <see cref="CSF.IO.CsvDataParser.CsvDataRowEnumerator"/> in an unusable state. After calling
      /// <see cref="Dispose"/>, you must release all references to the
      /// <see cref="CSF.IO.CsvDataParser.CsvDataRowEnumerator"/> so the garbage collector can reclaim the memory that
      /// the <see cref="CSF.IO.CsvDataParser.CsvDataRowEnumerator"/> was occupying.
      /// </remarks>
      /// <exception cref='NotImplementedException'>
      /// Is thrown when a requested operation is not implemented for a given type.
      /// </exception>
      public void Dispose()
      {
        // Intentional no-operation - no disposal is required.
      }
      
      /// <summary>
      /// Initializes a new instance of the <see cref="CSF.IO.CsvDataParser.CsvDataRowEnumerator"/> class.
      /// </summary>
      public CsvDataRowEnumerator(TextReader underlyingReader)
      {
        this.UnderlyingReader = underlyingReader;
        this.AtEndOfStream = false;
        this.ReadBufferMaxLength = DefaultBufferLength;
      }
    }
    
    #endregion
  }
}

