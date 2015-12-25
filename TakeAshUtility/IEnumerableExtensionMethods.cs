using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TakeAshUtility {

    public static class IEnumerableExtensionMethods {

        /// <summary>
        /// Returns the minimal element of the given sequence,
        /// based on the given projection.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <item>[morelinq - Extensions to LINQ to Objects - Google Project Hosting](http://code.google.com/p/morelinq/)</item>
        /// <item>[c# - How to use LINQ to select object with minimum or maximum property value - Stack Overflow](http://stackoverflow.com/questions/914109/)</item>
        /// <item>[c# - How to use linq to find the minimum - Stack Overflow](http://stackoverflow.com/questions/2736236/)</item>
        /// </list>
        /// </remarks>
        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector
        ) {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the minimal element of the given sequence,
        /// based on the given projection and the specified comparer for projected values.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <item>[morelinq - Extensions to LINQ to Objects - Google Project Hosting](http://code.google.com/p/morelinq/)</item>
        /// <item>[c# - How to use LINQ to select object with minimum or maximum property value - Stack Overflow](http://stackoverflow.com/questions/914109/)</item>
        /// <item>[c# - How to use linq to find the minimum - Stack Overflow](http://stackoverflow.com/questions/2736236/)</item>
        /// </list>
        /// </remarks>
        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer
        ) {
            if (source == null || source.Count() == 0 || selector == null || comparer == null) {
                return default(TSource);
            }
            using (var iterator = source.GetEnumerator()) {
                iterator.MoveNext();
                var candidate = iterator.Current;
                var candidateKey = selector(candidate);
                while (iterator.MoveNext()) {
                    var next = iterator.Current;
                    var nextKey = selector(next);
                    if (comparer.Compare(candidateKey, nextKey) > 0) {
                        candidate = next;
                        candidateKey = nextKey;
                    }
                }
                return candidate;
            }
        }

        /// <summary>
        /// Returns the maximal element of the given sequence,
        /// based on the given projection.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <item>[morelinq - Extensions to LINQ to Objects - Google Project Hosting](http://code.google.com/p/morelinq/)</item>
        /// <item>[c# - How to use LINQ to select object with minimum or maximum property value - Stack Overflow](http://stackoverflow.com/questions/914109/)</item>
        /// <item>[c# - How to use linq to find the minimum - Stack Overflow](http://stackoverflow.com/questions/2736236/)</item>
        /// </list>
        /// </remarks>
        public static TSource MaxBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector
        ) {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the maximal element of the given sequence,
        /// based on the given projection and the specified comparer for projected values.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <item>[morelinq - Extensions to LINQ to Objects - Google Project Hosting](http://code.google.com/p/morelinq/)</item>
        /// <item>[c# - How to use LINQ to select object with minimum or maximum property value - Stack Overflow](http://stackoverflow.com/questions/914109/)</item>
        /// <item>[c# - How to use linq to find the minimum - Stack Overflow](http://stackoverflow.com/questions/2736236/)</item>
        /// </list>
        /// </remarks>
        public static TSource MaxBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer
        ) {
            if (source == null || source.Count() == 0 || selector == null || comparer == null) {
                return default(TSource);
            }
            using (var iterator = source.GetEnumerator()) {
                iterator.MoveNext();
                var candidate = iterator.Current;
                var candidateKey = selector(candidate);
                while (iterator.MoveNext()) {
                    var next = iterator.Current;
                    var nextKey = selector(next);
                    if (comparer.Compare(candidateKey, nextKey) < 0) {
                        candidate = next;
                        candidateKey = nextKey;
                    }
                }
                return candidate;
            }
        }

        /// <summary>
        /// Make testcases for NUnit
        /// </summary>
        /// <typeparam name="T">Type of source unit</typeparam>
        /// <param name="source">Testcases source</param>
        /// <returns>Testcases for NUnit</returns>
        public static IEnumerable<object[]> ToTestCases<T>(this IEnumerable<T> source) {
            var readableProperties = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(prop => prop.CanRead == true && prop.GetGetMethod() != null)
                .ToArray();
            return source.Select(item => readableProperties.Select(prop => prop.GetValue(item, null)).ToArray());
        }

        /// <summary>
        /// ForEach for IEnumerable&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="enumeration">Source</param>
        /// <param name="action">Action to be applied for each source items</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>[LINQ equivalent of foreach for IEnumerable&lt;T&gt; - Stack Overflow](http://stackoverflow.com/questions/200574/)</item>
        /// <item>["foreach" vs "ForEach" | Fabulous Adventures In Coding](https://blogs.msdn.microsoft.com/ericlippert/2009/05/18/foreach-vs-foreach/)</item>
        /// </list>
        /// </remarks>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action) {
            if (enumeration == null || action == null) {
                return;
            }
            foreach (T item in enumeration) {
                action(item);
            }
        }

        /// <summary>
        /// ForEach for IEnumerable&lt;T&gt; with index
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="enumeration">Source</param>
        /// <param name="action">Action to be applied for each source items</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>[c# - List&lt;T&gt;.ForEach with index - Stack Overflow](http://stackoverflow.com/questions/13054281/)</item>
        /// </list>
        /// </remarks>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action) {
            if (enumeration == null || action == null) {
                return;
            }
            var index = 0;
            foreach (T item in enumeration) {
                action(item, index);
                ++index;
            }
        }
    }
}
