using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Collections
{
    public class ListEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
        // Arbitrarily chosen prime numbers, useful in hashing functions
        const int
            FirstPrimeNumber = 19,
            SecondPrimeNumber = 31;

        readonly IEqualityComparer<TItem> itemComparer;

        bool IEqualityComparer.Equals(object x, object y) => NonGenericCollectionEqualityComparisons.Equals<TItem>(x, y, Equals);

        int IEqualityComparer.GetHashCode(object obj) => NonGenericCollectionEqualityComparisons.GetHashCode<TItem>(obj, GetHashCode);
        
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
                return obj.Aggregate(FirstPrimeNumber, (acc, next) => acc * SecondPrimeNumber + itemComparer.GetHashCode(next));
            }
        }

        public ListEqualityComparer() : this(null) {}

        public ListEqualityComparer(IEqualityComparer<TItem> itemComparer)
        {
            this.itemComparer = itemComparer ?? EqualityComparer<TItem>.Default;
        }
    }
}