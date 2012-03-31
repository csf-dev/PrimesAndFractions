
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CSF.Data
{
  public class CsvStream
  {
    #region fields
    
    private TextReader rawStream;
    private char[] buffer;
    private int bufferMaxLength, position, bufferLength;
    private bool endOfStream;
    
    #endregion
    
    #region constructor
    
    public CsvStream(TextReader stream)
    {
      rawStream = stream;
      bufferMaxLength = 4096;
      buffer = new char[bufferMaxLength];
      position = 0;
      endOfStream = false;
    }
    
    #endregion
    
    #region public methods
    
    public List<string> GetNextRow()
    {
      List<string> output;
      bool endOfLine;
      string item = null;
      
      if(endOfStream) 
      {
        return null;
      }
      
      endOfLine = false;
      output = new List<string>();
      
      item = readNextItem(ref endOfLine);
      
      while(item != null && !endOfStream)
      {
        output.Add(item);
        
        if(endOfLine)
        {
          break;
        }
        
        item = readNextItem(ref endOfLine);
      }
      
      /* If we hit the end of the stream and there is an item left (and the last row is not empty) then tag on that
       * last item.
       */
      if(endOfStream && item != null && output.Count > 0)
      {
        output.Add(item);
      }
      else if(output.Count == 0)
      {
        output = null;
      }
      
      return output;
    }
    
    #endregion
    
    #region private methods
    
    private string readNextItem(ref bool endOfLine)
    {
      StringBuilder item = new StringBuilder();
      bool quotesOpened = false, quotesClosed = false, complete = false;
      char c, cPeek;
      
      for(c = readNextCharacter();
          complete == false;
          c = readNextCharacter())
      {
        // This is the case where we are at the end of the stream
        if(c == '\0')
        {
          complete = true;
          endOfStream = true;
          
          if(item.Length == 0)
          {
            return String.Empty;
          }
          else
          {
            break;
          }
        }
        
        /* This is the case where we have come to the end of an item and have
         * discovered the beginning of the next item
         */
        if(c == ',' && (!quotesOpened || (quotesOpened && quotesClosed)))
        {
          complete = true;
          break;
        }
        
        /* The last character on a line MUST be LF (Unicode '\x0A').  In
         * this case we need to check see if the last character in the item is
         * a CR (Unicode '\x0D') and if it is, remove it.
         */
        if(c == '\x0A' && (!quotesOpened || (quotesOpened && quotesClosed)))
        {
          if(item.Length > 0 && item[item.Length -1] == '\x0D')
          {
            item.Length--;
          }
          
          complete = true;
          endOfLine = true;
          
          if(item.Length == 0) 
          {
            return String.Empty;
          }
          else
          {
            break;
          }
        }
        
        /* Detect and ignore leading and trailling whitespace - this can be
         * both spaces or tabs
         */
        if((c == ' ' || c == '\x09') &&
           ((item.Length == 0 && !quotesOpened) ||
            (quotesOpened && quotesClosed)))
        {
          continue;
        }
        
        /* Detect quotation symbols.  There are several possible scenarios
         * here:
         */
        if(c == '"')
        {
          /* This is the first quotation character we have found - the item is
           * zero length and we have not found a quote already.  Mark quotes
           * open and continue searching for data.
           */
          if(item.Length == 0 && !quotesOpened)
          {
            quotesOpened = true;
            continue;
          }
          /* If quotes are already opened then determine whether this is a
           * double-quote (parsed as a single-quote) or an end-quote
           */
          else if(quotesOpened && !quotesClosed)
          {
            /* Peek ahead - if the next character is not a quote then we have
             * reached the end of a quoted section.
             * NOTE: If the next character IS a quote then we can leave the
             * routine to continue, because it will later be dealt with as
             * regular data.
             */
            cPeek = readNextCharacter();
            if(cPeek != '"')
            {
              quotesClosed = true;
              switch(cPeek)
              {
                case ',':
                  complete = true;
                  break;
                case '\0':
                  complete = true;
                  endOfStream = true;
                  break;
                case '\x0A':
                  complete = true;
                  endOfLine = true;
                  break;
              }
              if(complete == true) break;
              else continue;
            }
          }
          else
          {
            /* If we get this far (the item has already begun, but we had not
             * opened quotes) and we find a quote, then technically the file
             * breaks the CSV spec, because in order to use quotes, the field
             * must be enclosed within quotes.  I want to be strict in this
             * implementation, so I'm going to throw an exception.
             */
            throw new IOException("The CSV data appears to be invalid, a data item that is not enclosed in " +
                                  "\"quotation marks\" contains a quotation mark.");
          }
        }
        
        // The item is vaid data, so let's append it
        item.Append(c);
      }
      
      /* Now, we have some data to return.  In case the data had any trailling
       * whitespace (and we weren't in quoted mode) then discard it.
       */
      if(item.Length > 0)
      {
        for(c = item[item.Length -1];
            quotesOpened == false && (c == ' ' || c == '\x09');
            c = item[item.Length -1])
        {
          item.Length --;
          if(item.Length == 0) break;
        }
      }
      return item.ToString();
    }
    
    private char readNextCharacter()
    {
      if(position >= bufferLength)
      {
        bufferLength = rawStream.ReadBlock(buffer, 0, buffer.Length);
        if(bufferLength == 0) return '\0';
        position = 0;
      }
      return buffer[position++];
    }
    
    #endregion
  }
}
