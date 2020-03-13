//
// PrimeFactorProvider.cs
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
    /// A service which gets the prime factors for an arbitrary number.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type is based heavily upon the excellent work found at
    /// http://handcraftsman.wordpress.com/2010/09/02/ienumerable-of-prime-numbers-in-csharp/ and
    /// http://handcraftsman.wordpress.com/2010/09/02/prime-factorization-in-csharp/
    /// </para>
    /// </remarks>
    public class PrimeFactorProvider : IGetsPrimeFactors
    {
        readonly IGetsPrimeNumbers primeNumberProvider;

        /// <summary>
        /// Gets the prime factors for the specified <paramref name="number"/>.
        /// </summary>
        /// <returns>The prime factors for the input number.</returns>
        /// <param name="number">A number for which to get the prime factors.</param>
        public IEnumerable<long> GetPrimeFactors(long number)
        {
            var output = new List<long>();

            for(long currentValue = number, factor = 1L;
                factor != default(long);
                currentValue = currentValue / (factor != default(long)? factor : 1L))
            {
                factor = primeNumberProvider.GetPrimeNumbers(currentValue)
                    .TakeWhile(x => x <= Math.Sqrt(currentValue))
                    .FirstOrDefault(x => currentValue % x == 0);

                output.Add((factor == default(long)) ? currentValue : factor);
            }

            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimeFactorProvider"/> class.
        /// </summary>
        /// <param name="primeNumberProvider">Prime number provider.</param>
        public PrimeFactorProvider(IGetsPrimeNumbers primeNumberProvider)
        {
            this.primeNumberProvider = primeNumberProvider ?? throw new ArgumentNullException(nameof(primeNumberProvider));
        }
    }
}
