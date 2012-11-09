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
  public static class NullSafe
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
    public static Nullable<TDestination> ConvertTo<TDestination>(object value)
      where TDestination : struct
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

    /// <summary>
    /// Gets a value from a specified input object in null-safe manner.
    /// </summary>
    /// <param name='input'>
    /// The input object instance from which to extract a value.
    /// </param>
    /// <param name='getter'>
    /// A function (taking an input object and outputting the desired output object) that is used to get the output.
    /// </param>
    /// <param name='output'>
    /// If the return value of this method is <c>true</c> then this parameter contains the output of the
    /// getter-function.  If <c>false</c> then the value of this parameter is undefined.
    /// </param>
    /// <returns>
    /// A value that indicates whether or not this operation was successful or not.
    /// </returns>
    /// <typeparam name='TInput'>
    /// The type of input object.
    /// </typeparam>
    /// <typeparam name='TOutput'>
    /// The type of expected output object.
    /// </typeparam>
    public static bool Get<TInput,TOutput>(TInput input, Func<TInput,TOutput> getter, out TOutput output)
    {
      bool result = false;
      output = default(TOutput);

      try
      {
        output = getter(input);
        result = true;
      }
      catch(NullReferenceException) {}

      return result;
    }

    /// <summary>
    /// Gets a string representation of an object (taken from a specified input) in null-safe manner.
    /// </summary>
    /// <param name='input'>
    /// The input object instance from which to extract the value.
    /// </param>
    /// <param name='getter'>
    /// A function (taking an input object and outputting the desired output object) that is used to get the output.
    /// </param>
    /// <returns>
    /// A string representation of the output.  This may be a null reference if the output is null.
    /// </returns>
    /// <typeparam name='TInput'>
    /// The type of input object.
    /// </typeparam>
    /// <typeparam name='TOutput'>
    /// The type of expected output object.
    /// </typeparam>
    public static string ToString<TInput,TOutput>(TInput input, Func<TInput,TOutput> getter)
    {
      TOutput value = default(TOutput);
      bool result = NullSafe.Get<TInput,TOutput>(input, getter, out value);
      string output;

      if(result)
      {
        output = (value != null)? value.ToString() : null;
      }
      else
      {
        output = null;
      }

      return output;
    }
  }
}

