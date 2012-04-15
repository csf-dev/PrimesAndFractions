//  
//  IniDocument.cs
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
using System.Text.RegularExpressions;
using System.Text;

namespace CSF.IO
{
  /// <summary>
  /// <para>Represents an <c>.ini</c>-style document, such as might be used for a configuration file.</para>
  /// </summary>
  public class IniDocument : IniSection, IIniDocument
  {
    #region constants
    
    private const string
      EMPTY_LINE_REGEX                  = @"^\s*$",
      COMMENT_LINE_REGEX                = @"^\s*[#;].*$",
      SECTION_REGEX                     = @"^\[(\S+)\]$",
      VALUE_REGEX                       = @"^([^:=]+)[:=](.+)$";
    
    private static readonly Regex
      EmptyLine                         = new Regex(EMPTY_LINE_REGEX, RegexOptions.Compiled),
      CommentLine                       = new Regex(COMMENT_LINE_REGEX, RegexOptions.Compiled),
      SectionLine                       = new Regex(SECTION_REGEX, RegexOptions.Compiled),
      ValueLine                         = new Regex(VALUE_REGEX, RegexOptions.Compiled);
    
    #endregion
    
    #region fields
    
    private IDictionary<string, IIniSection> _sections;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Gets or sets a collection of the sections contained by the current instance.</para>
    /// </summary>
    public IDictionary<string, IIniSection> Sections
    {
      get {
        return _sections;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _sections = value;
      }
    }
    
    #endregion
    
    #region methods
    
    
    /// <summary>
    /// Write this instance (as an INI-formatted file) to the specified path.
    /// </summary>
    /// <param name='filePath'>
    /// The path to save the written file.
    /// </param>
    public void Write(string filePath)
    {
      if(filePath == null)
      {
        throw new ArgumentNullException ("filePath");
      }
      
      using(TextWriter writer = new StreamWriter(filePath))
      {
        this.Write(writer);
      }
    }
    
    /// <summary>
    /// Write this instance to a string (as INI-formatted data).
    /// </summary>
    public string Write()
    {
      StringBuilder builder = new StringBuilder();
      
      using(TextWriter writer = new StringWriter(builder))
      {
        this.Write(writer);
      }
      
      return builder.ToString();
    }
    
    /// <summary>
    /// Write this instance (as INI-formatted data) to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='writer'>
    /// A <see cref="TextWriter"/>.
    /// </param>
    public void Write(TextWriter writer)
    {
      if(writer == null)
      {
        throw new ArgumentNullException ("writer");
      }
      
      foreach(string key in this.Keys)
      {
        writer.WriteLine("{0} = {1}", key, this[key]);
      }
      
      foreach(string sectionName in this.Sections.Keys)
      {
        writer.WriteLine("[{0}]", sectionName);
        foreach(string key in this.Sections[sectionName].Keys)
        {
          writer.WriteLine("{0} = {1}", key, this.Sections[sectionName][key]);
        }
      }
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises the current instance.</para>
    /// </summary>
    public IniDocument()
    {
      this.Sections = new Dictionary<string, IIniSection>();
    }
    
    #endregion
    
    #region static methods
    
    /// <summary>
    /// Creates a new <see cref="IIniDocument"/> using information from a file.
    /// </summary>
    /// <param name='path'>
    /// The path from which to read the data.
    /// </param>
    public static IIniDocument Read(FileInfo path)
    {
      IIniDocument output;
      
      if(path == null)
      {
        throw new ArgumentNullException ("path");
      }
      
      using(TextReader reader = new StreamReader(path.FullName))
      {
        output = Read(reader);
      }
      
      return output;
    }
    
    /// <summary>
    /// Creates a new <see cref="IIniDocument"/> using data from a <see cref="TextReader"/>.
    /// </summary>
    /// <param name='reader'>
    /// A <see cref="TextReader"/> that will supply the input data.
    /// </param>
    public static IIniDocument Read(TextReader reader)
    {
      IIniDocument output = new IniDocument();
      IIniSection currentSection = output;
      bool endOfStream = false;
        
      while(!endOfStream)
      {
        string currentLine = reader.ReadLine();
        Match currentMatch;
        
        // If we get a null response then we are at the end of the stream and can exit
        if(currentLine == null)
        {
          endOfStream = true;
          continue;
        }
        
        // We silently skip over empty lines
        if(EmptyLine.Match(currentLine).Success || CommentLine.Match(currentLine).Success)
        {
          continue;
        }
        
        // If we find a 'new section' line then we create a new section and begin dealing with it
        currentMatch = SectionLine.Match(currentLine);
        if(currentMatch.Success)
        {
          string sectionName = currentMatch.Groups[1].Value.Trim();
          currentSection = new IniSection();
          output.Sections.Add(sectionName, currentSection);
          continue;
        }
        
        // If we find a value line then we store it within the current section.
        currentMatch = ValueLine.Match(currentLine);
        if(currentMatch.Success)
        {
          string key = currentMatch.Groups[1].Value.Trim();
          string value = currentMatch.Groups[2].Value.Trim();
          currentSection[key] = value;
          continue;
        }
        
        throw new FormatException("Unexpected line reading ini file");
      }
      
      return output;
    }
    
    /// <summary>
    /// Creates a new <see cref="IIniDocument"/> using data from a string.
    /// </summary>
    /// <param name='data'>
    /// The string data to read.
    /// </param>
    public static IIniDocument Read(string data)
    {
      IIniDocument output;
      
      if(data == null)
      {
        throw new ArgumentNullException ("data");
      }
      
      using(TextReader reader = new StringReader(data))
      {
        output = Read(reader);
      }
      
      return output;
    }
    
    #endregion
  }
}
