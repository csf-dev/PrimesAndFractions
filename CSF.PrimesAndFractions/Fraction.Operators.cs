//
// Fraction.Operators.cs
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
    public partial struct Fraction
    {
        #region Equality

        /// <summary>
        /// Determines whether a specified instance of <see cref="Fraction"/> is
        /// equal to another specified <see cref="Fraction"/>.
        /// </summary>
        /// <param name="a">The first <see cref="Fraction"/> to compare.</param>
        /// <param name="b">The second <see cref="Fraction"/> to compare.</param>
        /// <returns><c>true</c> if <c>a</c> and <c>b</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Fraction a, Fraction b) => a.Equals(b);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Fraction"/> is
        /// not equal to another specified <see cref="Fraction"/>.
        /// </summary>
        /// <param name="a">The first <see cref="Fraction"/> to compare.</param>
        /// <param name="b">The second <see cref="Fraction"/> to compare.</param>
        /// <returns><c>true</c> if <c>a</c> and <c>b</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Fraction a, Fraction b) => !(a == b);

        #endregion

        #region Arithmetic

        /// <summary>
        /// Adds a <see cref="Fraction"/> to a <see cref="Fraction"/>,
        /// yielding a new <see cref="Fraction"/>.
        /// </summary>
        /// <param name="a">The first <see cref="Fraction"/> to add.</param>
        /// <param name="b">The second <see cref="Fraction"/> to add.</param>
        /// <returns>The <see cref="Fraction"/> that is the sum of the values of <c>a</c> and <c>b</c>.</returns>
        public static Fraction operator +(Fraction a, Fraction b)
        {
            var intA = a.AbsoluteInteger * a.Multiplier;
            var intB = b.AbsoluteInteger * b.Multiplier;

            var numA = a.Numerator * a.Multiplier;
            var numB = b.Numerator * b.Multiplier;

            var numerator = (numA * b.Denominator) + (numB * a.Denominator);
            var denominator = a.Denominator * b.Denominator;

            return new Fraction(intA + intB, numerator, denominator).Simplify();
        }

        /// <summary>
        /// Subtracts a <see cref="Fraction"/> from a <see cref="Fraction"/>,
        /// yielding a new <see cref="Fraction"/>.
        /// </summary>
        /// <param name="a">The <see cref="Fraction"/> to subtract from (the minuend).</param>
        /// <param name="b">The <see cref="Fraction"/> to subtract (the subtrahend).</param>
        /// <returns>The <see cref="Fraction"/> that is the <c>a</c> minus <c>b</c>.</returns>
        public static Fraction operator -(Fraction a, Fraction b)
        {
            var negativeB = new Fraction(b.AbsoluteInteger, b.Numerator, b.Denominator, !b.IsNegative);
            return a + negativeB;
        }

        /// <summary>
        /// Computes the product of <c>a</c> and <c>b</c>, yielding a new <see cref="Fraction"/>.
        /// </summary>
        /// <param name="a">The <see cref="Fraction"/> to multiply.</param>
        /// <param name="b">The <see cref="Fraction"/> to multiply.</param>
        /// <returns>The <see cref="Fraction"/> that is the <c>a</c> * <c>b</c>.</returns>
        public static Fraction operator *(Fraction a, Fraction b)
        {
            var isNegative = a.IsNegative != b.IsNegative;

            var intPart = new Fraction(a.AbsoluteInteger * b.AbsoluteInteger, isNegative);
            var numeratorOnlyAPart = new Fraction(a.Numerator * b.AbsoluteInteger, a.Denominator, isNegative);
            var numeratorOnlyBPart = new Fraction(b.Numerator * a.AbsoluteInteger, b.Denominator, isNegative);
            var fractionalPart = new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator, isNegative);

            return (intPart + numeratorOnlyAPart + numeratorOnlyBPart + fractionalPart).Simplify();
        }

        /// <summary>
        /// Computes the division of <c>a</c> and <c>b</c>, yielding a new <see cref="Fraction"/>.
        /// </summary>
        /// <param name="a">The <see cref="Fraction"/> to divide (the divident).</param>
        /// <param name="b">The <see cref="Fraction"/> to divide (the divisor).</param>
        /// <returns>The <see cref="Fraction"/> that is the <c>a</c> / <c>b</c>.</returns>
        public static Fraction operator /(Fraction a, Fraction b)
        {
            // Division is the same as multiplication if you swap the numerator and denominator of one of the
            // operands.  This is easier to do if it's a vulgar fraction and includes no integer component.
            var vulgarB = b.Simplify(true);
            var invertedB = new Fraction(vulgarB.Denominator, vulgarB.Numerator, vulgarB.IsNegative);
            return a * invertedB;
        }

        #endregion

        #region Comparison

        /// <summary>
        /// Determines whether one specified <see cref="CSF.Fraction"/> is greater than another
        /// specfied <see cref="CSF.Fraction"/>.
        /// </summary>
        /// <param name="first">The first <see cref="CSF.Fraction"/> to compare.</param>
        /// <param name="second">The second <see cref="CSF.Fraction"/> to compare.</param>
        /// <returns><c>true</c> if <c>first</c> is greater than <c>second</c>; otherwise, <c>false</c>.</returns>
        public static bool operator >(Fraction first, Fraction second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Determines whether one specified <see cref="CSF.Fraction"/> is lower than another
        /// specfied <see cref="CSF.Fraction"/>.
        /// </summary>
        /// <param name="first">The first <see cref="CSF.Fraction"/> to compare.</param>
        /// <param name="second">The second <see cref="CSF.Fraction"/> to compare.</param>
        /// <returns><c>true</c> if <c>first</c> is lower than <c>second</c>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Fraction first, Fraction second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Determines whether one specified <see cref="CSF.Fraction"/> is greater than or equal to another
        /// specfied <see cref="CSF.Fraction"/>.
        /// </summary>
        /// <param name="first">The first <see cref="CSF.Fraction"/> to compare.</param>
        /// <param name="second">The second <see cref="CSF.Fraction"/> to compare.</param>
        /// <returns><c>true</c> if <c>first</c> is greater than or equal to <c>second</c>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Fraction first, Fraction second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Determines whether one specified <see cref="CSF.Fraction"/> is lower than or equal to another
        /// specfied <see cref="CSF.Fraction"/>.
        /// </summary>
        /// <param name="first">The first <see cref="CSF.Fraction"/> to compare.</param>
        /// <param name="second">The second <see cref="CSF.Fraction"/> to compare.</param>
        /// <returns><c>true</c> if <c>first</c> is lower than or equal to <c>second</c>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Fraction first, Fraction second) => first.CompareTo(second) <= 0;

        #endregion
    }
}
