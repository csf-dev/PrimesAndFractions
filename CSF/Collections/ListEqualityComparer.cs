using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common = CSF.Collections.CommonCollectionEqualityComparisonFunctions;

namespace CSF.Collections
{
    public class ListEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
        readonly IEqualityComparer<TItem> itemComparer;

        bool IEqualityComparer.Equals(object x, object y) => Common.Equals<TItem>(x, y, Equals);

        int IEqualityComparer.GetHashCode(object obj) => Common.GetHashCode<TItem>(obj, GetHashCode);
        
        bool DoFiniteCollectionCountsDiffer(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var finiteCollectionComparer = new FiniteCollectionCountComparer<TItem>();
            return !finiteCollectionComparer.Equals(x as ICollection<TItem>, y as ICollection<TItem>);
        }

        public bool Equals(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            
            if (DoFiniteCollectionCountsDiffer(x, y))
                return false;

            return x.SequenceEqual(y, itemComparer);
        }

        public int GetHashCode(IEnumerable<TItem> obj)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException(nameof(obj));

            unchecked
            {
                return obj.Aggregate(19, (acc, next) => acc * 31 + Common.GetItemHashCode(next, itemComparer));
            }
        }

        public ListEqualityComparer() : this(null) {}

        public ListEqualityComparer(IEqualityComparer<TItem> itemComparer)
        {
            this.itemComparer = itemComparer ?? EqualityComparer<TItem>.Default;
        }
    }
}