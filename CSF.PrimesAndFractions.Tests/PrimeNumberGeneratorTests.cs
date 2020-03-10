//
// PrimeNumberGeneratorTests.cs
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
using NUnit.Framework;

namespace CSF.Tests
{
    [TestFixture,Parallelizable]
    public class PrimeNumberGeneratorTests
    {
        [Test,AutoMoqData]
        public void GetPrimeNumbers_can_correctly_return_the_first_six_primes(PrimeNumberGenerator sut)
        {
            Assert.That(() => sut.GetPrimeNumbers(15), Is.EqualTo(new[] { 2, 3, 5, 7, 11, 13 }));
        }

        [Test, AutoMoqData]
        public void GetPrimeNumbers_can_correctly_return_the_first_six_primes_if_executed_twice(PrimeNumberGenerator sut)
        {
            sut.GetPrimeNumbers(15);
            Assert.That(() => sut.GetPrimeNumbers(15), Is.EqualTo(new[] { 2, 3, 5, 7, 11, 13 }));
        }

        [Test, AutoMoqData, Description("The hundred-thousandth prime number is 1299709.  Generating primes up to this ceiling should return precisely 100,000 numbers")]
        public void GetPrimeNumbers_can_get_the_one_hundred_thousandth_prime(PrimeNumberGenerator sut)
        {
            Assert.That(() => sut.GetPrimeNumbers(1299709).ToList(), Has.Count.EqualTo(100000));
        }
    }
}
