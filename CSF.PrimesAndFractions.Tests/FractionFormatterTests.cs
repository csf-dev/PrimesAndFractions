//
// FractionFormatterTests.cs
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
    [TestFixture, Parallelizable]
    public class FractionFormatterTests
    {
        [Test]
        public void Format_can_format_a_normal_fraction_using_standard_format()
        {
            var sut = new FractionFormatter();
            var fraction = new Fraction(3, 4);
            Assert.That(() => sut.Format(fraction), Is.EqualTo("3/4"));
        }

        [Test]
        public void Format_can_format_a_composite_fraction_using_standard_format()
        {
            var sut = new FractionFormatter();
            var fraction = new Fraction(10, 3, 4);
            Assert.That(() => sut.Format(fraction), Is.EqualTo("10 3/4"));
        }

        [Test]
        public void Format_can_format_a_negative_composite_fraction_using_standard_format()
        {
            var sut = new FractionFormatter();
            var fraction = new Fraction(-10, 3, 4);
            Assert.That(() => sut.Format(fraction), Is.EqualTo("-10 3/4"));
        }

        [Test]
        public void Format_can_format_a_negative_fraction_using_standard_format()
        {
            var sut = new FractionFormatter();
            var fraction = new Fraction(-3, 4);
            Assert.That(() => sut.Format(fraction), Is.EqualTo("-3/4"));
        }

        [Test]
        public void Format_can_format_a_normal_fraction_using_leading_zero_format()
        {
            var sut = new FractionFormatter();
            var fraction = new Fraction(3, 4);
            Assert.That(() => sut.Format(fraction, "z"), Is.EqualTo("0 3/4"));
        }

        [Test]
        public void Format_can_format_a_negative_fraction_using_leading_zero_format()
        {
            var sut = new FractionFormatter();
            var fraction = new Fraction(-3, 4);
            Assert.That(() => sut.Format(fraction, "z"), Is.EqualTo("-0 3/4"));
        }

        [Test]
        public void Format_can_format_a_composite_fraction_as_a_vulgar_fraction()
        {
            var sut = new FractionFormatter();
            var fraction = new Fraction(10, 3, 4);
            Assert.That(() => sut.Format(fraction, "v"), Is.EqualTo("43/4"));
        }

        [Test]
        public void Format_simplifies_the_fraction_if_the_format_specifier_indicates_it_should([Values("v", "s", "z", null)] string format)
        {
            var sut = new FractionFormatter();
            var fraction = new Fraction(6, 8);
            Assert.That(() => sut.Format(fraction, format), Contains.Substring("3/4"));
        }

        [Test]
        public void Format_does_not_simplift_the_fraction_if_the_format_specifier_indicates_it_shouldnt([Values("S", "Z")] string format)
        {
            var sut = new FractionFormatter();
            var fraction = new Fraction(6, 8);
            Assert.That(() => sut.Format(fraction, format), Contains.Substring("6/8"));
        }
    }
}
