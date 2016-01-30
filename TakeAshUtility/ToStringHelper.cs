using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TakeAshUtility {

    public class ToStringMemberAttribute :
        Attribute {

        private string _format;
        private Type _typeConverterType;
        private TypeConverter _typeConverter;

        public ToStringMemberAttribute() { }

        public ToStringMemberAttribute(string name) {
            Name = name;
        }

        public ToStringMemberAttribute(string name, string format) {
            Name = name;
            Format = format;
        }

        public ToStringMemberAttribute(string name, Type typeConverter) {
            Name = name;
            TypeConverter = typeConverter;
        }

        public ToStringMemberAttribute(Type typeConverter) {
            TypeConverter = typeConverter;
        }

        public string Name { get; set; }

        public string Format {
            get { return _format; }
            set { _format = "{0:" + value + "}"; }
        }

        public Type TypeConverter {
            get { return _typeConverterType; }
            set {
                _typeConverterType = value;
                _typeConverter = _typeConverterType == null ?
                    null :
                    Activator.CreateInstance(TypeConverter) as TypeConverter;
            }
        }

        public string ToString(string name, Type type, object value) {
            var valueString = _typeConverter != null ? _typeConverter.ConvertToString(value) :
                !String.IsNullOrEmpty(Format) ? String.Format(Format, value) :
                value;
            return (Name ?? name) + ":" +
                (type.IsPrimitive ?
                    valueString :
                    "{" + valueString + "}");
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
        public static string ToStringMembers(this object obj, string separator = ", ") {
            if (obj == null) {
                return null;
            }
            var objType = obj.GetType();
            var properties = objType.GetProperties(_flags)
                .Select(property => {
                    var printMember = objType.GetAttribute<ToStringMemberAttribute>(property.Name);
                    return printMember == null ?
                        null :
                        printMember.ToString(
                            property.Name,
                            property.PropertyType,
                            property.GetValue(obj, null)
                        );
                }).Where(item => item != null);
            var fields = objType.GetFields(_flags)
                .Select(field => {
                    var printMember = objType.GetAttribute<ToStringMemberAttribute>(field.Name);
                    return printMember == null ?
                        null :
                        printMember.ToString(
                            field.Name,
                            field.FieldType,
                            field.GetValue(obj)
                        );
                }).Where(item => item != null);
            return String.Join(separator, properties.Concat(fields));
        }
    }
}
