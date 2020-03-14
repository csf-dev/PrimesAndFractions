//
// PrimeNumberGenerator.cs
//
// Author:
//       Clinton Sheppard <https://handcraftsman.wordpress.com/>
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2016-2020 the respective authors
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
using System.Linq;

namespace CSF
{
    /// <summary>
    /// A geneator for prime numbers.  Gets an ordered sequence of prime numbers up to an
    /// upper limit.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type is based heavily upon the excellent work found at
    /// http://handcraftsman.wordpress.com/2010/09/02/ienumerable-of-prime-numbers-in-csharp/ and
    /// http://handcraftsman.wordpress.com/2010/09/02/prime-factorization-in-csharp/
    /// </para>
    /// </remarks>
    public class PrimeNumberGenerator : IGetsPrimeNumbers
    {
        readonly IProvidesPrimeNumberCache cacheProvider;

        /// <summary>
        /// Generates and returns a sequence of all prime numbers starting with the first prime number
        /// (two) and ending with the highest prime number which is equal to or less than the
        /// specified <paramref name="upperLimit"/>.
        /// </summary>
        /// <returns>An ordered sequence of prime numbers.</returns>
        /// <param name="upperLimit">The highest numeric value for which to get prime numbers; this method will not get any prime numbers which are greater than this number.</param>
        public IEnumerable<long> GetPrimeNumbers(long upperLimit)
        {
            using (var cache = cacheProvider.GetCache())
            {
                var cachedPrimes = cache.Contents;

                foreach (var number in cachedPrimes)
                {
                    if (number > upperLimit) yield break;
                    yield return number;
                }

                long sqrt;
                if (cachedPrimes.Any())
                    sqrt = (long)Math.Abs(Math.Sqrt(cachedPrimes.Last()));
                else
                    sqrt = 1;

                var generatedPrimes = GetCandidatesForPrimeNumbers(upperLimit)
                    .Where(x =>
                    {
                        sqrt = GetSqrtCeiling(x, sqrt);
                        return !cachedPrimes.TakeWhile(y => y <= sqrt).Any(y => x % y == 0);
                    });

                foreach (var prime in generatedPrimes)
                {
                    yield return prime;
                    cachedPrimes.Add(prime);
                }
            }
        }

        long GetSqrtCeiling(long value, long start)
        {
            while (start * start < value)
            {
                start++;
            }
            return start;
        }

        /// <summary>
        /// Gets a collection of candidates for prime numbers, up to a given limit.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is an optimisation, Above the number 2 itself, no number divisible by 2 can be a prime number.
        /// Likewise, above the number 3, no number divisible by 3 can be a prime number.
        /// </para>
        /// <para>
        /// This method returns the numbers 2, then 3, and then a collection of numbers which are one-less-then and then
        /// one-greater-than multiples of 6.  The multiple of 6 itself is impossible (divides by 2 and 3).  The multiple of
        /// 6 ± two is impossible (it would divide by 2) as is the multiple of 6 ± three (it would divide by 3).
        /// Once we get to ± four or ± five we are talking about the same number as [a different multiple of 6] ± two or
        /// ± one (respectively) would result in.
        /// </para>
        /// <para>
        /// Thus, this method cuts the work we must perform to find prime numbers roughly to a third of that which we would
        /// need if we used a naïve search of the whole integer range.
        /// </para>
        /// </remarks>
        /// <returns>The candidates for prime numbers; not neccesarily prime numbers, but numbers which could feasibly be.</returns>
        /// <param name="upTo">The upper range/ceiling for which to get primes.</param>
        IEnumerable<long> GetCandidatesForPrimeNumbers(long upTo)
        {
            yield return 2;
            yield return 3;

            var n = 1;

            while (n > 0 && (6 * n - 1) <= upTo)
            {
                yield return 6 * n - 1;
                yield return 6 * n + 1;
                n++;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimeNumberGenerator"/> class.
        /// </summary>
        /// <param name="cacheProvider">An optional object which provides caching for prime numbers which have already been generated.</param>
        public PrimeNumberGenerator(IProvidesPrimeNumberCache cacheProvider = null)
        {
            this.cacheProvider = cacheProvider ?? new EphemeralPrimeNumberCacheProvider();
        }
    }
}
