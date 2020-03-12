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
using Moq;
using NUnit.Framework;

namespace CSF.Tests
{
    [TestFixture,Parallelizable(ParallelScope.None)]
    public class FractionTests
    {
        [SetUp]
        public void Setup()
        {
            Fraction.ResetServices();
        }

        #region constructors

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

        #endregion

        #region Parse

        [Test,AutoMoqData]
        public void Parse_uses_parser_service(IParsesFraction parser,
                                              string fractionString,
                                              Fraction fraction)
        {
            Fraction.Parser = parser;
            Mock.Get(parser).Setup(x => x.Parse(fractionString)).Returns(fraction);
            Assert.That(() => Fraction.Parse(fractionString), Is.EqualTo(fraction));
        }

        #endregion

        #region Simplify

        [Test, AutoMoqData]
        public void Simplify_uses_simplification_service(ISimplifiesFraction simplifier,
                                                         Fraction fraction,
                                                         Fraction expected)
        {
            Fraction.Simplifier = simplifier;
            Mock.Get(simplifier).Setup(x => x.Simplify(It.IsAny<Fraction>(), It.IsAny<bool>())).Returns(expected);
            Assert.That(() => fraction.Simplify(), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void Simplify_passes_correct_parameters_to_service(ISimplifiesFraction simplifier,
                                                                  Fraction fraction,
                                                                  bool vulgar,
                                                                  Fraction expected)
        {
            Fraction.Simplifier = simplifier;
            Mock.Get(simplifier).Setup(x => x.Simplify(It.IsAny<Fraction>(), It.IsAny<bool>())).Returns(expected);
            fraction.Simplify();
            Mock.Get(simplifier).Verify(x => x.Simplify(fraction, vulgar), Times.Once);
        }

        #endregion

        #region IsVulgarFraction

        [Test]
        public void IsVulgarFraction_returns_correct_value_for_a_vulgar_fraction()
        {
            var sut = new Fraction(5, 4);
            Assert.That(sut.IsVulgarFraction, Is.True);
        }

        [Test]
        public void IsVulgarFraction_returns_correct_value_for_a_non_vulgar_fraction()
        {
            var sut = new Fraction(5, 6);
            Assert.That(sut.IsVulgarFraction, Is.False);
        }

        [Test]
        public void IsVulgarFraction_returns_correct_value_for_a_vulgar_fraction_with_an_integer_component()
        {
            var sut = new Fraction(2, 5, 4);
            Assert.That(sut.IsVulgarFraction, Is.True);
        }

        [Test]
        public void IsVulgarFraction_returns_correct_value_for_a_non_vulgar_fraction_with_an_integer_component()
        {
            var sut = new Fraction(2, 5, 6);
            Assert.That(sut.IsVulgarFraction, Is.False);
        }

        [Test]
        public void IsVulgarFraction_returns_correct_value_for_a_negative_vulgar_fraction_with_an_integer_component()
        {
            var sut = new Fraction(2, 5, 4, true);
            Assert.That(sut.IsVulgarFraction, Is.True);
        }

        [Test]
        public void IsVulgarFraction_returns_correct_value_for_a_negative_non_vulgar_fraction_with_an_integer_component()
        {
            var sut = new Fraction(2, 5, 6, true);
            Assert.That(sut.IsVulgarFraction, Is.False);
        }

        #endregion

        #region To Decimal, Double and Single

        [Test]
        public void ToDecimal_gets_correct_value_for_positive_fraction()
        {
            var sut = new Fraction(3, 4);
            Assert.That(() => sut.ToDecimal(), Is.EqualTo(0.75m));
        }

        [Test]
        public void ToDecimal_gets_correct_value_for_positive_fraction_with_integer()
        {
            var sut = new Fraction(5, 3, 4);
            Assert.That(() => sut.ToDecimal(), Is.EqualTo(5.75m));
        }

        [Test]
        public void ToDecimal_gets_correct_value_for_negative_fraction()
        {
            var sut = new Fraction(3, 4, true);
            Assert.That(() => sut.ToDecimal(), Is.EqualTo(-0.75m));
        }

        [Test]
        public void ToDecimal_gets_correct_value_for_negative_fraction_with_integer()
        {
            var sut = new Fraction(5, 3, 4, true);
            Assert.That(() => sut.ToDecimal(), Is.EqualTo(-5.75m));
        }

        [Test]
        public void ToDouble_gets_correct_value_for_positive_fraction()
        {
            var sut = new Fraction(3, 4);
            Assert.That(() => sut.ToDouble(), Is.EqualTo(0.75d));
        }

        [Test]
        public void ToDouble_gets_correct_value_for_positive_fraction_with_integer()
        {
            var sut = new Fraction(5, 3, 4);
            Assert.That(() => sut.ToDouble(), Is.EqualTo(5.75d));
        }

        [Test]
        public void ToDouble_gets_correct_value_for_negative_fraction()
        {
            var sut = new Fraction(3, 4, true);
            Assert.That(() => sut.ToDouble(), Is.EqualTo(-0.75d));
        }

        [Test]
        public void ToDouble_gets_correct_value_for_negative_fraction_with_integer()
        {
            var sut = new Fraction(5, 3, 4, true);
            Assert.That(() => sut.ToDouble(), Is.EqualTo(-5.75d));
        }

        [Test]
        public void ToSingle_gets_correct_value_for_positive_fraction()
        {
            var sut = new Fraction(3, 4);
            Assert.That(() => sut.ToSingle(), Is.EqualTo(0.75));
        }

        [Test]
        public void ToSingle_gets_correct_value_for_positive_fraction_with_integer()
        {
            var sut = new Fraction(5, 3, 4);
            Assert.That(() => sut.ToSingle(), Is.EqualTo(5.75));
        }

        [Test]
        public void ToSingle_gets_correct_value_for_negative_fraction()
        {
            var sut = new Fraction(3, 4, true);
            Assert.That(() => sut.ToSingle(), Is.EqualTo(-0.75));
        }

        [Test]
        public void ToSingle_gets_correct_value_for_negative_fraction_with_integer()
        {
            var sut = new Fraction(5, 3, 4, true);
            Assert.That(() => sut.ToSingle(), Is.EqualTo(-5.75));
        }

        #endregion

        #region Equals and GetHashCode

        [Test, AutoMoqData]
        public void GetHashCode_returns_hash_of_simplified_fraction(Fraction sut,
                                                                    ISimplifiesFraction simplifier,
                                                                    long integer,
                                                                    long numerator,
                                                                    long denominator,
                                                                    bool negative)
        {
            Fraction.Simplifier = simplifier;
            var expected = new Fraction(integer, numerator, denominator, negative);
            Mock.Get(simplifier).Setup(x => x.Simplify(sut, false)).Returns(expected);

            int expectedHashCode;

            // This is the expected algorithm to use, although it depends upon the runtime's mechanism
            // for getting the hash codes of the four component values.
            unchecked
            {
                expectedHashCode = (integer.GetHashCode() * 137)
                                 ^ (numerator.GetHashCode() * 137)
                                 ^ (denominator.GetHashCode() * 137)
                                 ^ (negative.GetHashCode() * 137);
            }

            Assert.That(() => sut.GetHashCode(), Is.EqualTo(expectedHashCode));
        }

        [Test]
        public void Equals_returns_true_for_two_identical_positive_fractions()
        {
            var fraction1 = new Fraction(3, 4);
            var fraction2 = new Fraction(3, 4);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_true_for_two_equivalent_positive_fractions()
        {
            var fraction1 = new Fraction(3, 4);
            var fraction2 = new Fraction(6, 8);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_true_for_two_identical_positive_fractions_with_integer_components()
        {
            var fraction1 = new Fraction(4, 3, 4);
            var fraction2 = new Fraction(4, 3, 4);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_true_for_two_equivalent_positive_fractions_with_integer_components()
        {
            var fraction1 = new Fraction(4, 3, 4);
            var fraction2 = new Fraction(4, 6, 8);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_true_for_an_equivalent_positive_vulgar_fraction()
        {
            var fraction1 = new Fraction(4, 3, 4);
            var fraction2 = new Fraction(38, 8);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_true_for_two_identical_negative_fractions()
        {
            var fraction1 = new Fraction(3, 4, true);
            var fraction2 = new Fraction(3, 4, true);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_true_for_two_equivalent_negative_fractions()
        {
            var fraction1 = new Fraction(3, 4, true);
            var fraction2 = new Fraction(6, 8, true);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_true_for_two_identical_negative_fractions_with_integer_components()
        {
            var fraction1 = new Fraction(4, 3, 4, true);
            var fraction2 = new Fraction(4, 3, 4, true);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_true_for_two_equivalent_negative_fractions_with_integer_components()
        {
            var fraction1 = new Fraction(4, 3, 4, true);
            var fraction2 = new Fraction(4, 6, 8, true);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_true_for_an_equivalent_negative_vulgar_fraction()
        {
            var fraction1 = new Fraction(4, 3, 4, true);
            var fraction2 = new Fraction(38, 8, true);

            Assert.That(() => fraction1.Equals(fraction2), Is.True);
        }

        [Test]
        public void Equals_returns_false_for_unequal_fraction_which_differs_by_denominator()
        {
            var fraction1 = new Fraction(3, 4);
            var fraction2 = new Fraction(3, 5);

            Assert.That(() => fraction1.Equals(fraction2), Is.False);
        }

        [Test]
        public void Equals_returns_false_for_unequal_fraction_which_differs_by_numerator()
        {
            var fraction1 = new Fraction(3, 4);
            var fraction2 = new Fraction(2, 4);

            Assert.That(() => fraction1.Equals(fraction2), Is.False);
        }

        [Test]
        public void Equals_returns_false_for_unequal_fraction_which_differs_by_integer_amount()
        {
            var fraction1 = new Fraction(1, 3, 4);
            var fraction2 = new Fraction(2, 3, 4);

            Assert.That(() => fraction1.Equals(fraction2), Is.False);
        }

        [Test]
        public void Equals_returns_false_for_unequal_fraction_which_differs_by_sign()
        {
            var fraction1 = new Fraction(2, 3, 4);
            var fraction2 = new Fraction(2, 3, 4, true);

            Assert.That(() => fraction1.Equals(fraction2), Is.False);
        }

        [Test]
        public void Equals_returns_false_for_different_object_type()
        {
            var fraction1 = new Fraction(2, 3, 4);

            Assert.That(() => fraction1.Equals(Guid.NewGuid()), Is.False);
        }

        #endregion

        #region ToString

        [Test,AutoMoqData]
        public void ToString_uses_formatter_service(IFormatsFraction formatter,
                                                    Fraction sut,
                                                    string expected)
        {
            Fraction.Formatter = formatter;
            Mock.Get(formatter).Setup(x => x.Format(sut, "S")).Returns(expected);
            Assert.That(() => sut.ToString(), Is.EqualTo(expected));
        }

        #endregion
    }
}
