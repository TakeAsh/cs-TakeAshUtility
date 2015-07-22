using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class TypeExtensionMethods {

        public static TAttr[] GetAttributes<TAttr>(this Type type, string propertyName)
            where TAttr : Attribute {

            if (type == null || String.IsNullOrEmpty(propertyName)) {
                return null;
            }
            var memInfos = type.GetMember(propertyName);
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
    }
}
