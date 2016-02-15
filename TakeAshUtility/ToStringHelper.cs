using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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
        /// Returns properties and fields that have ToStringMember attribute.
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
                    var toStringMember = objType.GetAttribute<ToStringMemberAttribute>(property.Name);
                    return toStringMember == null ?
                        null :
                        toStringMember.ToString(
                            property.Name,
                            property.PropertyType,
                            property.GetValue(obj, null)
                        );
                }).Where(item => item != null);
            var fields = objType.GetFields(_flags)
                .Select(field => {
                    var toStringMember = objType.GetAttribute<ToStringMemberAttribute>(field.Name);
                    return toStringMember == null ?
                        null :
                        toStringMember.ToString(
                            field.Name,
                            field.FieldType,
                            field.GetValue(obj)
                        );
                }).Where(item => item != null);
            return String.Join(separator, properties.Concat(fields));
        }

        public static Dictionary<string, string> ToHash(this string text, string separator = ",") {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(separator)) {
                return null;
            }
            var regNonPrimitiveValue = new Regex(@"(\{[^\}]+\})");
            var escapedSeparator = "[[" + String.Join("", Encoding.UTF8.GetBytes(separator).Select(x => x.ToString("X2"))) + "]]";
            text = regNonPrimitiveValue.Replace(text, (m) => m.Value.Replace(separator, escapedSeparator));
            return text.SplitTrim(new[] { separator })
                .Select(pair => {
                    var index = pair.IndexOf(":");
                    var key = pair.Substring(0, index);
                    var value = pair.Substring(index + 1);
                    if (value != null && value.First() == '{' && value.Last() == '}') {
                        value = value.Substring(1, value.Length - 2)
                            .Replace(escapedSeparator, separator);
                    }
                    return new KeyValuePair<string, string>(key, value);
                }).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
