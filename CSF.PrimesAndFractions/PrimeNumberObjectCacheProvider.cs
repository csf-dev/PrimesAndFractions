//
// PrimeNumberObjectCacheProvider.cs
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

/* Note that this type is UNAVAILABLE in netstandard1.0 builds.  It relies upon a
 * dependency which has no equivalent in that target runtime.  To use caching EITHER
 * build for netstandard2.0 or net45 OR choose/create an alternative implementation
 * of IProvidesPrimeNumberCache.
 */
#if !NETSTANDARD1_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;

namespace CSF
{
    /// <summary>
    /// A provider class which gets an <see cref="ICachesPrimeNumbers"/>, using an instance of
    /// <see cref="ObjectCache"/> as a backing-store for the caching of prime numbers which have
    /// already been generated.
    /// </summary>
    public class PrimeNumberObjectCacheProvider : IProvidesPrimeNumberCache
    {
        // This uses an arbitrary GUID in order to avoid unwanted conflicts
        // in an object cache which is not used exclusively by this type.
        const string DefaultKey = "[PrimeNumberObjectCacheProvider 3b575cd3-0597-4d43-97be-53c8a0484685]";

        readonly ObjectCache cache;
        readonly CacheItemPolicy policy;
        readonly string key;
        readonly ReaderWriterLockSlim syncRoot;

        /// <summary>
        /// Gets the prime-number cache.
        /// </summary>
        /// <returns>A cache instance.</returns>
        public ICachesPrimeNumbers GetCache()
        {
            return new PrimeNumberObjectCache(cache, policy, key, syncRoot);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimeNumberObjectCacheProvider"/> class using
        /// a specified <see cref="ObjectCache"/> instance, and optionally a policy for the eviction of
        /// the cached prime numbers and a key to use (within the cache) for the storing of the primes.
        /// </summary>
        /// <param name="cache">The cache implementation to use.</param>
        /// <param name="policy">A policy for the storage of the cached prime numbers.  If not provided then a default policy will be used, which will store the cached primes indefinitely.</param>
        /// <param name="key">A key to use within the cache.  If not provide then a default key will be used identifying this type and using a GUID to attempt to prevent conflicts.</param>
        public PrimeNumberObjectCacheProvider(ObjectCache cache,
                                              CacheItemPolicy policy = null,
                                              string key = null)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.policy = policy ?? new CacheItemPolicy();
            this.key = key ?? DefaultKey;
            syncRoot = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }
    }

    internal sealed class PrimeNumberObjectCache : ICachesPrimeNumbers
    {
        readonly ObjectCache cache;
        readonly CacheItemPolicy policy;
        readonly string key;
        readonly ReaderWriterLockSlim syncRoot;

        int originalCount;
        List<long> cachedPrimes;
        bool isDisposed;

        /// <summary>
        /// Gets the contents of the cache.
        /// </summary>
        /// <value>The cache contents.</value>
        public ICollection<long> Contents
        {
            get
            {
                if (isDisposed)
                    throw new ObjectDisposedException($"This instance of {nameof(PrimeNumberObjectCache)} has been disposed; its contents are unavailable.");

                if (cachedPrimes == null)
                {
                    syncRoot.EnterUpgradeableReadLock();

                    // We copy the list so that (outside of disposal) we can't directly
                    // alter the contents of the cache.
                    cachedPrimes = new List<long>(GetCacheContents());
                    originalCount = cachedPrimes.Count;
                }

                return cachedPrimes;
            }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="PrimeNumberObjectCache"/> object.
        /// Also transfers all newly-discovered prime numbers (added to <see cref="Contents"/>) to the
        /// underlying cache implementation.
        /// </summary>
        /// <remarks>
        /// Call <see cref="Dispose"/> when you are finished using the <see cref="PrimeNumberObjectCache"/>.
        /// The <see cref="Dispose"/> method leaves the <see cref="PrimeNumberObjectCache"/> in an unusable
        /// state. After calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="PrimeNumberObjectCache"/> so the garbage collector can reclaim the memory that the
        /// <see cref="PrimeNumberObjectCache"/> was occupying.
        /// </remarks>
        public void Dispose()
        {
            try
            {
                syncRoot.EnterWriteLock();

                // If we haven't added any new primes since construction then
                // there's nothing to do.
                if (cachedPrimes == null || cachedPrimes.Count == originalCount) return;

                var currentContents = GetCacheContents();
                var newContents = currentContents
                    .Union(cachedPrimes)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                cache.Set(key, newContents, policy);
            }
            finally
            {
                isDisposed = true;

                if (syncRoot.IsWriteLockHeld)
                    syncRoot.ExitWriteLock();
                if (syncRoot.IsUpgradeableReadLockHeld)
                    syncRoot.ExitUpgradeableReadLock();
            }
        }

        List<long> GetCacheContents()
        {
            var newItem = new List<long>();
            var existing = (List<long>) cache.AddOrGetExisting(key, newItem, policy);
            return existing ?? newItem;
        }

        internal PrimeNumberObjectCache(ObjectCache cache,
                                        CacheItemPolicy policy,
                                        string key,
                                        ReaderWriterLockSlim syncRoot)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.policy = policy ?? throw new ArgumentNullException(nameof(policy));
            this.key = key ?? throw new ArgumentNullException(nameof(key));
            this.syncRoot = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));
        }
    }
}
#endif