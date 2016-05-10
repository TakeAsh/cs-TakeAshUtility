using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class IDictionaryExtensionMethods {

        public static TValue SafeGetValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dic,
            TKey key,
            TValue defaultValue = default(TValue)
        ) {
            if (dic == null ||
                key == null ||
                !dic.ContainsKey(key)) {
                return defaultValue;
            }
            return dic[key];
        }
    }
}
