using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace TakeAshUtility {

    public static class EnumHelper {

        /// <summary>
        /// Get the localized string for the enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="en">The enum.</param>
        /// <param name="assemblyName">The name to specify the assembly.</param>
        /// <param name="callingAssembly">The calling assembly.</param>
        /// <returns>
        /// The localized string that is searched from below list.
        /// <list type="bullet">
        /// <item>The assembly that is specified by the "assemblyName".</item>
        /// <item>The assembly that is specified by the "callingAssembly".</item>
        /// <item>The assembly that the enum is defined.</item>
        /// <item>The Description attribute of the enum.</item>
        /// <item>The name of the enum.</item>
        /// </list>
        /// </returns>
        public static string ToLocalizationEx<TEnum>(
            this TEnum en,
            string assemblyName = null,
            Assembly callingAssembly = null
        ) where TEnum : struct, IConvertible {

            return en.ToLocalization(AssemblyHelper.GetAssembly(assemblyName)) ??
                en.ToLocalization(callingAssembly ?? Assembly.GetCallingAssembly()) ??
                en.ToLocalization(typeof(TEnum).Assembly) ??
                en.ToDescription() ??
                en.ToString();
        }

        /// <summary>
        /// Get the localized string for the nullable enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="en">The nullable enum.</param>
        /// <param name="assemblyName">The name to specify the assembly.</param>
        /// <param name="callingAssembly">The calling assembly.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>The localized string, if the nullable enum is not null.</item>
        /// <item>null, if the nullable enum is null.</item>
        /// </list>
        /// </returns>
        public static string ToLocalizationEx<TEnum>(
            this Nullable<TEnum> en,
            string assemblyName = null,
            Assembly callingAssembly = null
        ) where TEnum : struct, IConvertible {

            return en == null ?
                null :
                en.Value.ToLocalizationEx(assemblyName, callingAssembly ?? Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Get the localized string for the enum from the resource in the assembly.
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
        /// Get the localized string for the enum from the resource in the assembly.
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
        /// Get the all values of Enum as TEnum type array.
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
        /// <param name="callingAssembly">The calling assembly.</param>
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
            var memInfos = typeof(TEnum).GetMember(en.ToString());
            if (memInfos.SafeCount() == 0 ||
                (attrs = memInfos.First().GetCustomAttributes(typeof(TAttr), false) as TAttr[]).SafeCount() == 0) {
                return null;
            }
            return predicate == null ?
                attrs.FirstOrDefault() :
                attrs.FirstOrDefault(predicate);
        }

        public static TAttr GetAttribute<TEnum, TAttr>(Func<TAttr, bool> predicate = null)
            where TEnum : struct, IConvertible
            where TAttr : Attribute {

            var attrs = typeof(TEnum).GetCustomAttributes(typeof(TAttr), false) as TAttr[];
            return attrs.SafeCount() == 0 ? null :
                predicate == null ? attrs.FirstOrDefault() :
                attrs.FirstOrDefault(predicate);
        }

        public static string ToDescription<TEnum>(this TEnum en)
            where TEnum : struct, IConvertible {

            var descriptionAttribute = en.GetAttribute<TEnum, DescriptionAttribute>();
            return descriptionAttribute == null ?
                null :
                descriptionAttribute.Description;
        }

        static public string GetEnumProperty<TEnum>(this TEnum en, string key)
            where TEnum : struct, IConvertible {

            if (key == null) {
                return null;
            }
            var enumProperty = en.GetAttribute<TEnum, EnumPropertyAttribute>(attr => attr.ContainsKey(key));
            return enumProperty == null ?
                null :
                enumProperty[key];
        }
    }
}
