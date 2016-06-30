using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TakeAshUtility {

    public static class EnumHelper {

        /// <summary>
        /// Get localized string from the resource in the assembly.
        /// </summary>
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
        public static string ToLocalization(this Enum en, string assemblyName = null) {
            var assembly = AssemblyHelper.GetAssembly(assemblyName) ??
                Assembly.GetCallingAssembly();
            var resourceManager = assembly.GetResourceManager();
            return resourceManager == null ?
                null :
                resourceManager.GetString(en.ToResourceName());
        }

        /// <summary>
        /// Get the resource name for the enum.
        /// </summary>
        /// <param name="en">The enum.</param>
        /// <returns>The resource name.</returns>
        public static string ToResourceName(this Enum en) {
            var enumType = en.GetType();
            return (enumType.ReflectedType != null ? enumType.ReflectedType.Name + "_" : "") +
                enumType.Name + "_" +
                en.ToString();
        }
    }
}
