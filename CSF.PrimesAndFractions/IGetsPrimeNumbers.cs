﻿//
// IGetsPrimeNumbers.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2020 Craig Fowler
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System.Collections.Generic;

namespace CSF
{
    /// <summary>
    /// A service which is capable of getting/generating prime numbers from two (the first prime number)
    /// up to an arbitrary upper limit.
    /// </summary>
    public interface IGetsPrimeNumbers
    {
        /// <summary>
        /// Gets a sequence of all prime numbers starting with the first prime number (two) and ending
        /// with the highest prime number which is equal to or less than the specified <paramref name="upperLimit"/>.
        /// </summary>
        /// <returns>An ordered sequence of prime numbers.</returns>
        /// <param name="upperLimit">The highest numeric value for which to get prime numbers; this method will not get any prime numbers which are greater than this number.</param>
        IEnumerable<long> GetPrimeNumbers(long upperLimit);
    }
}
