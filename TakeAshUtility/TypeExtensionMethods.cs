using System;
using System.Collections.Generic;
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
    }
}
