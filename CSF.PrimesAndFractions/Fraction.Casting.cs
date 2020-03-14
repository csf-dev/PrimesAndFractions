//
// Fraction.Casting.cs
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
        /// <summary>
        /// Explicitly casts the fraction to a decimal number.
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator decimal(Fraction f) => f.ToDecimal();

        /// <summary>
        /// Explicitly casts the fraction to a double-precision floating point number.
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator double(Fraction f) => f.ToDouble();

        /// <summary>
        /// Explicitly casts the fraction to a single-precision floating point number.
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator float(Fraction f) => f.ToSingle();

        /// <summary>
        /// Explicitly casts the fraction to a signed byte.
        /// Note that this cast causes loss of data; only the whole-number part
        /// will be retained.
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator sbyte(Fraction f) => (sbyte) (f.Simplify(false).AbsoluteInteger * f.Multiplier);

        /// <summary>
        /// Explicitly casts the fraction to a signed 16-bit integer.
        /// Note that this cast causes loss of data; only the whole-number part
        /// will be retained.
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator short(Fraction f) => (short) (f.Simplify(false).AbsoluteInteger * f.Multiplier);

        /// <summary>
        /// Explicitly casts the fraction to a signed 32-bit integer.
        /// Note that this cast causes loss of data; only the whole-number part
        /// will be retained.
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator int(Fraction f) => (int) f.Simplify(false).AbsoluteInteger * (int) f.Multiplier;

        /// <summary>
        /// Explicitly casts the fraction to a signed 64-bit integer.
        /// Note that this cast causes loss of data; only the whole-number part
        /// will be retained.
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator long(Fraction f) => f.Simplify(false).AbsoluteInteger * f.Multiplier;

        /// <summary>
        /// Explicitly casts the fraction to an unsigned byte.
        /// Note that this cast causes loss of data; only the whole-number part
        /// will be retained and only the absolute value (negative numbers will be
        /// converted to positive).
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator byte(Fraction f) => (byte) f.Simplify(false).AbsoluteInteger;

        /// <summary>
        /// Explicitly casts the fraction to an unsigned 16-bit integer.
        /// Note that this cast causes loss of data; only the whole-number part
        /// will be retained and only the absolute value (negative numbers will be
        /// converted to positive).
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator ushort(Fraction f) => (ushort) f.Simplify(false).AbsoluteInteger;

        /// <summary>
        /// Explicitly casts the fraction to an unsigned 32-bit integer.
        /// Note that this cast causes loss of data; only the whole-number part
        /// will be retained and only the absolute value (negative numbers will be
        /// converted to positive).
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator uint(Fraction f) => (uint) f.Simplify(false).AbsoluteInteger;

        /// <summary>
        /// Explicitly casts the fraction to an unsigned 64-bit integer.
        /// Note that this cast causes loss of data; only the whole-number part
        /// will be retained and only the absolute value (negative numbers will be
        /// converted to positive).
        /// </summary>
        /// <returns>The value of the fraction.</returns>
        /// <param name="f">The fraction to cast.</param>
        public static explicit operator ulong(Fraction f) => (ulong) f.Simplify(false).AbsoluteInteger;

        /// <summary>
        /// Implicitly casts a signed byte as a fraction.
        /// </summary>
        /// <returns>A fraction with the same value as the input number.</returns>
        /// <param name="n">The number to cast.</param>
        public static implicit operator Fraction(sbyte n) => new Fraction(n);

        /// <summary>
        /// Implicitly casts a signed 16-bit integer as a fraction.
        /// </summary>
        /// <returns>A fraction with the same value as the input number.</returns>
        /// <param name="n">The number to cast.</param>
        public static implicit operator Fraction(short n) => new Fraction(n);

        /// <summary>
        /// Implicitly casts a signed 32-bit integer as a fraction.
        /// </summary>
        /// <returns>A fraction with the same value as the input number.</returns>
        /// <param name="n">The number to cast.</param>
        public static implicit operator Fraction(int n) => new Fraction(n);

        /// <summary>
        /// Implicitly casts a signed 64-bit integer as a fraction.
        /// </summary>
        /// <returns>A fraction with the same value as the input number.</returns>
        /// <param name="n">The number to cast.</param>
        public static implicit operator Fraction(long n) => new Fraction(n);

        /// <summary>
        /// Implicitly casts an unsigned byte as a fraction.
        /// </summary>
        /// <returns>A fraction with the same value as the input number.</returns>
        /// <param name="n">The number to cast.</param>
        public static implicit operator Fraction(byte n) => new Fraction(n);

        /// <summary>
        /// Implicitly casts an unsigned 16-bit integer as a fraction.
        /// </summary>
        /// <returns>A fraction with the same value as the input number.</returns>
        /// <param name="n">The number to cast.</param>
        public static implicit operator Fraction(ushort n) => new Fraction(n);

        /// <summary>
        /// Implicitly casts an unsigned 32-bit integer as a fraction.
        /// </summary>
        /// <returns>A fraction with the same value as the input number.</returns>
        /// <param name="n">The number to cast.</param>
        public static implicit operator Fraction(uint n) => new Fraction(n);

        /// <summary>
        /// Explicitly casts an unsigned 64-bit integer as a fraction.
        /// Note that this cast could cause loss of data, if the input number is greater
        /// than <see cref="Int64.MaxValue"/>.
        /// </summary>
        /// <returns>A fraction with the same value as the input number.</returns>
        /// <param name="n">The number to cast.</param>
        public static explicit operator Fraction(ulong n) => new Fraction((long) n);
    }
}
