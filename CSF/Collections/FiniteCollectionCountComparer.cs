using System.Collections;
using System.Collections.Generic;

namespace CSF.Collections
{
    public class FiniteCollectionCountComparer<TItem> : IEqualityComparer<ICollection<TItem>>, IEqualityComparer
    {
        public bool Equals(ICollection<TItem> x, ICollection<TItem> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Count == y.Count;
        }

        public int GetHashCode(ICollection<TItem> obj) => obj?.Count ?? 0;

        bool IEqualityComparer.Equals(object x, object y)
        {
            return Equals(x as ICollection<TItem>, y as ICollection<TItem>);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetHashCode(obj as ICollection<TItem>);
        }
    }
}