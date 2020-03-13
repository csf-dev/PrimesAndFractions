//
// FractionFormatter.cs
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
    /// A formatting service for <see cref="Fraction"/> objects, which gets string representations of
    /// the fraction amount.
    /// </summary>
    public class FractionFormatter : IFormatsFraction
    {
        internal const string
            VulgarFormat = "v",
            StandardFormat = "s",
            StandardFormatWithoutSimplification = "S",
            LeadingZeroFormat = "z",
            LeadingZeroFormatWithoutSimplification = "Z";

        readonly ISimplifiesFraction simplifier;

        /// <summary>
        /// Gets a string representation of a <see cref="Fraction"/>, optionally
        /// using a format specifier.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the <paramref name="formatType"/> is <c>null</c> or <c>s</c> then the standard format is used.
        /// This will first <see cref="Fraction.Simplify(bool)"/> the fraction (using a parameter of
        /// <c>false</c>) before creating the formatted string.  If there is no whole-number component to the
        /// fraction (IE: its absolute value is less than one) then no leading zero will be displayed.
        /// For example <c>6/8</c> would become <c>3/4</c>.
        /// </para>
        /// <para>
        /// If the <paramref name="formatType"/> is <c>z</c> or <c>Z</c> then the result will be the same
        /// as using the standard format (above), except that a leading zero will be added, if there is no
        /// whole-number component to the fraction, for example <c>0 3/4</c>.
        /// Additionally, if an uppercase <c>Z</c> is used, then the fraction will not be simplified before
        /// formatting.  For example <c>6/8</c> would become <c>0 6/8</c>.
        /// </para>
        /// <para>
        /// If the <paramref name="formatType"/> is an uppercase <c>S</c> then the result will be the same
        /// as the standard format, except the fraction will not be simplified before it is formatted.
        /// For example <c>6/8</c> would remain <c>6/8</c>.
        /// </para>
        /// <para>
        /// If the <paramref name="formatType"/> is <c>v</c> then vulgar fractions will be preferred over
        /// composite (whole-number and fractions parts) representations.
        /// For example <c>3 3/4</c> would become <c>15/4</c>.
        /// </para>
        /// </remarks>
        /// <returns>The formatted string.</returns>
        /// <param name="fraction">The fraction to format.</param>
        /// <param name="formatType">
        /// An optional format type specifier, which (if provided) must be one of
        /// <c>v, s, S, z, Z</c>.
        /// </param>
        public string Format(Fraction fraction, string formatType = null)
        {
            Fraction simplified;

            switch(formatType)
            {
            case StandardFormatWithoutSimplification:
                return Format(fraction, false);
            case LeadingZeroFormatWithoutSimplification:
                return Format(fraction, true);
            case StandardFormat:
            case null:
                simplified = simplifier.Simplify(fraction);
                return Format(simplified, false);
            case LeadingZeroFormat:
                simplified = simplifier.Simplify(fraction);
                return Format(simplified, true);
            case VulgarFormat:
                simplified = simplifier.Simplify(fraction, true);
                return Format(simplified, false);
            default:
                throw new ArgumentException("The format type must be a supported value.", nameof(formatType));
            }
        }

        string Format(Fraction fraction, bool leadingZero)
        {
            var sign = fraction.IsNegative ? "-" : String.Empty;
            var integer = GetIntegerRepresentation(fraction, leadingZero);
            var spacer = (integer.Length > 0) ? " " : String.Empty;

            return $"{sign}{integer}{spacer}{fraction.Numerator}/{fraction.Denominator}";
        }

        string GetIntegerRepresentation(Fraction fraction, bool leadingZero)
        {
            if (fraction.AbsoluteInteger == 0L && leadingZero)
                return "0";

            if (fraction.AbsoluteInteger == 0L)
                return String.Empty;

            return fraction.AbsoluteInteger.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FractionFormatter"/> class.
        /// </summary>
        /// <param name="simplifier">A service which simplifies fractions.</param>
        public FractionFormatter(ISimplifiesFraction simplifier = null)
        {
            this.simplifier = simplifier ?? new FractionSimplifier();
        }
    }
}
