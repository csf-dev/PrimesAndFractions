//
// FractionSimplifier.cs
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
using System.Linq;

namespace CSF
{
    /// <summary>
    /// An object which simplifies fractions by dividing the numerator and denominator by
    /// the prime factors which are common to both.
    /// </summary>
    public class FractionSimplifier : ISimplifiesFraction
    {
        readonly IGetsCommonPrimeFactors factoriser;

        /// <summary>
        /// Simplify the specified fraction, optionally preferring the use of vulgar fractions.
        /// </summary>
        /// <returns>The simplified fraction.</returns>
        /// <param name="input">The fraction to simplify.</param>
        /// <param name="preferVulgarFraction">If set to <c>true</c> then vulgar fractions are preferred over composite/mixed whole-number and fractional parts.</param>
        public Fraction Simplify(Fraction input, bool preferVulgarFraction = false)
        {
            // We can perform an optimisation if the numerator is zero
            if (input.Numerator == 0 && !preferVulgarFraction)
                return new Fraction(input.AbsoluteInteger, input.IsNegative);
            if (input.Numerator == 0)
                return new Fraction(0, input.AbsoluteInteger, 1, input.IsNegative);

            var primeFactors = factoriser.GetPrimeFactors(input.Numerator, input.Denominator);
            var highestCommonFactor = primeFactors.Aggregate(1L, (acc, next) => acc * next);

            var integer = preferVulgarFraction ? 0L : (input.AbsoluteInteger + (long) Math.Floor((double) input.Numerator / (double) input.Denominator));
            var numerator = input.Numerator / highestCommonFactor;
            var denominator = input.Denominator/ highestCommonFactor;

            if (preferVulgarFraction)
                numerator = numerator + input.AbsoluteInteger * denominator;
            else
                numerator = numerator % denominator;

            return new Fraction(integer, numerator, denominator, input.IsNegative);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FractionSimplifier"/> class.
        /// </summary>
        /// <param name="factoriser">Factoriser.</param>
        public FractionSimplifier(IGetsCommonPrimeFactors factoriser = null)
        {
            this.factoriser = factoriser ?? CommonPrimeFactorProvider.Default;
        }
    }
}
