using System.Collections;
using System.Collections.Generic;

namespace CSF.Collections
{
    public class SetEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
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

            throw new System.NotImplementedException();
        }

        public int GetHashCode(IEnumerable<TItem> obj)
        {
            throw new System.NotImplementedException();
        }
    }
}