using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    /// <summary>
    /// Create comparer from lambda
    /// </summary>
    /// <typeparam name="T">Type of the object to compare</typeparam>
    /// <remarks>
    /// [c# - Using IEqualityComparer for Union - Stack Overflow](http://stackoverflow.com/questions/5969505)
    /// </remarks>
    public class InlineEqualityComparer<T> :
        IEqualityComparer<T> {

        private readonly Func<T, T, bool> _equals;
        private readonly Func<T, int> _getHashCode;

        public InlineEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode) {
            _equals = equals;
            _getHashCode = getHashCode;
        }

        public bool Equals(T x, T y) {
            return _equals(x, y);
        }

        public int GetHashCode(T obj) {
            return _getHashCode(obj);
        }
    }

    public class InlineComparer<T> :
        IComparer<T> {

        private readonly Func<T, T, int> _compare;

        public InlineComparer(Func<T, T, int> compare) {
            _compare = compare;
        }

        public int Compare(T x, T y) {
            return _compare(x, y);
        }
    }
}
