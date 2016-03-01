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
    }
}
