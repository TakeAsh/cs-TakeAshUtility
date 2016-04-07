using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class BytesExtensionMethods {

        /// <summary>
        /// Reports the zero-based indexes of the specified pattern in this instance.
        /// The search starts at a specified index.
        /// </summary>
        /// <param name="bytes">The byte array to be sought.</param>
        /// <param name="pattern">The pattern to seek.</param>
        /// <param name="startIndex">The search starting index.</param>
        /// <param name="endIndex">The search ending index.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>The zero-based indexes of pattern, if the pattern is found.</item>
        /// <item>null, if the pattern is not found.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// [c# - byte[] array pattern search - Stack Overflow](http://stackoverflow.com/questions/283456/)
        /// </remarks>
        public static List<int> IndexesOf(
            this byte[] bytes,
            byte[] pattern,
            int startIndex = 0,
            int endIndex = int.MaxValue
        ) {
            if (bytes == null ||
                pattern == null ||
                bytes.Length == 0 ||
                pattern.Length == 0 ||
                bytes.Length < pattern.Length) {
                return null;
            }
            if (endIndex > bytes.Length) {
                endIndex = bytes.Length;
            }
            var indexes = new List<int>();
            for (var i = startIndex; i < endIndex; ++i) {
                if (pattern.Length > endIndex - i ||
                    !IsMatch(bytes, i, pattern)) {
                    continue;
                }
                indexes.Add(i);
            }
            return indexes.Count == 0 ?
                null :
                indexes;
        }

        private static bool IsMatch(byte[] array, int position, byte[] pattern) {
            for (int i = 0; i < pattern.Length; ++i) {
                if (array[position + i] != pattern[i]) {
                    return false;
                }
            }
            return true;
        }
    }
}
