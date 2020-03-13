//
// Fraction.IConvertible.cs
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
    public partial struct Fraction
#if !NETSTANDARD1_0
        : IConvertible
#endif
    {
#if !NETSTANDARD1_0
        /// <summary>
        /// Gets the type code.
        /// </summary>
        /// <returns>The type code.</returns>
        public TypeCode GetTypeCode() => TypeCode.Object;
#endif

        /// <summary>
        /// Converts the current instance to a boolean.
        /// </summary>
        /// <returns><c>true</c>, if current instance is not zero or negative, <c>false</c> otherwise.</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public bool ToBoolean(IFormatProvider provider)
            => (AbsoluteInteger > 0L) || (Numerator > 0L) && !IsNegative;

        /// <summary>
        /// Converts the current instance to an unsigned byte.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public byte ToByte(IFormatProvider provider) => (byte) this;

        /// <summary>
        /// Converts the current instance to a character.
        /// </summary>
        /// <returns>A character</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public char ToChar(IFormatProvider provider) => Convert.ToChar(ToDouble());

        /// <summary>
        /// Converts the current instance to a date/time.
        /// </summary>
        /// <returns>A date/time</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(ToDouble());

        /// <summary>
        /// Converts the current instance to a decimal.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public decimal ToDecimal(IFormatProvider provider) => ToDecimal();

        /// <summary>
        /// Converts the current instance to a double-precision floating poit.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public double ToDouble(IFormatProvider provider) => ToDouble();

        /// <summary>
        /// Converts the current instance to a signed 16-bit integer.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public short ToInt16(IFormatProvider provider) => (short) this;

        /// <summary>
        /// Converts the current instance to a signed 32-bit integer.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public int ToInt32(IFormatProvider provider) => (int) this;

        /// <summary>
        /// Converts the current instance to a signed 64-bit integer.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public long ToInt64(IFormatProvider provider) => (long) this;

        /// <summary>
        /// Converts the current instance to a signed byte.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public sbyte ToSByte(IFormatProvider provider) => (sbyte) this;

        /// <summary>
        /// Converts the current instance to a single-precision floating poit.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public float ToSingle(IFormatProvider provider) => ToSingle();

        /// <summary>
        /// Converts the current instance to a string, using the given format provider.
        /// See also: <see cref="ToString(string,IFormatProvider)"/>
        /// </summary>
        /// <returns>A string.</returns>
        /// <param name="provider">A format provider be able to return an <see cref="IFormatsFraction"/>.</param>
        public string ToString(IFormatProvider provider) => ToString(null, provider);

        /// <summary>
        /// Attempts to convert the current instance to an arbitrary type.
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="conversionType">The type to which to convert the fraction.</param>
        /// <param name="provider">A format provider (which is unused).</param>
        public object ToType(Type conversionType, IFormatProvider provider)
            => Convert.ChangeType(ToDouble(), conversionType);

        /// <summary>
        /// Converts the current instance to an unsigned 16-bit integer.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public ushort ToUInt16(IFormatProvider provider) => (ushort) this;

        /// <summary>
        /// Converts the current instance to an unsigned 32-bit integer.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public uint ToUInt32(IFormatProvider provider) => (uint) this;

        /// <summary>
        /// Converts the current instance to an unsigned 64-bit integer.
        /// </summary>
        /// <returns>A number</returns>
        /// <param name="provider">A format provider (which is unused).</param>
        public ulong ToUInt64(IFormatProvider provider) => (ulong) this;
    }
}
