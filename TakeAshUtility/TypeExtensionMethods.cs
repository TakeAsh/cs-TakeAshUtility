using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace TakeAshUtility {

    public static class TypeExtensionMethods {

        private static readonly BindingFlags _flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static readonly BindingFlags _extensionMethodFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        private static List<MethodInfo> _allExtensionMethods;

        public static TAttr[] GetAttributes<TAttr>(this Type type, string propertyName)
            where TAttr : Attribute {

            if (type == null || String.IsNullOrEmpty(propertyName)) {
                return null;
            }
            var memInfos = type.GetMember(propertyName, _flags);
            if (memInfos == null || memInfos.Length == 0) {
                return null;
            }
            var attrs = memInfos.First().GetCustomAttributes(typeof(TAttr), false) as TAttr[];
            if (attrs == null || attrs.Length == 0) {
                return null;
            }
            return attrs;
        }

        public static TAttr GetAttribute<TAttr>(this Type type, string propertyName)
            where TAttr : Attribute {

            var attrs = type.GetAttributes<TAttr>(propertyName);
            return attrs == null || attrs.Length == 0 ?
                null :
                attrs.FirstOrDefault();
        }

        public static string ToDescription(this Type type, string propertyName) {
            if (type == null || String.IsNullOrEmpty(propertyName)) {
                return null;
            }
            string ret = null;
            var resManager = ResourceHelper.GetResourceManager(type);
            if (resManager != null &&
               !String.IsNullOrEmpty(ret = resManager.GetString(type.Name + "_" + propertyName))) {
                return ret;
            }
            var descriptionAttribute = type.GetAttribute<DescriptionAttribute>(propertyName);
            if (descriptionAttribute != null &&
                !String.IsNullOrEmpty(descriptionAttribute.Description)) {
                return descriptionAttribute.Description;
            }
            var pi = type.GetProperty(propertyName);
            return pi != null ?
                pi.Name :
                null;
        }

        /// <summary>
        /// Check if a class is derived from a generic class
        /// </summary>
        /// <param name="toCheck">a type of class to check</param>
        /// <param name="generic">a type of generic class</param>
        /// <returns>
        /// <list type="table">
        /// <item><term>true</term><description>toCheck is derived from generic</description></item>
        /// <item><term>false</term><description>toCheck is not derived from generic</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// [c# - Check if a class is derived from a generic class - Stack Overflow](http://stackoverflow.com/questions/457676/)
        /// </remarks>
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic) {
            while (toCheck != null && toCheck != typeof(object)) {
                var cur = toCheck.IsGenericType ?
                    toCheck.GetGenericTypeDefinition() :
                    toCheck;
                if (generic == cur) {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Get default value of the type at Runtime
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The default value of the type.</returns>
        /// <remarks>
        /// [c# - Default value of a type at Runtime - Stack Overflow](http://stackoverflow.com/questions/2490244/)
        /// </remarks>
        public static object GetDefaultValue(this Type type) {
            return type == null || !type.IsValueType ?
                null :
                Activator.CreateInstance(type);
        }

        public static MethodInfo GetExtensionMethod(this Type t, string methodName) {
            if (_allExtensionMethods == null) {
                _allExtensionMethods = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Aggregate(
                        new List<MethodInfo>(),
                        (current, assembly) => {
                            assembly.GetTypes()
                                .Where(type => type.IsSealed && !type.IsGenericType && !type.IsNested)
                                .ForEach(type => {
                                    var extensionMethods = type.GetMethods(_extensionMethodFlags)
                                        .Where(method => method.IsDefined(typeof(ExtensionAttribute), false));
                                    current.AddRange(extensionMethods);
                                });
                            return current;
                        }
                    );
            }
            return _allExtensionMethods.Where(method => method.GetParameters().First().ParameterType.IsAssignableFrom(t))
                .FirstOrDefault(method => method.Name == methodName);
        }
    }
}
