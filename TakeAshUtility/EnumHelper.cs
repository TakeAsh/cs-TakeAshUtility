using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace TakeAshUtility {

    public static class EnumHelper {

        public static string ToLocalizationEx<TEnum>(
            this TEnum en,
            string assemblyName = null,
            Assembly callingAssembly = null
        ) where TEnum : struct, IConvertible {

            callingAssembly = callingAssembly ?? Assembly.GetCallingAssembly();
            return en.ToLocalization(AssemblyHelper.GetAssembly(assemblyName)) ??
                en.ToLocalization(callingAssembly) ??
                en.ToLocalization(typeof(TEnum).Assembly) ??
                en.ToDescription() ??
                en.ToString();
        }

        /// <summary>
        /// Get localized string from the resource in the assembly.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
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
        public static string ToLocalization<TEnum>(
            this TEnum en,
            string assemblyName = null
        ) where TEnum : struct, IConvertible {

            return en.ToLocalization(AssemblyHelper.GetAssembly(assemblyName)) ??
                en.ToLocalization(Assembly.GetCallingAssembly()) ??
                en.ToLocalization(typeof(TEnum).Assembly);
        }

        /// <summary>
        /// Get localized string from the resource in the assembly.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="en">The enum.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>The localized string, if exist.</item>
        /// <item>null, if not exist.</item>
        /// </list>
        /// </returns>
        public static string ToLocalization<TEnum>(this TEnum en, Assembly assembly)
            where TEnum : struct, IConvertible {

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
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="en">The enum.</param>
        /// <returns>The resource name.</returns>
        public static string ToResourceName<TEnum>(this TEnum en)
            where TEnum : struct, IConvertible {

            return (typeof(TEnum).ReflectedType != null ? typeof(TEnum).ReflectedType.Name + "_" : "") +
                typeof(TEnum).Name + "_" +
                en.ToString();
        }

        /// <summary>
        /// Get the all values of Enum as T type array.
        /// </summary>
        /// <typeparam name="TEnum">The type of Enum</typeparam>
        /// <returns>The Enum type values.</returns>
        public static TEnum[] GetValues<TEnum>()
            where TEnum : struct, IConvertible {

            return Enum.GetValues(typeof(TEnum)) as TEnum[];
        }

        /// <summary>
        /// Convert the enums to the localized KeyValuePairs.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="source">The enums.</param>
        /// <param name="assemblyName">
        /// The name of the assembly to be searched.
        /// If name is null, CallingAssembly is used.
        /// </param>
        /// <returns>The localized KeyValuePairs.</returns>
        public static KeyValuePair<TEnum, string>[] ToLocalizedPairs<TEnum>(
            this IEnumerable<TEnum> source,
            string assemblyName = null,
            Assembly callingAssembly = null
        ) where TEnum : struct, IConvertible {

            callingAssembly = callingAssembly ?? Assembly.GetCallingAssembly();
            return source.Select(en => new KeyValuePair<TEnum, string>(en, en.ToLocalizationEx(assemblyName, callingAssembly)))
                .SafeToArray();
        }

        public static TAttr GetAttribute<TEnum, TAttr>(this TEnum en, Func<TAttr, bool> predicate = null)
            where TEnum : struct, IConvertible
            where TAttr : Attribute {

            TAttr[] attrs;
            var memInfos = en.GetType().GetMember(en.ToString());
            if (memInfos.SafeCount() == 0 ||
                (attrs = memInfos.First().GetCustomAttributes(typeof(TAttr), false) as TAttr[]).SafeCount() == 0) {
                return null;
            }
            return predicate == null ?
                attrs.FirstOrDefault() :
                attrs.FirstOrDefault(predicate);
        }

        public static string ToDescription<TEnum>(this TEnum en)
            where TEnum : struct, IConvertible {

            var descriptionAttribute = en.GetAttribute<TEnum, DescriptionAttribute>();
            return descriptionAttribute == null ?
                null :
                descriptionAttribute.Description;
        }
    }
}
