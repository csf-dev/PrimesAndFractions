//
// PrimeFactoriser.cs
//
// Authors:
//       Clinton Sheppard <https://handcraftsman.wordpress.com/>
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2016 the respective authors
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
  /// <para>A utility type for the generation of prime numbers and factorising numbers.</para>
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is heavily (almost completely) based on the excellent work found at
  /// http://handcraftsman.wordpress.com/2010/09/02/ienumerable-of-prime-numbers-in-csharp/ and
  /// http://handcraftsman.wordpress.com/2010/09/02/prime-factorization-in-csharp/
  /// </para>
  /// </remarks>
  public class PrimeFactoriser
  {
    #region methods

    /// <summary>
    /// <para>Gets a collection of the prime factors that are common between two numbers.</para>
    /// </summary>
    /// <param name="firstNumber">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <param name="secondNumber">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A collection of <see cref="System.Int32"/>
    /// </returns>
    public IEnumerable<int> GetCommonPrimeFactors(int firstNumber, int secondNumber)
    {
      var firstPrimeFactors = this.GetPrimeFactors(firstNumber).ToList();
      var secondPrimeFactors = this.GetPrimeFactors(secondNumber).ToList();

      return GetCommonPrimeFactors(firstPrimeFactors, secondPrimeFactors);
    }
    
    /// <summary>
    /// <para>Gets a collection of the prime factors of a number.</para>
    /// </summary>
    /// <param name="number">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A collection of <see cref="System.Int32"/>
    /// </returns>
    public IEnumerable<int> GetPrimeFactors(int number)
    {
      /* A future plan for this method would be to cache the primes that this instance has already generated.
       * Then, future calls will use the cache as much as possible rather than generating the prime numbers afresh
       * on every call.  This is particularly annoying because this method operates recursively.
       */
      int first = this.GeneratePrimes(number)
                  .TakeWhile(x => x <= Math.Sqrt(number))
                  .FirstOrDefault(x => number % x == 0);
      
      return (first == 0)? new[] { number } : new[] { first }.Concat(this.GetPrimeFactors(number / first));
    }
    
    /// <summary>
    /// <para>Gets a collection of the prime numbers up to a given ceiling.</para>
    /// </summary>
    /// <param name="upTo">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A collection of <see cref="System.Int32"/>
    /// </returns>
    public IEnumerable<int> GeneratePrimes(int upTo)
    {
      /* A future plan for this method would be to have a startAt parameter as well, which only starts generating prime
       * numbers from the starting point.
       */
      List<int> cachedPrimes = new List<int>();
      int sqrt = 1;
      var primes = this.PotentialPrimes(upTo).Where (x =>
      {
        sqrt = this.GetSqrtCeiling(x, sqrt);
        return !cachedPrimes.TakeWhile (y => y <= sqrt).Any (y => x % y == 0);
      });
      
      foreach (int prime in primes) {
        yield return prime;
        cachedPrimes.Add (prime);
      }
    }

    private int GetSqrtCeiling (int value, int start)
    {
      while (start * start < value) {
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
    /// <returns>The primes.</returns>
    /// <param name="upTo">Up to.</param>
    private IEnumerable<int> PotentialPrimes(int upTo)
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
    /// Builds and returns a collection of the prime factors which exist in both first and second collections.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In this operation, duplicates (which appear in both collections more than once) are included multiple times in
    /// the output - as many times as they exist in both collections.
    /// </para>
    /// <para>
    /// Note that this method is destructive.  The collection of second prime factors is altered (elements removed) as
    /// the output is built.  Do not call this when you care that the input parameters are unmodified!
    /// </para>
    /// </remarks>
    /// <returns>The common prime factors.</returns>
    /// <param name="firstPrimeFactors">First prime factors.</param>
    /// <param name="secondPrimeFactors">Second prime factors.</param>
    private IEnumerable<int> GetCommonPrimeFactors(IList<int> firstPrimeFactors, IList<int> secondPrimeFactors)
    {
      var output = new List<int>();

      foreach(var factorInFirstCollection in firstPrimeFactors)
      {
        var indexInSecondCollection = secondPrimeFactors.IndexOf(factorInFirstCollection);

        if(indexInSecondCollection >= 0)
        {
          output.Add(factorInFirstCollection);
          secondPrimeFactors.RemoveAt(indexInSecondCollection);
        }
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// <para>Initialises the current instance.</para>
    /// </summary>
    private PrimeFactoriser () {}

    /// <summary>
    /// <para>Initialises the singleton <see cref="Default"/> generator.</para>
    /// </summary>
    static PrimeFactoriser ()
    {
      Default = new PrimeFactoriser ();
    }

    #endregion

    #region static properties

    /// <summary>
    /// <para>Read-only.  Gets a singleton instance of <see cref="PrimeFactoriser"/>.</para>
    /// </summary>
    public static PrimeFactoriser Default { get; private set; }
    
    #endregion
  }
}

