using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common = CSF.Collections.CommonCollectionEqualityComparisonFunctions;

namespace CSF.Collections
{
    /// <summary>
    /// Implementation of <c>IEqualityComparer&lt;T&gt;</c> which compares two enumerable objects for set equality.
    /// That is, they must contain an equal collection of items, in any order, disregarding duplicate items.
    /// </summary>
    /// <typeparam name="TItem">The type of item within the collections</typeparam>
    public class SetEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
        readonly IEqualityComparer<TItem> itemComparer;

        bool IEqualityComparer.Equals(object x, object y) => Common.Equals<TItem>(x, y, Equals);

        int IEqualityComparer.GetHashCode(object obj) => Common.GetHashCode<TItem>(obj, GetHashCode);

        public bool Equals(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            var setOne = GetSet(x);

            return setOne.SetEquals(y);
        }

        public int GetHashCode(IEnumerable<TItem> obj)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException(nameof(obj));

            var set = GetSet(obj);
            return set.Aggregate(0, (acc, next) => acc ^ Common.GetItemHashCode(next, itemComparer));
        }

        ISet<TItem> GetSet(IEnumerable<TItem> collection)
        {
            var set = collection as HashSet<TItem>;
            if (set != null && set.Comparer.Equals(itemComparer))
                return set;

            return new HashSet<TItem>(collection, itemComparer);
        }

        public SetEqualityComparer()
        {
            itemComparer = EqualityComparer<TItem>.Default;
        }

        public SetEqualityComparer(IEqualityComparer<TItem> itemComparer)
        {
            if (itemComparer == null) throw new ArgumentNullException(nameof(itemComparer));
            this.itemComparer = itemComparer;
        }
    }
}