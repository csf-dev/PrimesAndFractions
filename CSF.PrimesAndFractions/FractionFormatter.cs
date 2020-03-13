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
    public class FractionFormatter : IFormatsFraction
    {
        internal const string
            VulgarFormat = "v",
            StandardFormat = "s",
            StandardFormatWithoutSimplification = "S",
            LeadingZeroFormat = "z",
            LeadingZeroFormatWithoutSimplification = "Z";

        readonly ISimplifiesFraction simplifier;

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
            var spacer = (sign.Length > 0 || integer.Length > 0) ? " " : String.Empty;

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

        public FractionFormatter(ISimplifiesFraction simplifier)
        {
            this.simplifier = simplifier ?? throw new ArgumentNullException(nameof(simplifier));
        }
    }
}
