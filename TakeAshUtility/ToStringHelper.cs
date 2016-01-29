using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TakeAshUtility {

    public class PrintMemberAttribute :
        Attribute {

        public PrintMemberAttribute() { }

        public PrintMemberAttribute(string format) {
            Format = "{0:" + format + "}";
        }

        public string Format { get; set; }
    }

    public static class ToStringHelper {

        private static readonly BindingFlags _flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Returns properties and fields that have PrintMember attribute.
        /// </summary>
        /// <param name="obj">Object to be converted to string</param>
        /// <param name="separator">Separator between properties and fields</param>
        /// <returns>properties and fields</returns>
        public static string MembersToString(this object obj, string separator = ", ") {
            if (obj == null) {
                return null;
            }
            var objType = obj.GetType();
            var properties = objType.GetProperties(_flags)
                .Select(property => {
                    var attr = objType.GetAttribute<PrintMemberAttribute>(property.Name);
                    if (attr == null) {
                        return null;
                    }
                    var valueString = String.IsNullOrEmpty(attr.Format) ?
                        property.GetValue(obj, null) :
                        String.Format(attr.Format, property.GetValue(obj, null));
                    return property.Name + ":" + (property.PropertyType.IsPrimitive ?
                        valueString :
                        "{" + valueString + "}");
                }).Where(item => item != null);
            var fields = objType.GetFields(_flags)
                .Select(field => {
                    var attr = objType.GetAttribute<PrintMemberAttribute>(field.Name);
                    if (attr == null) {
                        return null;
                    }
                    var valueString = String.IsNullOrEmpty(attr.Format) ?
                        field.GetValue(obj) :
                        String.Format(attr.Format, field.GetValue(obj));
                    return field.Name + ":" + (field.FieldType.IsPrimitive ?
                        valueString :
                        "{" + valueString + "}");
                }).Where(item => item != null);
            return String.Join(separator, properties.Concat(fields));
        }
    }
}
