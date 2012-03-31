//  
//  CsvParser.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2008 CSF Software Limited
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
using System.Text;

namespace CSF.Data
{
  public class CsvParser
  {
    #region constants

    private const bool
      USE_HEADER_DEFAULT      = false,
      FORCE_QUOTES_DEFAULT    = false;
    
    private static readonly char[]
      QUOTED_CHARACTERS       = ",\"\n\r".ToCharArray();

    #endregion

    #region fields

    private bool useHeaderRow, forceQuotes;

    #endregion

    #region properties

    public bool ForceQuotes
    {
      get {
        return forceQuotes;
      }
      set {
        forceQuotes = value;
      }
    }

    public bool UseHeaderRow
    {
      get {
        return useHeaderRow;
      }
      set {
        useHeaderRow = value;
      }
    }

    #endregion

    #region methods
    
    public StringDataTable Read(TextReader data)
    {
      CsvStream stream = new CsvStream(data);
      StringDataTable output = null;
      int currentRow;
      List<string> rowItems = null;
      
      // If we are using a header row then get the first row of data and set it up as the headers
      if(UseHeaderRow)
      {
        rowItems = stream.GetNextRow();
        
        if(rowItems == null)
        {
          throw new IOException("Error whilst parsing the header.  Likely this is not valid CSV data.");
        }
        
        // Initialise the output data table
        output = new StringDataTable(rowItems.Count);
        
        for(int i = 0; i < rowItems.Count; i++)
        {
          output.Headers[i] = rowItems[i];
        }
      }
      
      currentRow = 1;
      rowItems = stream.GetNextRow();
      
      // Get each row and store it in the data table
      while(rowItems != null)
      {
        // If the table has not yet been initialised then we can do that now
        if(output == null)
        {
          output = new StringDataTable(rowItems.Count);
        }
        
        try
        {
          output.Add(rowItems);
        }
        catch (ArgumentOutOfRangeException ex)
        {
          throw new IOException(String.Format("Mismatched number of columns in row {0}", currentRow), ex);
        }
        
        rowItems = stream.GetNextRow();
        currentRow ++;
      }
      
      if(output == null)
      {
        throw new IOException ("No valid CSV data was found.");
      }
      
      return output;
    }
    
    public StringDataTable Read(string data)
    {
      TextReader dataReader = new StringReader(data);
      return Read(dataReader);
    }
    
    public TextWriter Write(StringDataTable data, TextWriter writeTo)
    {
      if(UseHeaderRow)
      {
        writeRow (data.Headers, writeTo);
      }
      
      for(int i = 0; i < data.Count; i++)
      {
        writeRow (data[i].Values, writeTo);
      }
      
      return writeTo;
    }
    
    public TextWriter Write(StringDataTable data)
    {
      TextWriter newStream = new StringWriter();
      return Write(data, newStream);
    }

    #endregion
    
    #region private methods
    
    private void writeRow (string[] items, TextWriter stream)
    {
      string item;
      
      for (int i = 0; i < items.Length; i++)
      {
        item = items[i];
        
        if(item == null)
        {
          item = String.Empty;
        }
        
        if(shouldThisBeQuoted(item))
        {
          item = String.Format("\"{0}\"", item.Replace("\"", "\"\""));
        }
        
        if(i != items.Length - 1)
        {
          item = String.Format("{0},", item);
        }
        
        stream.Write(item);
      }
      
      stream.Write ('\n');
    }
    
    private bool shouldThisBeQuoted(string value)
    {
      return (ForceQuotes ||
              value.IndexOfAny(QUOTED_CHARACTERS) > -1 ||
              value.Trim().Length != value.Length);
    }
    
    #endregion

    #region constructor
    
    public CsvParser()
    {
      UseHeaderRow = USE_HEADER_DEFAULT;
      ForceQuotes = FORCE_QUOTES_DEFAULT;
    }
    
    #endregion
  }
}
