//
// CommonPrimeFactorProvider.cs
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
    /// A service which gets the prime factors which are common to two arbitrary numbers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type is based heavily upon the excellent work found at
    /// http://handcraftsman.wordpress.com/2010/09/02/ienumerable-of-prime-numbers-in-csharp/ and
    /// http://handcraftsman.wordpress.com/2010/09/02/prime-factorization-in-csharp/
    /// </para>
    /// </remarks>
    public class CommonPrimeFactorProvider : IGetsCommonPrimeFactors
    {
        readonly IGetsPrimeFactors primeFactorsProvider;

        /// <summary>
        /// Gets the prime factors which are common to the two specified numbers.
        /// </summary>
        /// <returns>The common prime factors.</returns>
        /// <param name="first">The first number.</param>
        /// <param name="second">The second number.</param>
        public IEnumerable<long> GetPrimeFactors(long first, long second)
        {
            var firstPrimeFactors = primeFactorsProvider.GetPrimeFactors(first).ToList();
            var secondPrimeFactors = primeFactorsProvider.GetPrimeFactors(second).ToList();

            return GetCommonPrimeFactors(firstPrimeFactors, secondPrimeFactors);
        }

        IEnumerable<long> GetCommonPrimeFactors(IList<long> firstPrimeFactors, IList<long> secondPrimeFactors)
        {
            var output = new List<long>();

            foreach (var factorInFirstCollection in firstPrimeFactors)
            {
                var indexInSecondCollection = secondPrimeFactors.IndexOf(factorInFirstCollection);

                if (indexInSecondCollection >= 0)
                {
                    output.Add(factorInFirstCollection);
                    secondPrimeFactors.RemoveAt(indexInSecondCollection);
                }
            }

            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonPrimeFactorProvider"/> class.
        /// </summary>
        /// <param name="primeFactorsProvider">Prime factors provider.</param>
        public CommonPrimeFactorProvider(IGetsPrimeFactors primeFactorsProvider)
        {
            this.primeFactorsProvider = primeFactorsProvider ?? throw new ArgumentNullException(nameof(primeFactorsProvider));
        }

        /// <summary>
        /// Initializes the <see cref="CommonPrimeFactorProvider"/> class.
        /// </summary>
        static CommonPrimeFactorProvider()
        {
            var primeProvider = new PrimeNumberGenerator();
            var factoriser = new PrimeFactorProvider(primeProvider);
            Default = new CommonPrimeFactorProvider(factoriser);
        }

        /// <summary>
        /// Gets a default instance of <see cref="IGetsCommonPrimeFactors"/>.
        /// </summary>
        /// <value>The default common prime factor provider.</value>
        public static IGetsCommonPrimeFactors Default { get; }
    }
}
