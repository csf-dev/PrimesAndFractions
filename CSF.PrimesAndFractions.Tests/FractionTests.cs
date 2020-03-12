//
// FractionTests.cs
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
    [TestFixture,Parallelizable(ParallelScope.None)]
    public class FractionTests
    {
        [Test]
        public void Constructor_can_create_a_fraction_from_an_integer()
        {
            var sut = new Fraction(5);
            Assert.That(sut.AbsoluteInteger, Is.EqualTo(5));
            Assert.That(sut.Numerator, Is.Zero);
            Assert.That(sut.Denominator, Is.EqualTo(1));
            Assert.That(sut.IsNegative, Is.False);
        }

        [Test]
        public void Constructor_can_create_a_fraction_from_a_negative_integer()
        {
            var sut = new Fraction(-5);
            Assert.That(sut.AbsoluteInteger, Is.EqualTo(5));
            Assert.That(sut.Numerator, Is.Zero);
            Assert.That(sut.Denominator, Is.EqualTo(1));
            Assert.That(sut.IsNegative, Is.True);
        }

        [Test]
        public void Constructor_can_create_a_fraction_from_an_integer_which_is_explicitly_negative()
        {
            var sut = new Fraction(5, true);
            Assert.That(sut.AbsoluteInteger, Is.EqualTo(5));
            Assert.That(sut.Numerator, Is.Zero);
            Assert.That(sut.Denominator, Is.EqualTo(1));
            Assert.That(sut.IsNegative, Is.True);
        }

        [Test]
        public void Constructor_ignores_sign_for_a_negative_integer_which_is_explicitly_positive()
        {
            var sut = new Fraction(-5, false);
            Assert.That(sut.AbsoluteInteger, Is.EqualTo(5));
            Assert.That(sut.Numerator, Is.Zero);
            Assert.That(sut.Denominator, Is.EqualTo(1));
            Assert.That(sut.IsNegative, Is.False);
        }

        [Test]
        public void Constructor_can_create_negative_fraction_for_a_negative_integer_which_is_explicitly_negative()
        {
            var sut = new Fraction(-5, true);
            Assert.That(sut.AbsoluteInteger, Is.EqualTo(5));
            Assert.That(sut.Numerator, Is.Zero);
            Assert.That(sut.Denominator, Is.EqualTo(1));
            Assert.That(sut.IsNegative, Is.True);
        }
    }
}
