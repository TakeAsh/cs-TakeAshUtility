using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    /// <summary>
    /// Provides ToLocalizationEx() as string converter.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    /// <remarks>
    /// <list type="bullet">
    /// <item>[c# - Data bind enum properties to grid and display description - Stack Overflow](http://stackoverflow.com/questions/1540103/)</item>
    /// </list>
    /// </remarks>
    public class EnumTypeConverter<TEnum> :
        EnumConverter
        where TEnum : struct, IConvertible {

        public EnumTypeConverter() : base(typeof(TEnum)) { }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType
        ) {
            if (destinationType == typeof(string) &&
                value is TEnum) {
                var enumValue = (TEnum)value;
                return EnumHelper.GetAttribute<TEnum, FlagsAttribute>() == null ?
                    enumValue.ToLocalizationEx() :
                    enumValue.ToHexWithFlags();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
