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
        public static bool operator ==(Fraction a, Fraction b) => a.Equals(b);

        public static bool operator !=(Fraction a, Fraction b) => !(a == b);

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

        public static Fraction operator -(Fraction a, Fraction b)
        {
            var negativeB = new Fraction(b.AbsoluteInteger, b.Numerator, b.Denominator, !b.IsNegative);
            return a + negativeB;
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            var isNegative = a.IsNegative != b.IsNegative;

            var intPart = new Fraction(a.AbsoluteInteger * b.AbsoluteInteger, isNegative);
            var numeratorOnlyAPart = new Fraction(a.Numerator * b.AbsoluteInteger, a.Denominator, isNegative);
            var numeratorOnlyBPart = new Fraction(b.Numerator * a.AbsoluteInteger, b.Denominator, isNegative);
            var fractionalPart = new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator, isNegative);

            return (intPart + numeratorOnlyAPart + numeratorOnlyBPart + fractionalPart).Simplify();
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            // Division is the same as multiplication if you swap the numerator and denominator of one of the
            // operands.  This is easier to do if it's a vulgar fraction and includes no integer component.
            var vulgarB = b.Simplify(true);
            var invertedB = new Fraction(vulgarB.Denominator, vulgarB.Numerator, vulgarB.IsNegative);
            return a * invertedB;
        }
    }
}
