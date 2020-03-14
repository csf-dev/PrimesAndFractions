//
// PrimeNumberObjectCacheProviderIntegrationTests.cs
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
using System.Diagnostics;
using System.Runtime.Caching;
using NUnit.Framework;
using System.Linq;

namespace CSF.Tests
{
    [TestFixture,NonParallelizable]
    public class PrimeNumberObjectCacheProviderIntegrationTests
    {
        [Test]
        public void GetPrimeNumbers_can_get_10_thousand_primes_quicker_a_second_time_after_using_a_cache()
        {
            const long tenThousandthPrime = 104729;

            var cache = new MemoryCache(nameof(GetPrimeNumbers_can_get_10_thousand_primes_quicker_a_second_time_after_using_a_cache));
            var cacheProvider = new PrimeNumberObjectCacheProvider(cache);
            IGetsPrimeNumbers primeGenerator = new PrimeNumberGenerator(cacheProvider);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var firstAttempt = primeGenerator.GetPrimeNumbers(tenThousandthPrime).ToList();
            var firstTime = stopwatch.ElapsedTicks;

            stopwatch.Restart();

            var secondAttempt = primeGenerator.GetPrimeNumbers(tenThousandthPrime).ToList();
            var secondTime = stopwatch.ElapsedTicks;

            stopwatch.Stop();

            Console.WriteLine($"First attempt to get  10,000 primes took {firstTime} ticks");
            Console.WriteLine($"Second attempt to get 10,000 primes took {secondTime} ticks");

            Assert.That(secondTime, Is.LessThan(firstTime));
            Assert.That(firstAttempt.Count, Is.EqualTo(10000), "Count of primes in first run");
            Assert.That(secondAttempt.Count, Is.EqualTo(10000), "Count of primes in second run");
        }

        [Test]
        public void GetPrimeNumbers_can_get_10_primes_quicker_a_second_time_after_using_a_cache()
        {
            const long tenthPrime = 29;

            var cache = new MemoryCache(nameof(GetPrimeNumbers_can_get_10_primes_quicker_a_second_time_after_using_a_cache));
            var cacheProvider = new PrimeNumberObjectCacheProvider(cache);
            IGetsPrimeNumbers primeGenerator = new PrimeNumberGenerator(cacheProvider);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var firstAttempt = primeGenerator.GetPrimeNumbers(tenthPrime).ToList();
            var firstTime = stopwatch.ElapsedTicks;

            stopwatch.Restart();

            var secondAttempt = primeGenerator.GetPrimeNumbers(tenthPrime).ToList();
            var secondTime = stopwatch.ElapsedTicks;

            stopwatch.Stop();

            Console.WriteLine($"First attempt to get  10 primes took {firstTime} ticks");
            Console.WriteLine($"Second attempt to get 10 primes took {secondTime} ticks");

            Assert.That(secondTime, Is.LessThan(firstTime));
            Assert.That(firstAttempt.Count, Is.EqualTo(10), "Count of primes in first run");
            Assert.That(secondAttempt.Count, Is.EqualTo(10), "Count of primes in second run");
        }
    }
}
