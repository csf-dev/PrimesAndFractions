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
    public partial struct Fraction :  IEquatable<Fraction>
    {
        // An arbitrary prime number for the purpose of deriving a hash code
        const int hashPrime = 137;

        internal static IParsesFraction Parser;
        internal static IFormatsFraction Formatter;
        internal static ISimplifiesFraction Simplifier;

        long Multiplier => IsNegative ? -1L : 1L;

        public bool IsNegative { get; }

        public long AbsoluteInteger { get; }

        public long Numerator { get; }

        public long Denominator { get; }

        public Fraction Simplify(bool preferVulgarFraction = false)
            => Simplifier.Simplify(this, preferVulgarFraction);

        public decimal ToDecimal()
        {
            var absoluteValue = AbsoluteInteger + ((decimal)Numerator / (decimal)Denominator);
            return absoluteValue * Multiplier;
        }

        public double ToDouble()
        {
            var absoluteValue = AbsoluteInteger + ((double)Numerator / (double)Denominator);
            return absoluteValue * Multiplier;
        }

        public float ToSingle()
        {
            var absoluteValue = AbsoluteInteger + ((float)Numerator / (float)Denominator);
            return absoluteValue * Multiplier;
        }

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

        public override bool Equals(object obj)
        {
            if (obj is Fraction fraction) return Equals(fraction);
            return false;
        }

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

        public override string ToString()
            => Formatter.Format(this, FractionFormatter.StandardFormatWithoutSimplification);

        bool GetIsNegative(long absoluteInteger, long numerator, long denominator, bool? isNegative)
        {
            if (isNegative.HasValue) return isNegative.Value;

            if (absoluteInteger < 0) return true;
            if (absoluteInteger > 0) return false;

            if (numerator < 0 && denominator > 0) return true;
            if (numerator > 0 && denominator < 0) return true;

            return false;
        }

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

        public Fraction(long numerator, long denominator, bool? isNegative = null)
            : this(0, numerator, denominator, isNegative) { }

        public Fraction(long absoluteInteger, bool? isNegative = null)
            : this(absoluteInteger, 0, 1, isNegative) { }

        static Fraction()
        {
            ResetServices();
        }

        internal static void ResetServices()
        {
            Parser = new FractionParser();
            Simplifier = new FractionSimplifier();
            Formatter = new FractionFormatter(Simplifier);
        }

        public static Fraction Parse(string fractionString) => Parser.Parse(fractionString);
    }
}
