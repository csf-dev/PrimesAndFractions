//
// Fraction.IComparable.cs
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
using System;
namespace CSF
{
    public partial struct Fraction : IComparable<Fraction>,
                                     IComparable
    {
        /// <summary>
        /// Compares the current instance to another <see cref="Fraction"/> to determine whether it is
        /// greater-than, less-than or equal-to the other instance.
        /// </summary>
        /// <returns>
        /// <c>-1</c> if the current instance is less than <paramref name="other"/>,
        /// <c>0</c> if the current instance is equal to <paramref name="other"/>
        /// or <c>1</c> is the current instance is greater than <paramref name="other"/>.
        /// </returns>
        /// <param name="other">The other fraction against which to compare.</param>
        public int CompareTo(Fraction other)
        {
            var thisDecimal = ToDecimal();
            var otherDecimal = other.ToDecimal();

            var diff = thisDecimal - otherDecimal;
            if (diff < 0m) return -1;
            if (diff > 0m) return 1;
            return 0;
        }

        /// <summary>
        /// Compares the current instance to another object to determine whether it is
        /// greater-than, less-than or equal-to the other instance.
        /// </summary>
        /// <returns>
        /// <c>-1</c> if the current instance is less than <paramref name="obj"/>,
        /// <c>0</c> if the current instance is equal to <paramref name="obj"/>
        /// or <c>1</c> is the current instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <param name="obj">The other object (which must be a <see cref="Fraction"/>) against which to compare.</param>
        /// <exception cref="ArgumentException">
        /// If the <paramref name="obj"/> is not a <see cref="Fraction"/>.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Fraction fraction) return CompareTo(fraction);
            throw new ArgumentException($"Object to compare with must be a {nameof(Fraction)}.", nameof(obj));
        }
    }
}
