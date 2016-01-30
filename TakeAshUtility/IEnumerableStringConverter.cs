using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public class IEnumerableJoinConverter<T> :
        TypeConverter {

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType
        ) {
            if (destinationType == typeof(string)) {
                var enumerable = value as IEnumerable<T>;
                return enumerable == null ?
                    null :
                    String.Join(", ", enumerable);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class IEnumerableCountConverter<T> :
        TypeConverter {

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType
        ) {
            if (destinationType == typeof(string)) {
                var enumerable = value as IEnumerable<T>;
                return enumerable == null ?
                    null :
                    "Count:" + enumerable.Count();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class IEnumerableJoinWithCountConverter<T> :
        TypeConverter {

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType
        ) {
            if (destinationType == typeof(string)) {
                var enumerable = value as IEnumerable<T>;
                return enumerable == null ?
                    null :
                    "Count:" + enumerable.Count() + "; " + String.Join(", ", enumerable);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
