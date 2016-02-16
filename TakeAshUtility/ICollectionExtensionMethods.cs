using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class ICollectionExtensionMethods {

        public static void AddRange<T>(
            this ICollection<T> collection,
            IEnumerable<T> source
        ) {
            if (collection == null || source == null || source.Count() == 0) {
                return;
            }
            source.ForEach(item => collection.Add(item));
        }

        public static void SafeAddRange<T>(
            this ICollection<T> collection,
            IEnumerable<T> source
        ) where T : class {
            if (collection == null || source == null || source.Count() == 0) {
                return;
            }
            source.Where(item => item != null)
                .ForEach(item => collection.Add(item));
        }
    }
}
