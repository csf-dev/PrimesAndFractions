using System;
using System.Collections.Generic;

namespace CSF.Collections
{
    internal static class CommonCollectionEqualityComparisonFunctions
    {
        internal static bool Equals<TItem>(object x, object y, Func<IEnumerable<TItem>,IEnumerable<TItem>,bool> genericEqualityFunc)
        {
            IEnumerable<TItem> a, b;

            try
            {
                a = (IEnumerable<TItem>) x;
                b = (IEnumerable<TItem>) y;
            }
            catch (InvalidCastException)
            {
                return false;
            }

            return genericEqualityFunc(a, b);
        }

        internal static int GetHashCode<TItem>(object obj, Func<IEnumerable<TItem>,int> genericHashCodeFunc)
        {
            if(ReferenceEquals(obj, null))
                throw new ArgumentNullException(nameof(obj));

            IEnumerable<TItem> collection;

            try { collection = (IEnumerable<TItem>) obj; }
            catch (InvalidCastException) { return 0; }

            return genericHashCodeFunc(collection);
        }

        internal static int GetItemHashCode<TItem>(TItem item, IEqualityComparer<TItem> itemComparer)
        {
            if (ReferenceEquals(item, null))
                return 31;

            return itemComparer.GetHashCode(item);
        }
    }
}