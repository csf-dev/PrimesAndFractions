using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Collections
{
    public class BagEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
        readonly IEqualityComparer<TItem> itemComparer;

        bool IEqualityComparer.Equals(object x, object y)
        {
            return Equals(x as IEnumerable<TItem>, y as IEnumerable<TItem>);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetHashCode(obj as IEnumerable<TItem>);
        }

        public bool Equals(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            var setResult = TryCompareAsSets(x, y);
            if (setResult.HasValue) return setResult.Value;

            if (DoFiniteCollectionCountsDiffer(x, y))
                return false;

            if (typeof(IComparable).IsAssignableFrom(typeof(TItem)))
                return AreCollectionsOfComparableItemsEqual(x.Cast<IComparable>(), y.Cast<IComparable>());

            throw new System.NotImplementedException();
        }

        bool AreCollectionsOfComparableItemsEqual(IEnumerable<IComparable> x, IEnumerable<IComparable> y)
        {
            throw new NotImplementedException();
        }

        bool? TryCompareAsSets(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var set1 = x as HashSet<TItem>;
            var set2 = y as HashSet<TItem>;

            if (set1 != null && set2 != null)
            {
                var comparer = new SetEqualityComparer<TItem>(itemComparer);
                return comparer.Equals(set1, set2);
            }

            return null;
        }

        bool DoFiniteCollectionCountsDiffer(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var coll1 = x as ICollection<TItem>;
            var coll2 = y as ICollection<TItem>;

            if (coll1 != null && coll2 != null && coll1.Count != coll2.Count)
                return true;

            return false;
        }


        public int GetHashCode(IEnumerable<TItem> obj)
        {
            if (ReferenceEquals(obj, null)) return 0;
            return obj.Aggregate(0, (acc, next) => acc ^ itemComparer.GetHashCode(next));
        }

        public BagEqualityComparer()
        {
            itemComparer = EqualityComparer<TItem>.Default;
        }

        public BagEqualityComparer(IEqualityComparer<TItem> itemComparer)
        {
            if (itemComparer == null) throw new ArgumentNullException(nameof(itemComparer));
            this.itemComparer = itemComparer;
        }
    }
}