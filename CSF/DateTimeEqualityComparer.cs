//
//  DateTimeEqualityComparer.cs
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
using System.Collections;

namespace CSF
{
  /// <summary>
  /// Custom comparison type for comparing equality between <see cref="DateTime"/> instances.
  /// </summary>
  public class DateTimeEqualityComparer : IEqualityComparer
  {
    #region properties

    /// <summary>
    /// Gets the permitted difference between dates to be compared.
    /// </summary>
    /// <value>
    /// The permitted difference.
    /// </value>
    public TimeSpan PermittedDifference
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Tests for equality between the two objects (which should both be instances of <see cref="DateTime"/>) within the
    /// <see cref="PermittedDifference"/>.
    /// </summary>
    /// <param name='obj1'>
    /// A date/time instance
    /// </param>
    /// <param name='obj2'>
    /// A date/time instance
    /// </param>
    public bool AreEqual(object obj1, object obj2)
    {
      bool output;

      if(Object.ReferenceEquals(obj1, obj2))
      {
        output = true;
      }
      else if(obj1 == null)
      {
        output = false;
      }
      else
      {
        try
        {
          DateTime
            dateTime1 = (DateTime) obj1,
            dateTime2 = (DateTime) obj2;

          var difference = (dateTime1 - dateTime2).Duration();
          output = (difference <= this.PermittedDifference);
        }
        catch(InvalidCastException)
        {
          output = obj1.Equals(obj2);
        }
      }

      return output;
    }

    /// <summary>
    /// Unsupported method, would get the hash code of a DateTime.
    /// </summary>
    /// <param name='obj'>
    /// An object
    /// </param>
    public int GetHashCode(object obj)
    {
      throw new NotImplementedException("This method is unsupported");
    }

    #endregion

    #region explicit interface implementation

    bool IEqualityComparer.Equals(object obj1, object obj2)
    {
      return this.AreEqual(obj1, obj2);
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.DateTimeEqualityComparer"/> class.
    /// </summary>
    /// <param name='permittedDifference'>
    /// Permitted difference.
    /// </param>
    public DateTimeEqualityComparer(TimeSpan permittedDifference)
    {
      this.PermittedDifference = permittedDifference;
    }

    #endregion
  }
}

