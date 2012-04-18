//  
//  EnumExtensions.cs
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
using System.Reflection;
using System.ComponentModel;

namespace CSF
{
  /// <summary>
  /// Type containing extension methods that are useful to enumerated types.
  /// </summary>
  public static class EnumExtensions
  {
    /// <summary>
    /// Gets the description from an enumeration member.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method uses the presence of <see cref="DescriptionAttribute"/> to get a human-readable description from an
    /// enumeration value.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The description, or a null reference if no description attribute was present on the enumeration member indicated
    /// by <paramref name="value"/>.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when the specified value does not correspond to a member of the enumeration.
    /// </exception>
    public static string GetUIText(this Enum value)
    {
      if(value == null)
      {
        throw new ArgumentNullException ("value");
      }
      
      Type enumType = value.GetType();
      string fieldName = value.ToString();
      FieldInfo field = enumType.GetField(fieldName);
      string output;
      
      if(field == null)
      {
        throw new ArgumentException(String.Format("'{0}' is not a member of the enumeration {1}",
                                                  fieldName,
                                                  enumType.ToString()),
                                    "value");
      }
      
      object[] attributes = field.GetCustomAttributes(typeof(UITextAttribute), false);
      
      if(attributes.Length == 0)
      {
        output = null;
      }
      else
      {
        UITextAttribute attribute = (UITextAttribute) attributes[0];
        output = attribute.Text;
      }
      
      return output;
    }
  }
}

