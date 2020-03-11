﻿//
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
        public TypeCode GetTypeCode() => TypeCode.Object;

        public bool ToBoolean(IFormatProvider provider)
            => (AbsoluteInteger > 0L) || (Numerator > 0L) && !IsNegative;

        public byte ToByte(IFormatProvider provider) => (byte) this;

        public char ToChar(IFormatProvider provider) => Convert.ToChar(ToDouble());

        public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(ToDouble());

        public decimal ToDecimal(IFormatProvider provider) => ToDecimal();

        public double ToDouble(IFormatProvider provider) => ToDouble();

        public short ToInt16(IFormatProvider provider) => (short) this;

        public int ToInt32(IFormatProvider provider) => (int) this;

        public long ToInt64(IFormatProvider provider) => (long) this;

        public sbyte ToSByte(IFormatProvider provider) => (sbyte) this;

        public float ToSingle(IFormatProvider provider) => ToSingle();

        public string ToString(IFormatProvider provider) => ToString(null, provider);

        public object ToType(Type conversionType, IFormatProvider provider)
            => Convert.ChangeType(ToDouble(), conversionType);

        public ushort ToUInt16(IFormatProvider provider) => (ushort) this;

        public uint ToUInt32(IFormatProvider provider) => (uint) this;

        public ulong ToUInt64(IFormatProvider provider) => (ulong) this;
    }
}
