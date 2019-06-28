using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common = CSF.Collections.CommonCollectionEqualityComparisonFunctions;

namespace CSF.Collections
{
    /// <summary>
    /// Implementation of <c>IEqualityComparer&lt;T&gt;</c> which compares two enumerable objects for set equality.
    /// That is, they must contain an equal collection of items, in any order (duplicate items are considered, however).
    /// </summary>
    /// <typeparam name="TItem">The type of item within the collections</typeparam>
    public class BagEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
        readonly IEqualityComparer<TItem> itemEqComparer;
        readonly IComparer<TItem> itemComparer;

        bool IEqualityComparer.Equals(object x, object y) => Common.Equals<TItem>(x, y, Equals);

        int IEqualityComparer.GetHashCode(object obj) => Common.GetHashCode<TItem>(obj, GetHashCode);

        public bool Equals(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            var setResult = TryCompareAsSets(x, y);
            if (setResult.HasValue) return setResult.Value;

            if (DoFiniteCollectionCountsDiffer(x, y))
                return false;

            if(typeof(IComparable).IsAssignableFrom(typeof(TItem)))
                return AreCollectionsOfComparableItemsEqual(x, y);

            return !DoCollectionsDifferByElementEquality(x, y);
        }

        bool AreCollectionsOfComparableItemsEqual(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var coll1 = x.OrderBy(i => i, itemComparer);
            var coll2 = y.OrderBy(i => i, itemComparer);

            return coll1.SequenceEqual(coll2, itemEqComparer);
        }

        bool? TryCompareAsSets(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var set1 = x as HashSet<TItem>;
            var set2 = y as HashSet<TItem>;

            if (set1 != null
                && set2 != null
                && set1.Comparer.Equals(set2.Comparer)
                && set1.Comparer.Equals(itemComparer))
            {
                var comparer = new SetEqualityComparer<TItem>(itemEqComparer);
                return comparer.Equals(set1, set2);
            }

            return null;
        }

        bool DoFiniteCollectionCountsDiffer(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var finiteCollectionComparer = new FiniteCollectionCountComparer<TItem>();
            return !finiteCollectionComparer.Equals(x as ICollection<TItem>, y as ICollection<TItem>);
        }

        bool DoCollectionsDifferByElementEquality(IEnumerable<TItem> first, IEnumerable<TItem> second)
        {
            int firstNullCount;
            int secondNullCount;

            var firstElementCounts = GetCountsOfDistinctItems(first, out firstNullCount);
            var secondElementCounts = GetCountsOfDistinctItems(second, out secondNullCount);

            // An optimization to avoid a full comparison, if the counts of contents do not match
            if (firstNullCount != secondNullCount || firstElementCounts.Count != secondElementCounts.Count)
                return true;

            foreach (var kvp in firstElementCounts)
            {
                var firstElementCount = kvp.Value;
                int secondElementCount;
                secondElementCounts.TryGetValue(kvp.Key, out secondElementCount);

                if (firstElementCount != secondElementCount)
                    return true;
            }

            return false;
        }

        Dictionary<TItem, int> GetCountsOfDistinctItems(IEnumerable<TItem> enumerable, out int countOfNullItems)
        {
            var dictionary = new Dictionary<TItem, int>(itemEqComparer);
            countOfNullItems = 0;

            foreach (var element in enumerable)
            {
                if (ReferenceEquals(element, null))
                {
                    countOfNullItems ++;
                    continue;
                }

                int timesThisElementSeen;
                dictionary.TryGetValue(element, out timesThisElementSeen);
                timesThisElementSeen++;
                dictionary[element] = timesThisElementSeen;
            }

            return dictionary;
        }

        public int GetHashCode(IEnumerable<TItem> obj)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException(nameof(obj));

            return obj.Aggregate(0, (acc, next) => acc ^ Common.GetItemHashCode(next, itemEqComparer));
        }

        public BagEqualityComparer() : this(null, null) {}

        public BagEqualityComparer(IEqualityComparer<TItem> itemEqComparer = null, IComparer<TItem> itemComparer = null)
        {
            this.itemEqComparer = itemEqComparer ?? EqualityComparer<TItem>.Default;
            this.itemComparer = itemComparer ?? Comparer<TItem>.Default;
        }
    }
}