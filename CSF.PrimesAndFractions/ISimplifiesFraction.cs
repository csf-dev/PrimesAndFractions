//
// ISimplifiesFraction.cs
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

namespace CSF
{
    /// <summary>
    /// An object which can simplify a fraction, by reducing its denominator to the
    /// lowest possible value.
    /// </summary>
    public interface ISimplifiesFraction
    {
        /// <summary>
        /// Simplify the specified fraction, optionally preferring the use of vulgar fractions.
        /// </summary>
        /// <returns>The simplified fraction.</returns>
        /// <param name="input">The fraction to simplify.</param>
        /// <param name="preferVulgarFraction">If set to <c>true</c> then vulgar fractions are preferred over composite/mixed whole-number and fractional parts.</param>
        Fraction Simplify(Fraction input, bool preferVulgarFraction = false);
    }
}
