using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {
    
    public static class ListExtensionMethods {

        public static void SafeAdd<T>(this List<T> source, T item) {
            if (source == null || item == null) {
                return;
            }
            source.Add(item);
        }
    }
}
