using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class IComparableExtensionMethods {

        /// <summary>
        /// Force a value to be in a range
        /// </summary>
        /// <typeparam name="T">the type of value</typeparam>
        /// <param name="value">the value</param>
        /// <param name="min">the minimum of the range</param>
        /// <param name="max">the maximum of the range</param>
        /// <param name="comparer">comparer of T</param>
        /// <returns>
        /// <list type="table">
        /// <item><term>value &lt;= min</term><description>min</description></item>
        /// <item><term>value &gt;= max</term><description>max</description></item>
        /// <item><term>min &lt; value &amp;&amp; value &lt; max</term><description>value</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// [.net - How to force a number to be in a range in C#? - Stack Overflow](http://stackoverflow.com/questions/3176602/)
        /// </remarks>
        public static T Clamp<T>(this T value, T min, T max, Comparer<T> comparer = null)
            where T : IComparable<T> {

            comparer = comparer ?? Comparer<T>.Default;
            if (comparer.Compare(value, min) <= 0) {
                return min;
            }
            if (comparer.Compare(value, max) >= 0) {
                return max;
            }
            return value;
        }
    }
}
