//
// IParsesFraction.cs
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
namespace CSF
{
    /// <summary>
    /// An object which can parse a string which represents a fraction and return instances of
    /// the <see cref="Fraction"/> type.
    /// </summary>
    public interface IParsesFraction
    {
        /// <summary>
        /// Parses a specified string which represents a formatted fraction value and returns a
        /// <see cref="Fraction"/> created from that string.
        /// </summary>
        /// <returns>The parsed fraction.</returns>
        /// <param name="fractionString">A string which represents a fraction.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="fractionString"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If the <paramref name="fractionString"/> is not a correctly-formatted string that represents a fraction.</exception>
        Fraction Parse(string fractionString);
    }
}
