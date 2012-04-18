//  
//  ICalculator.cs
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
  /// Interface for a type that provides calculation services for <see cref="System.String"/> values, returning numeric
  /// results.
  /// </summary>
  public interface ICalculator
  {
    #region methods
    
    /// <summary>
    /// Calculates the input and returns the result as a <see cref="System.Decimal"/>.
    /// </summary>
    /// <param name='input'>
    /// The string-based input from which to calculate a result.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="input"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="FormatException">
    /// Thrown if the <paramref name="input"/> was not formatted in a manner that this calculator can understand.
    /// </exception>
    decimal CalculateDecimal(string input);
    
    /// <summary>
    /// Tries the input and exposes the <paramref name="result"/> the result as a <see cref="System.Decimal"/> or
    /// otherwise return <c>false</c>.
    /// </summary>
    /// <returns>
    /// A value that indicates whether or not the calculation was a success or not.
    /// </returns>
    /// <param name='input'>
    /// The string-based input from which to calculate a result.
    /// </param>
    /// <param name='result'>
    /// If the return value is <c>true</c>, this parameter exposes the calculated result.  If <c>false</c> then the
    /// value exposed by this property is undefined.
    /// </param>
    bool TryCalculateDecimal(string input, out decimal result);
    
    /// <summary>
    /// Calculates the input and returns the result as a <see cref="System.Int32"/>.
    /// </summary>
    /// <param name='input'>
    /// The string-based input from which to calculate a result.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="input"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="FormatException">
    /// Thrown if the <paramref name="input"/> was not formatted in a manner that this calculator can understand.
    /// </exception>
    int CalculateInteger(string input);
    
    /// <summary>
    /// Tries the input and exposes the <paramref name="result"/> the result as a <see cref="System.Int32"/> or
    /// otherwise return <c>false</c>.
    /// </summary>
    /// <returns>
    /// A value that indicates whether or not the calculation was a success or not.
    /// </returns>
    /// <param name='input'>
    /// The string-based input from which to calculate a result.
    /// </param>
    /// <param name='result'>
    /// If the return value is <c>true</c>, this parameter exposes the calculated result.  If <c>false</c> then the
    /// value exposed by this property is undefined.
    /// </param>
    bool TryCalculateInteger(string input, out int result);
    
    #endregion
  }
}

