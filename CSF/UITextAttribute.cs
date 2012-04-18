//  
//  UITextAttribute.cs
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

namespace CSF
{
  /// <summary>
  /// Attribute to mark the user interface text that corresponds to an enumeration value.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class UITextAttribute : Attribute
  {
    #region properties
    
    /// <summary>
    /// Gets or sets the text associated with this attribute.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    public string Text
    {
      get;
      set;
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.UITextAttribute"/> class.
    /// </summary>
    /// <param name='text'>
    /// Text.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public UITextAttribute(string text)
    {
      if(text == null)
      {
        throw new ArgumentNullException ("text");
      }
      
      this.Text = text;
    }
    
    #endregion
  }
}

