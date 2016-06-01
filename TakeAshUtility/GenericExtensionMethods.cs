using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TakeAshUtility {

    public static class GenericExtensionMethods {

        private const BindingFlags _flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;

        /// <summary>
        /// Duplicate properties from source to destination.
        /// </summary>
        /// <typeparam name="T">Type of destination and source</typeparam>
        /// <param name="destination">Destination to be overwrited</param>
        /// <param name="source">Source of properties</param>
        /// <remarks>
        /// Properties to be Duplicated are Instance | Public | DeclaredOnly property.
        /// </remarks>
        public static void Duplicate<T>(this T destination, T source) {
            var properties = typeof(T).GetProperties(_flags)
                .Where(property => property.CanWrite == true &&
                    property.GetSetMethod() != null &&
                    property.GetGetMethod() != null &&
                    property.GetIndexParameters().Length == 0
                ).SafeToArray();
            if (destination == null || source == null || properties == null) {
                return;
            }
            properties.ForEach(property => property.SetValue(destination, property.GetValue(source, null), null));
        }

        /// <summary>
        /// Duplicate properties from source to destination.
        /// </summary>
        /// <typeparam name="T">Type of destination and source</typeparam>
        /// <param name="destination">Destination to be overwrited</param>
        /// <param name="source">Source of properties</param>
        /// <param name="properties">Property names to be duplicated</param>
        public static void Duplicate<T>(this T destination, T source, IEnumerable<string> properties) {
            properties.ForEach(property => {
                var pi = typeof(T).GetProperty(property, Type.EmptyTypes);
                pi.SetValue(destination, pi.GetValue(source, null), null);
            });
        }

        /// <summary>
        /// Check the value is between the minimum and the maximum.
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="value">The value</param>
        /// <param name="minimum">The minimum value of the range</param>
        /// <param name="maximum">The maximum value of the range</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>true, if the value is in the range.</item>
        /// <item>false, if the value is out of the range.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// [c# - What's the Comparer&lt;T&gt; class for? - Stack Overflow](http://stackoverflow.com/questions/2843212/)
        /// </remarks>
        public static bool Between<T>(this T value, T minimum, T maximum) {
            var comparer = Comparer<T>.Default;
            return comparer.Compare(value, minimum) >= 0 &&
                comparer.Compare(value, maximum) <= 0;
        }
    }
}
