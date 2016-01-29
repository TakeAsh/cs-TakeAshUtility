using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TakeAshUtility {

    public static class TypeExtensionMethods {

        private static readonly BindingFlags _flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

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
    }
}
