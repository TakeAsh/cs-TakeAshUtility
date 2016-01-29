using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TakeAshUtility {

    public class PrintMemberAttribute :
        Attribute {

        private string _format;

        public PrintMemberAttribute() { }

        public PrintMemberAttribute(string name) {
            Name = name;
        }

        public PrintMemberAttribute(string name, string format) {
            Name = name;
            Format = format;
        }

        public string Name { get; set; }

        public string Format {
            get { return _format; }
            set { _format = "{0:" + value + "}"; }
        }
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
                    var printMember = objType.GetAttribute<PrintMemberAttribute>(property.Name);
                    if (printMember == null) {
                        return null;
                    }
                    var valueString = String.IsNullOrEmpty(printMember.Format) ?
                        property.GetValue(obj, null) :
                        String.Format(printMember.Format, property.GetValue(obj, null));
                    return (printMember.Name ?? property.Name) + ":" +
                        (property.PropertyType.IsPrimitive ?
                            valueString :
                            "{" + valueString + "}");
                }).Where(item => item != null);
            var fields = objType.GetFields(_flags)
                .Select(field => {
                    var printMember = objType.GetAttribute<PrintMemberAttribute>(field.Name);
                    if (printMember == null) {
                        return null;
                    }
                    var valueString = String.IsNullOrEmpty(printMember.Format) ?
                        field.GetValue(obj) :
                        String.Format(printMember.Format, field.GetValue(obj));
                    return (printMember.Name ?? field.Name) + ":" +
                        (field.FieldType.IsPrimitive ?
                            valueString :
                            "{" + valueString + "}");
                }).Where(item => item != null);
            return String.Join(separator, properties.Concat(fields));
        }
    }
}
