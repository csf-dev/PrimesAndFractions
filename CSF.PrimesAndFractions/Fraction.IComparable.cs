//
// Fraction.IComparable.cs
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
    public partial struct Fraction : IComparable<Fraction>,
                                     IComparable
    {
        public int CompareTo(Fraction other)
        {
            var thisDouble = ToDouble();
            var otherDouble = other.ToDouble();

            var diff = thisDouble - otherDouble;
            if (diff < 0d) return -1;
            if (diff > 0d) return 1;
            return 0;
        }

        public int CompareTo(object obj)
        {
            if (obj is Fraction fraction) return CompareTo(fraction);
            throw new ArgumentException($"Object to compare with must be a {nameof(Fraction)}.", nameof(obj));
        }
    }
}
