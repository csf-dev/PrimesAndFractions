using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Collections
{
    public class SetEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
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

            var setOne = GetSet(x);

            return setOne.SetEquals(y);
        }

        public int GetHashCode(IEnumerable<TItem> obj)
        {
            if (ReferenceEquals(obj, null)) return 0;
            var set = GetSet(obj);
            return set.Aggregate(0, (acc, next) => acc ^ itemComparer.GetHashCode(next));
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