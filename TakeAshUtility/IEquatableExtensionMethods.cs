using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class IEquatableExtensionMethods {

        public static bool EqualsEx<T>(this T a, object b)
            where T : IEquatable<T> {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b)) {
                return true;
            }
            // If one is null, or b is not T, return false.
            if ((object)a == null ||
                (object)b == null ||
                !(b is T)) {
                return false;
            }
            // Compare a with b as T
            return a.Equals((T)b);
        }
    }
}
