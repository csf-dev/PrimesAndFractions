using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Collections
{
    public class ListEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
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
            if (ReferenceEquals(x, null)) return false;

            return x.SequenceEqual(y, itemComparer);
        }

        public int GetHashCode(IEnumerable<TItem> obj)
        {
            throw new System.NotImplementedException();
        }

        public ListEqualityComparer() : this(null) {}

        public ListEqualityComparer(IEqualityComparer<TItem> itemComparer)
        {
            this.itemComparer = itemComparer;
        }
    }
}