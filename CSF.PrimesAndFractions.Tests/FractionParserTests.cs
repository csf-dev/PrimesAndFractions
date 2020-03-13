//
// FractionParserTests.cs
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
using System.Collections.Generic;
using NUnit.Framework;

namespace CSF.Tests
{
    [TestFixture,Parallelizable]
    public class FractionParserTests
    {
        [Test]
        public void Parse_can_successfully_parse_various_fractions()
        {
            var scenarios = new Dictionary<string, Fraction>
            {
                { "3/4", new Fraction(3, 4) },
                { "-3/4", new Fraction(-3, 4) },
                { "-100 3/4", new Fraction(-100, 3, 4) },
                { "123 3/4", new Fraction(123, 3, 4) },
                { "16/4", new Fraction(4, 0, 4) },
            };
            var sut = new FractionParser();

            foreach (var scenario in scenarios)
                Assert.That(() => sut.Parse(scenario.Key), Is.EqualTo(scenario.Value), scenario.Key);
        }

        [Test]
        public void Parse_throws_FormatException_for_an_invalid_string()
        {
            var sut = new FractionParser();

            Assert.That(() => sut.Parse("Foo Bar Baz"), Throws.InstanceOf<FormatException>());
        }
    }
}
