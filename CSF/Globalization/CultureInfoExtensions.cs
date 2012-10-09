//
//  CalendarExtensions.cs
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
using System.Globalization;

namespace CSF.Globalization
{
  /// <summary>
  /// Extension methods for a calendar object.
  /// </summary>
  public static class CultureInfoExtensions
  {
    /// <summary>
    /// Gets all of the months in the year and their names.
    /// </summary>
    /// <returns>
    /// The months in the year.
    /// </returns>
    /// <param name='culture'>
    /// Culture information, relating to which culture to return this information in.
    /// </param>
    public static IDictionary<int, string> GetAllMonths(this CultureInfo culture)
    {
      return culture.GetAllMonths(DateTime.Today.Year);
    }

    /// <summary>
    /// Gets all of the months in the year and their names.
    /// </summary>
    /// <returns>
    /// The months in the year.
    /// </returns>
    /// <param name='culture'>
    /// Culture information, relating to which culture to return this information in.
    /// </param>
    /// <param name='year'>
    /// The year for which we want the months data.
    /// </param>
    public static IDictionary<int, string> GetAllMonths(this CultureInfo culture, int year)
    {
      IDictionary<int, string> output = new Dictionary<int, string>();

      if(culture == null)
      {
        throw new ArgumentNullException("culture");
      }

      int monthsInYear = culture.Calendar.GetMonthsInYear(year);
      for(int month = 1; month <= monthsInYear; month++)
      {
        output.Add(month, culture.DateTimeFormat.GetMonthName(month));
      }

      return output;
    }
  }
}

