using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace TakeAshUtility {

    public static class EnumHelper {

        /// <summary>
        /// Get localized string from the resource in the assembly.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="en">The enum.</param>
        /// <param name="assemblyName">
        /// The name of the assembly to be searched.
        /// If name is null, CallingAssembly is used.
        /// </param>
        /// <returns>
        /// <list type="bullet">
        /// <item>The localized string, if exist.</item>
        /// <item>null, if not exist.</item>
        /// </list>
        /// </returns>
        public static string ToLocalization<T>(this T en, string assemblyName = null)
            where T : struct, IConvertible {

            return en.ToLocalization(AssemblyHelper.GetAssembly(assemblyName)) ??
                en.ToLocalization(Assembly.GetCallingAssembly()) ??
                en.ToLocalization(en.GetType().Assembly);
        }

        /// <summary>
        /// Get localized string from the resource in the assembly.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="en">The enum.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>The localized string, if exist.</item>
        /// <item>null, if not exist.</item>
        /// </list>
        /// </returns>
        public static string ToLocalization<T>(this T en, Assembly assembly)
            where T : struct, IConvertible {

            ResourceManager resourceManager;
            if (assembly == null ||
                (resourceManager = assembly.GetResourceManager()) == null) {
                return null;
            }
            return resourceManager.GetString(en.ToResourceName());
        }

        /// <summary>
        /// Get the resource name for the enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="en">The enum.</param>
        /// <returns>The resource name.</returns>
        public static string ToResourceName<T>(this T en)
            where T : struct, IConvertible {

            var enumType = en.GetType();
            return (enumType.ReflectedType != null ? enumType.ReflectedType.Name + "_" : "") +
                enumType.Name + "_" +
                en.ToString();
        }

        /// <summary>
        /// Get the all values of Enum as T type array.
        /// </summary>
        /// <typeparam name="T">The type of Enum</typeparam>
        /// <returns>The Enum type values.</returns>
        public static T[] GetValues<T>()
            where T : struct, IConvertible {

            return Enum.GetValues(typeof(T)) as T[];
        }

        /// <summary>
        /// Convert the enums to the localized KeyValuePairs.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="source">The enums.</param>
        /// <param name="assemblyName">
        /// The name of the assembly to be searched.
        /// If name is null, CallingAssembly is used.
        /// </param>
        /// <returns>The localized KeyValuePairs.</returns>
        public static KeyValuePair<T, string>[] ToLocalizedPairs<T>(
            this IEnumerable<T> source,
            string assemblyName = null
        ) where T : struct, IConvertible {

            var assembly = AssemblyHelper.GetAssembly(assemblyName) ??
                Assembly.GetCallingAssembly();
            return source.ToLocalizedPairs(assembly);
        }

        /// <summary>
        /// Convert the enums to the localized KeyValuePairs.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="source">The enums.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The localized KeyValuePairs.</returns>
        public static KeyValuePair<T, string>[] ToLocalizedPairs<T>(
            this IEnumerable<T> source,
            Assembly assembly
        ) where T : struct, IConvertible {

            if (source.SafeCount() == 0 ||
                assembly == null) {
                return null;
            }
            return source.Select(en => en.ToLocalizedPair(assembly))
                .SafeToArray();
        }

        /// <summary>
        /// Get the localized KeyValuePair.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="en">The enum.</param>
        /// <param name="assemblyName">
        /// The name of the assembly to be searched.
        /// If name is null, CallingAssembly is used.
        /// </param>
        /// <returns>The localized KeyValuePair.</returns>
        public static KeyValuePair<T, string> ToLocalizedPair<T>(
            this T en,
            string assemblyName = null
        ) where T : struct, IConvertible {

            var assembly = AssemblyHelper.GetAssembly(assemblyName) ??
                Assembly.GetCallingAssembly();
            return en.ToLocalizedPair(assembly);
        }

        /// <summary>
        /// Get the localized KeyValuePair.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="en">The enum.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The localized KeyValuePair.</returns>
        public static KeyValuePair<T, string> ToLocalizedPair<T>(
            this T en,
            Assembly assembly
        ) where T : struct, IConvertible {

            return new KeyValuePair<T, string>(
                en,
                en.ToLocalization(assembly) ?? en.ToString()
            );
        }
    }
}
