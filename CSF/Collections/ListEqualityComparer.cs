using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace CSF.Collections
{
    public class ListEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
        // Arbitrarily chosen prime number, used to introduce some unpredictability to hash codes
        const int APrimeNumber = 137;

        readonly IEqualityComparer<TItem> itemComparer;

        bool IEqualityComparer.Equals(object x, object y)
        {
            return Equals(x as IEnumerable<TItem>, y as IEnumerable<TItem>);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetHashCode(obj as IEnumerable<TItem>);
        }
        
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
            if (ReferenceEquals(obj, null)) return 0;

            var increment = 1;

            return obj.Aggregate(0, (acc, next) =>
            {
                int multiplier;
                unchecked
                {
                    multiplier = (int) Math.Pow(increment++, 2) * APrimeNumber;
                }

                return acc ^ itemComparer.GetHashCode(next) ^ multiplier;
            });
        }

        public ListEqualityComparer() : this(null) {}

        public ListEqualityComparer(IEqualityComparer<TItem> itemComparer)
        {
            this.itemComparer = itemComparer ?? EqualityComparer<TItem>.Default;
        }
    }
}