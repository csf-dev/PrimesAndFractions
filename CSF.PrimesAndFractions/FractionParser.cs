//
// FractionParser.cs
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
using System.Text.RegularExpressions;

namespace CSF
{
    public class FractionParser : IParsesFraction
    {
        const string FractionPattern = @"^\s*(-)?\s*(\d+)?\s+(\d+)\s*/\s*(\d+)\s*$";
        static readonly Regex FractionMatcher = new Regex(FractionPattern, RegexOptions.CultureInvariant);

        public Fraction Parse(string fractionString)
        {
            if (fractionString == null)
                throw new ArgumentNullException(nameof(fractionString));
            var match = FractionMatcher.Match(fractionString);

            if (!match.Success)
                throw new FormatException("The specified string must be a fraction in string format.");

            var negativeVal = match.Groups[1].Value;
            var integerVal = match.Groups[2].Value;
            var numeratorVal = match.Groups[3].Value;
            var denominatorVal = match.Groups[4].Value;

            var isNegative = negativeVal.Length > 0;
            var integer = (integerVal.Length > 0) ? Int64.Parse(integerVal) : 0L;
            var numerator = Int64.Parse(numeratorVal);
            var denominator = Int64.Parse(denominatorVal);

            return new Fraction(integer, numerator, denominator, isNegative);
        }
    }
}
