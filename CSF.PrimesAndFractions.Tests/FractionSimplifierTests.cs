//
// FractionSimplifierTests.cs
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
using NUnit.Framework;

namespace CSF.Tests
{
    [TestFixture,Parallelizable]
    public class FractionSimplifierTests
    {
        [Test]
        public void Simplify_can_correctly_simplify_a_small_fraction()
        {
            var sut = new FractionSimplifier();
            var fraction = new Fraction(6, 8);
            var simplified = sut.Simplify(fraction);
            Assert.That((simplified.Numerator, simplified.Denominator), Is.EqualTo((3, 4)));
        }

        [Test]
        public void Simplify_does_not_change_an_already_simplified_fraction()
        {
            var sut = new FractionSimplifier();
            var fraction = new Fraction(3, 4);
            var simplified = sut.Simplify(fraction);
            Assert.That((simplified.Numerator, simplified.Denominator), Is.EqualTo((3, 4)));
        }

        [Test]
        public void Simplify_can_correctly_simplify_a_negative_fraction()
        {
            var sut = new FractionSimplifier();
            var fraction = new Fraction(-6, 8);
            var simplified = sut.Simplify(fraction);
            Assert.That((simplified.Numerator, simplified.Denominator, simplified.IsNegative), Is.EqualTo((3, 4, true)));
        }

        [Test]
        public void Simplify_can_correctly_simplify_a_vulgar_fraction()
        {
            var sut = new FractionSimplifier();
            var fraction = new Fraction(12, 8);
            var simplified = sut.Simplify(fraction);
            Assert.That((simplified.AbsoluteInteger, simplified.Numerator, simplified.Denominator), Is.EqualTo((1, 1, 2)));
        }

        [Test]
        public void Simplify_can_correctly_create_a_vulgar_fraction()
        {
            var sut = new FractionSimplifier();
            var fraction = new Fraction(1, 4, 8);
            var simplified = sut.Simplify(fraction, true);
            Assert.That((simplified.AbsoluteInteger, simplified.Numerator, simplified.Denominator), Is.EqualTo((0, 3, 2)));
        }
    }
}
