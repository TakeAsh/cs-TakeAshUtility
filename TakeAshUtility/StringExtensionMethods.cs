﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace TakeAshUtility {

    /// <summary>
    /// String Extension Methods
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>[C# - TryParse系のメソッドで一時変数を用意したくない… - Qiita](http://qiita.com/Temarin_PITA/items/9aac6c1f569fc2113e0d)</item>
    /// </list>
    /// </remarks>
    public static class StringExtensionMethods {

        /// <summary>
        /// Convert string to object
        /// </summary>
        /// <typeparam name="T">target type</typeparam>
        /// <param name="text">text to convert</param>
        /// <returns>T type object</returns>
        public static T TryParse<T>(this string text) {

            return text.TryParse(default(T));
        }

        /// <summary>
        /// Convert string to object
        /// </summary>
        /// <typeparam name="T">target type</typeparam>
        /// <param name="text">text to convert</param>
        /// <param name="defaultValue">return value if fail</param>
        /// <returns>T type object</returns>
        public static T TryParse<T>(this string text, T defaultValue) {

            // コンバーターを作成
            var converter = TypeDescriptor.GetConverter(typeof(T));

            // 変換不可能な場合は規定値を返す
            if (!converter.CanConvertFrom(typeof(string))) {
                return defaultValue;
            }

            try {
                // 変換した値を返す
                return (T)converter.ConvertFrom(text);
            }
            catch {
                // 変換に失敗したら規定値を返す
                return defaultValue;
            }
        }

        /// <summary>
        /// Return defalut value if text is null or empty
        /// </summary>
        /// <param name="text">text to test</param>
        /// <param name="defaultValue">default value</param>
        /// <returns>
        /// <list type="table">
        /// <item><term>text is neither null nor empty</term><description>text</description></item>
        /// <item><term>text is null or empty</term><description>default value</description></item>
        /// </list>
        /// </returns>
        public static string ToDefaultIfNullOrEmpty(this string text, string defaultValue = "") {
            return !String.IsNullOrEmpty(text) ?
                text :
                defaultValue;
        }

        /// <summary>
        /// Returns a string array that contains the substrings in this string that are delimited by elements of a specified string array.
        /// </summary>
        /// <param name="text">String to be splitted.</param>
        /// <param name="separator">An array of single-character strings that delimit the substrings in this string. Defaut is ",".</param>
        /// <param name="options">RemoveEmptyEntries to omit empty array elements from the array returned; or None to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more strings in separator.</returns>
        public static IEnumerable<string> SplitTrim(
            this string text,
            string[] separator = null,
            StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries
        ) {
            separator = separator ?? new[] { "," };
            return String.IsNullOrEmpty(text) ?
                null :
                text.Trim()
                    .Split(separator, options)
                    .Select(item => item.Trim());
        }

        /// <summary>
        /// Returns a string containing a specified number of characters from the right side of a string.
        /// </summary>
        /// <param name="text">expression from which the rightmost characters are returned.</param>
        /// <param name="length">Numeric expression indicating how many characters to return.</param>
        /// <returns>a string containing a specified number of characters from the right side of a string.</returns>
        /// <remarks>
        /// [c# - Extract only right most n letters from a string - Stack Overflow](http://stackoverflow.com/questions/1722334/)
        /// </remarks>
        public static string Right(this string text, int length) {
            if (text == null || length < 0) {
                return null;
            }
            if (text.Length <= length) {
                return text;
            }
            return text.Substring(text.Length - length);
        }

        private static readonly Regex _regQuotemeta = new Regex(@"([^0-9A-Za-z_])");

        /// <summary>
        /// escape non-word characters as unicode expression.
        /// </summary>
        /// <param name="source">raw string</param>
        /// <returns>escaped string</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <item>[Regex.Escape Method](http://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex.escape.aspx)</item>
        /// <item>[quotemeta for javascript](http://blog.livedoor.jp/dankogai/archives/51058313.html)</item>
        /// </list>
        /// </remarks>
        public static string Quotemeta(this string source) {
            if (String.IsNullOrEmpty(source)) {
                return source;
            }
            return _regQuotemeta.Replace(
                source,
                (Match m) => "\\u" + String.Format("{0:x4}", (int)(m.Value[0]))
            );
        }

        /// <summary>
        /// escape non-word characters as unicode expression.
        /// </summary>
        /// <param name="source">raw strings</param>
        /// <returns>escaped strings</returns>
        public static IEnumerable<string> Quotemeta(this IEnumerable<string> source) {
            return source.Select(text => Quotemeta(text));
        }

        /// <summary>
        /// Remove all whitespace from string.
        /// </summary>
        /// <param name="text">The string that contains white space.</param>
        /// <returns>The white space removed string.</returns>
        /// <remarks>
        /// [c# - Efficient way to remove ALL whitespace from String? - Stack Overflow](http://stackoverflow.com/questions/6219454/)
        /// </remarks>
        public static string RemoveWhitespace(this string text) {
            if (text == null) {
                return null;
            }
            return String.Join("", text.Split());
        }
    }
}
