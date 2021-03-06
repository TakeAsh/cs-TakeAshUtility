﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    /// <summary>
    /// Safe ToString Method
    /// </summary>
    /// <remarks>
    /// [c# - Checking for null before ToString() - Stack Overflow](http://stackoverflow.com/questions/550374/)
    /// </remarks>
    public static class ObjectExtensionMethods {

        /// <summary>
        /// check null before ToString()
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="valueIfNull">value if object is null</param>
        /// <returns>
        /// <list type="table">
        /// <item><term>object is not null</term><description>object.ToString()</description></item>
        /// <item><term>object is null</term><description>valueIfNull</description></item>
        /// </list>
        /// </returns>
        public static string SafeToString(this object obj, string valueIfNull = "") {
            return obj != null ?
                obj.ToString() :
                valueIfNull;
        }

        /// <summary>
        /// check null before ToString(TOpt1)
        /// </summary>
        /// <typeparam name="TOpt1">Type of option1</typeparam>
        /// <param name="obj">object</param>
        /// <param name="opt1">option1</param>
        /// <param name="valueIfNull">value if object is null</param>
        /// <returns>
        /// <list type="table">
        /// <item><term>object is not null</term><description>object.ToString(TOpt1)</description></item>
        /// <item><term>object is null</term><description>valueIfNull</description></item>
        /// </list>
        /// </returns>
        public static string SafeToString<TOpt1>(this object obj, TOpt1 opt1, string valueIfNull = "") {
            if (obj == null) {
                return valueIfNull;
            }
            var mi = obj.GetType().GetMethod("ToString", new[] { typeof(TOpt1), });
            if (mi == null || mi.ReturnType != typeof(string)) {
                throw new NotImplementedException(
                    String.Format(
                        "Not Implemented: {0}.ToString({1})",
                        obj.GetType().Name, typeof(TOpt1).Name
                    )
                );
            }
            return (string)mi.Invoke(obj, new object[] { opt1, });
        }

        /// <summary>
        /// check null before ToString(TOpt1, TOpt2)
        /// </summary>
        /// <typeparam name="TOpt1">Type of option1</typeparam>
        /// <typeparam name="TOpt2">Type of option2</typeparam>
        /// <param name="obj">object</param>
        /// <param name="opt1">option1</param>
        /// <param name="opt2">option2</param>
        /// <param name="valueIfNull">value if object is null</param>
        /// <returns>
        /// <list type="table">
        /// <item><term>object is not null</term><description>object.ToString(TOpt1, TOpt2)</description></item>
        /// <item><term>object is null</term><description>valueIfNull</description></item>
        /// </list>
        /// </returns>
        public static string SafeToString<TOpt1, TOpt2>(this object obj, TOpt1 opt1, TOpt2 opt2, string valueIfNull = "") {
            if (obj == null) {
                return valueIfNull;
            }
            var mi = obj.GetType().GetMethod("ToString", new[] { typeof(TOpt1), typeof(TOpt2), });
            if (mi == null || mi.ReturnType != typeof(string)) {
                throw new NotImplementedException(
                    String.Format(
                        "Not Implemented: {0}.ToString({1}, {2})",
                        obj.GetType().Name, typeof(TOpt1).Name, typeof(TOpt2).Name
                    )
                );
            }
            return (string)mi.Invoke(obj, new object[] { opt1, opt2, });
        }

        /// <summary>
        /// check null before ToString(TOpt1, TOpt2, TOpt3)
        /// </summary>
        /// <typeparam name="TOpt1">Type of option1</typeparam>
        /// <typeparam name="TOpt2">Type of option2</typeparam>
        /// <typeparam name="TOpt3">Type of option3</typeparam>
        /// <param name="obj">object</param>
        /// <param name="opt1">option1</param>
        /// <param name="opt2">option2</param>
        /// <param name="opt3">option3</param>
        /// <param name="valueIfNull">value if object is null</param>
        /// <returns>
        /// <list type="table">
        /// <item><term>object is not null</term><description>object.ToString(TOpt1, TOpt2, TOpt3)</description></item>
        /// <item><term>object is null</term><description>valueIfNull</description></item>
        /// </list>
        /// </returns>
        public static string SafeToString<TOpt1, TOpt2, TOpt3>(this object obj, TOpt1 opt1, TOpt2 opt2, TOpt3 opt3, string valueIfNull = "") {
            if (obj == null) {
                return valueIfNull;
            }
            var mi = obj.GetType().GetMethod("ToString", new[] { typeof(TOpt1), typeof(TOpt2), typeof(TOpt3), });
            if (mi == null || mi.ReturnType != typeof(string)) {
                throw new NotImplementedException(
                    String.Format(
                        "Not Implemented: {0}.ToString({1}, {2}, {3})",
                        obj.GetType().Name, typeof(TOpt1).Name, typeof(TOpt2).Name, typeof(TOpt3).Name
                    )
                );
            }
            return (string)mi.Invoke(obj, new object[] { opt1, opt2, opt3, });
        }

        public static T SafeToObject<T>(this object source, T defaultValue = default(T)) {
            if (source == null) {
                return defaultValue;
            }
            var sourceType = source.GetType();
            try {
                var targetConverter = TypeDescriptor.GetConverter(typeof(T));
                if (targetConverter != null && targetConverter.CanConvertFrom(sourceType)) {
                    return (T)targetConverter.ConvertFrom(source);
                }
                var sourceConverter = TypeDescriptor.GetConverter(sourceType);
                if (sourceConverter != null && sourceConverter.CanConvertTo(typeof(T))) {
                    return (T)sourceConverter.ConvertTo(source, typeof(T));
                }
                if (typeof(T).IsAssignableFrom(sourceType)) {
                    return (T)source;
                }
                return defaultValue;
            }
            catch {
                return defaultValue;
            }
        }
    }
}
