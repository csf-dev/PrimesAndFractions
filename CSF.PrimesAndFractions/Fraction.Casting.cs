//
// FractionC.cs
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
    public partial struct Fraction : IEquatable<Fraction>,
                                     IComparable<Fraction>,
                                     IComparable,
                                     IConvertible,
                                     IFormattable
    {
        public static implicit operator decimal(Fraction f) => f.ToDecimal();

        public static explicit operator double(Fraction f) => f.ToDouble();

        public static explicit operator float(Fraction f) => f.ToSingle();

        public static explicit operator sbyte(Fraction f) => (sbyte) (f.Simplify(false).AbsoluteInteger * f.Multiplier);

        public static explicit operator short(Fraction f) => (short) (f.Simplify(false).AbsoluteInteger * f.Multiplier);

        public static explicit operator int(Fraction f) => (int) f.Simplify(false).AbsoluteInteger * (int) f.Multiplier;

        public static explicit operator long(Fraction f) => f.Simplify(false).AbsoluteInteger * f.Multiplier;

        public static explicit operator byte(Fraction f) => (byte) f.Simplify(false).AbsoluteInteger;

        public static explicit operator ushort(Fraction f) => (ushort) f.Simplify(false).AbsoluteInteger;

        public static explicit operator uint(Fraction f) => (uint) f.Simplify(false).AbsoluteInteger;

        public static explicit operator ulong(Fraction f) => (ulong) f.Simplify(false).AbsoluteInteger;

        public static implicit operator Fraction(sbyte n) => new Fraction(n);

        public static implicit operator Fraction(short n) => new Fraction(n);

        public static implicit operator Fraction(int n) => new Fraction(n);

        public static implicit operator Fraction(long n) => new Fraction(n);

        public static implicit operator Fraction(byte n) => new Fraction(n);

        public static implicit operator Fraction(ushort n) => new Fraction(n);

        public static implicit operator Fraction(uint n) => new Fraction(n);

        public static explicit operator Fraction(ulong n) => new Fraction((long) n);
    }
}
