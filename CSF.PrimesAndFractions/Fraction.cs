//
// Fraction.cs
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
    /// <summary>
    /// A type which represents a fractional number.  This may contain a whole number (intger) part
    /// and also a fractional (numerator / denominator) part.  The fractional part may be a vulgar
    /// fraction (and could thus represent more than a whole number itself).  Fractions may also be
    /// negative.
    /// </summary>
    public partial struct Fraction :  IEquatable<Fraction>
    {
        // An arbitrary prime number for the purpose of deriving a hash code
        const int hashPrime = 137;

        internal static IParsesFraction Parser;
        internal static IFormatsFraction Formatter;
        internal static ISimplifiesFraction Simplifier;

        long Multiplier => IsNegative ? -1L : 1L;

        /// <summary>
        /// Gets a value which indicates whether or not the current instance represents a negative amount.
        /// </summary>
        /// <value><c>true</c> if this instance is negative; otherwise, <c>false</c>.</value>
        public bool IsNegative { get; }

        /// <summary>
        /// <para>
        /// Gets the absolute value of the "whole number" part of the current fraction instance.
        /// </para>
        /// <para>
        /// Note that - because the fraction could be a vulgar fraction, this property may not contain
        /// the entire whole number component of the current instance.
        /// Use <see cref="Simplify(bool)"/> with a value of <c>false</c> to get an instance which ensures
        /// that this property contains the complete whole number part.
        /// </para>
        /// </summary>
        /// <value>The part of the fraction which is a whole number.</value>
        public long AbsoluteInteger { get; }

        /// <summary>
        /// Gets the fraction numerator (the top part).  Note that this could be higher than the
        /// <see cref="Denominator"/>, making this a vulgar fraction.
        /// See also: <seealso cref="IsVulgarFraction"/>
        /// </summary>
        /// <value>The numerator.</value>
        public long Numerator { get; }

        /// <summary>
        /// Gets the fraction denominator (the bottom part).  Note that this could be lower than the
        /// <see cref="Numerator"/>, making this a vulgar fraction.
        /// See also: <seealso cref="IsVulgarFraction"/>
        /// </summary>
        /// <value>The numerator.</value>
        public long Denominator { get; }

        /// <summary>
        /// Gets a value which indicates whether the current instance is a vulgar fraction: one in which the
        /// <see cref="Numerator"/> is greater than the <see cref="Denominator"/>.
        /// </summary>
        /// <value><c>true</c> if this is vulgar fraction; otherwise, <c>false</c>.</value>
        public bool IsVulgarFraction => Numerator > Denominator;

        /// <summary>
        /// Gets a <see cref="Fraction"/> which is equal to the current fraction, reduced to its simplest
        /// form.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When simplifying a fraction, the overall goal is to reduce the <see cref="Denominator"/>
        /// to the lowest possible value.  This is accomplished by dividing both the <see cref="Numerator"/>
        /// and the <see cref="Denominator"/> by their highest common factor.
        /// For example the fraction "6/8" could be simplified, because both 6 &amp; 8 share 2 as a common
        /// factor.  The simplified version would be "3/4".
        /// </para>
        /// <para>
        /// Once the denominator has been reduced to the smallest possible value, the <see cref="Numerator"/>
        /// and <see cref="AbsoluteInteger"/> are processed using the following rules.
        /// </para>
        /// <para>
        /// If <paramref name="preferVulgarFraction"/> is <c>false</c> then the numerator is reduced to the
        /// smallest possible value, by transferring all whole-number components of the value to the
        /// <see cref="AbsoluteInteger"/>.  For example this would mean that "9/4" (nine quarters) would
        /// become "2 1/4" (two, and one quarter).  This will always leave the numerator as a number that is
        /// lower than the denominator.
        /// </para>
        /// <para>
        /// On the other hand, if <paramref name="preferVulgarFraction"/> is <c>true</c> then the reverse will
        /// happen.  The <see cref="AbsoluteInteger"/> will be reduced to zero by transferring its value to the
        /// <see cref="Numerator"/>, even if this results in the numerator being greater than the
        /// <see cref="Denominator"/>.  For example "3 4/5" (three, and four-fifths) would become "19/5"
        /// (nineteen fifths).  This form of fraction (with a numerator greater than the denominator) may be
        /// called a "vulgar fraction".
        /// </para>
        /// </remarks>
        /// <returns>The simplified version of the current fraction.</returns>
        /// <param name="preferVulgarFraction">If set to <c>true</c> then the simplification process will prefer the use of vulgar fractions, over presenting a mixture of whole numbers and fractional amounts.</param>
        public Fraction Simplify(bool preferVulgarFraction = false)
            => Simplifier.Simplify(this, preferVulgarFraction);

        /// <summary>
        /// Gets a representation of the value of the current instance as a <c>System.Decimal</c>.
        /// </summary>
        /// <returns>The decimal equivalent of the current fraction.</returns>
        public decimal ToDecimal()
        {
            var absoluteValue = AbsoluteInteger + ((decimal)Numerator / (decimal)Denominator);
            return absoluteValue * Multiplier;
        }

        /// <summary>
        /// Gets a representation of the value of the current instance as a <c>System.Double</c>.
        /// </summary>
        /// <returns>The double-precision floating point equivalent of the current fraction.</returns>
        public double ToDouble()
        {
            var absoluteValue = AbsoluteInteger + ((double)Numerator / (double)Denominator);
            return absoluteValue * Multiplier;
        }

        /// <summary>
        /// Gets a representation of the value of the current instance as a <c>System.Single</c>.
        /// </summary>
        /// <returns>The single-precision floating point equivalent of the current fraction.</returns>
        public float ToSingle()
        {
            var absoluteValue = AbsoluteInteger + ((float)Numerator / (float)Denominator);
            return absoluteValue * Multiplier;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Fraction"/> is equal to the current
        /// <see cref="Fraction"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When comparing fractions, the simplified forms of the fractions are used for comparison. This means
        /// that equivalent fractions (which could simplify to become identical) will be considered equal by
        /// this method.
        /// See also: <seealso cref="Simplify(bool)"/>
        /// </para>
        /// </remarks>
        /// <param name="other">
        /// The <see cref="Fraction"/> to compare with the current <see cref="Fraction"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Fraction"/> is equal to the current
        /// <see cref="Fraction"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Fraction other)
        {
            // As an optimisation, if the fractions are equal without
            // simplification then we can return true immediately.
            if (   IsNegative == other.IsNegative
                && AbsoluteInteger == other.AbsoluteInteger
                && Denominator == other.Denominator
                && Numerator == other.Numerator)
            {
                return true;
            }

            // Simplifying both fractions will ensure that if they are equal,
            // they will have the exact same values in their four stateful properties.
            var thisSimplified = Simplify(false);
            var otherSimplified = other.Simplify(false);

            return thisSimplified.IsNegative == otherSimplified.IsNegative
                && thisSimplified.AbsoluteInteger == otherSimplified.AbsoluteInteger
                && thisSimplified.Numerator == otherSimplified.Numerator
                && thisSimplified.Denominator == otherSimplified.Denominator;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current
        /// <see cref="Fraction"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="object"/> to compare with the current <see cref="Fraction"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Fraction"/>; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Fraction fraction) return Equals(fraction);
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Fraction"/> object.
        /// </summary>
        /// <returns>
        /// A hash code for this instance that is suitable for use in hashing algorithms
        /// and data structures such as a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            var simplified = Simplify();

            unchecked
            {
                return (simplified.AbsoluteInteger.GetHashCode() * hashPrime)
                     ^ (simplified.Numerator.GetHashCode() * hashPrime)
                     ^ (simplified.Denominator.GetHashCode() * hashPrime)
                     ^ (simplified.IsNegative.GetHashCode() * hashPrime);
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="Fraction"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> that represents the current <see cref="Fraction"/>.
        /// </returns>
        public override string ToString()
            => Formatter.Format(this, FractionFormatter.StandardFormatWithoutSimplification);

        /// <summary>
        /// Executes logic, used during instance construction, to determine whether or not the parameters
        /// indicate a negative fraction or not.
        /// </summary>
        /// <returns><c>true</c>, if the paramteters indicate a negative fraction, <c>false</c> otherwise.</returns>
        /// <param name="absoluteInteger">The absolute value of the whole-number component of this number.</param>
        /// <param name="numerator">The absolute value of the numerator (top part of the fraction).</param>
        /// <param name="denominator">The absolute value of the denominator (bottom part of the fraction).</param>
        /// <param name="isNegative">A value indicating whether or not this number is negative.</param>
        bool GetIsNegative(long absoluteInteger, long numerator, long denominator, bool? isNegative)
        {
            if (isNegative.HasValue) return isNegative.Value;

            if (absoluteInteger < 0) return true;
            if (absoluteInteger > 0) return false;

            if (numerator < 0 && denominator > 0) return true;
            if (numerator > 0 && denominator < 0) return true;

            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fraction"/> struct.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The parameters <paramref name="absoluteInteger"/>, <paramref name="numerator"/> and
        /// <paramref name="denominator"/> should usually be specified as absolute values.  For negative
        /// fractions, set <paramref name="isNegative"/> to <c>true</c>.
        /// </para>
        /// <para>
        /// It is possible, however, to create negative fractions without using the <paramref name="isNegative"/>
        /// parameter.  If <paramref name="isNegative"/> is <c>null</c> and <paramref name="absoluteInteger"/>
        /// is negative then this fraction is assumed to be a negative one. Likewise, if
        /// <paramref name="isNegative"/> is <c>null</c>, <paramref name="absoluteInteger"/> is zero (and
        /// only zero) and either (but not both) of <paramref name="numerator"/> or
        /// <paramref name="denominator"/> is negative then again, this fraction is assumed to be negative.
        /// </para>
        /// </remarks>
        /// <param name="absoluteInteger">The absolute value of the whole-number component of this number.</param>
        /// <param name="numerator">The absolute value of the numerator (top part of the fraction).</param>
        /// <param name="denominator">The absolute value of the denominator (bottom part of the fraction).</param>
        /// <param name="isNegative">An optional value indicating whether or not this number is negative.</param>
        public Fraction(long absoluteInteger, long numerator, long denominator, bool? isNegative = null)
        {
            if (denominator == 0)
                throw new ArgumentOutOfRangeException(nameof(denominator), $"The denominator of a {nameof(Fraction)} cannot be zero.");

            AbsoluteInteger = Math.Abs(absoluteInteger);
            Numerator = Math.Abs(numerator);
            Denominator = Math.Abs(denominator);
            IsNegative = false;

            IsNegative = GetIsNegative(absoluteInteger, numerator, denominator, isNegative);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fraction"/> struct.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The parameters <paramref name="numerator"/> and
        /// <paramref name="denominator"/> should usually be specified as absolute values.  For negative
        /// fractions, set <paramref name="isNegative"/> to <c>true</c>.
        /// </para>
        /// <para>
        /// It is possible, however, to create negative fractions without using the <paramref name="isNegative"/>
        /// parameter.  If <paramref name="isNegative"/> is <c>null</c> and either (but not both) of
        /// <paramref name="numerator"/> or <paramref name="denominator"/> is negative then this fraction
        /// is assumed to be negative.
        /// </para>
        /// </remarks>
        /// <param name="numerator">The absolute value of the numerator (top part of the fraction).</param>
        /// <param name="denominator">The absolute value of the denominator (bottom part of the fraction).</param>
        /// <param name="isNegative">An optional value indicating whether or not this number is negative.</param>
        public Fraction(long numerator, long denominator, bool? isNegative = null)
            : this(0, numerator, denominator, isNegative) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fraction"/> struct.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The parameter <paramref name="absoluteInteger"/> may be specified as absolute value.
        /// To create a negative fraction, set <paramref name="isNegative"/> to <c>true</c>.
        /// </para>
        /// <para>
        /// It is possible, however, to create such a fraction without using the <paramref name="isNegative"/>
        /// parameter.  If <paramref name="isNegative"/> is <c>null</c> and <paramref name="absoluteInteger"/>
        /// is negative then this fraction is assumed to be a negative one.
        /// </para>
        /// </remarks>
        /// <param name="absoluteInteger">The absolute value of the whole-number component of this number.</param>
        /// <param name="isNegative">An optional value indicating whether or not this number is negative.</param>
        public Fraction(long absoluteInteger, bool? isNegative = null)
            : this(absoluteInteger, 0, 1, isNegative) { }

        /// <summary>
        /// Initializes the <see cref="Fraction"/> struct.
        /// </summary>
        static Fraction()
        {
            ResetServices();
        }

        /// <summary>
        /// Resets the internal services which are used by the <see cref="Fraction"/> class for
        /// parsing, simplification and formatting.
        /// </summary>
        internal static void ResetServices()
        {
            Parser = new FractionParser();
            Simplifier = new FractionSimplifier();
            Formatter = new FractionFormatter(Simplifier);
        }

        /// <summary>
        /// Parses a specified string which represents a formatted fraction value and returns a
        /// <see cref="Fraction"/> created from that string.
        /// </summary>
        /// <returns>The parsed fraction.</returns>
        /// <param name="fractionString">A string which represents a fraction.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="fractionString"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If the <paramref name="fractionString"/> is not a correctly-formatted string that represents a fraction.</exception>
        public static Fraction Parse(string fractionString) => Parser.Parse(fractionString);
    }
}
