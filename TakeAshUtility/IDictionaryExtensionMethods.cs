using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class IDictionaryExtensionMethods {

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dic">The dictionary to be scanned.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultValue">The returned value, if the key is not found.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>The value associated with the specified key, if the key is found.</item>
        /// <item>The defaultValue, if the key is not found.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// [c# - What is more efficient: Dictionary TryGetValue or ContainsKey+Item? - Stack Overflow](http://stackoverflow.com/questions/9382681/)
        /// </remarks>
        public static TValue SafeGetValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dic,
            TKey key,
            TValue defaultValue = default(TValue)
        ) {
            if (dic == null || key == null) {
                return defaultValue;
            }
            TValue value;
            return dic.TryGetValue(key, out value) ?
                value :
                defaultValue;
        }
    }
}
