//
//  NullableConvert.cs
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
  /// Static convertor type that converts to nullable types, which return a null state if conversion failed.
  /// </summary>
  public static class NullableConvert
  {
    /// <summary>
    /// Converts the specified value to a nullable instance of the specified type.
    /// </summary>
    /// <param name='value'>
    /// The value to convert.
    /// </param>
    /// <typeparam name='TDestination'>
    /// The target/destination type.
    /// </typeparam>
    public static Nullable<TDestination> To<TDestination>(object value) where TDestination : struct
    {
      TDestination? output = null;

      try
      {
        output = (TDestination) Convert.ChangeType(value, typeof(TDestination));
      }
      // Just drop any of these exceptions on the floor, they all indicate that the conversion failed
      catch(ArgumentNullException) {}
      catch(FormatException) {}
      catch(OverflowException) {}
      catch(InvalidCastException) {}

      return output;
    }
  }
}

