//
// PrimeNumberObjectCacheProviderTests.cs
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
using System.Runtime.Caching;
using NUnit.Framework;

namespace CSF.Tests
{
    [TestFixture]
    public class PrimeNumberObjectCacheProviderTests
    {
        [Test]
        public void GetCache_provides_a_cache_which_records_and_sorts_stored_numbers_after_disposal()
        {
            var cache = new MemoryCache(nameof(GetCache_provides_a_cache_which_records_and_sorts_stored_numbers_after_disposal));
            var sut = new PrimeNumberObjectCacheProvider(cache);

            using(var instance1 = sut.GetCache())
            {
                instance1.Contents.Add(3);
                instance1.Contents.Add(1);
                instance1.Contents.Add(2);

                using (var instance2 = sut.GetCache())
                {
                    Assert.That(instance2.Contents, Is.Empty, "Contents are empty before first instance disposed");

                    instance2.Contents.Add(4);
                    instance2.Contents.Add(1);
                }
            }

            using (var instance3 = sut.GetCache())
            {
                Assert.That(instance3.Contents, Is.EqualTo(new[] { 1, 2, 3, 4 }), "Contents are populated and combined from all disposed instances");
            }
        }
    }
}
