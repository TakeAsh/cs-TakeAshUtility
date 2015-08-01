using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TakeAshUtility {

    public interface IItems<TEnum>
        where TEnum : struct, IConvertible {

        string this[TEnum item] { get; set; }
    }

    public static class IItemsExtensionMethods {

        public const string ItemsSeparator = "\n";
        public const string KeyValueSeparator = "\t";

        public static string ToString<TIItems, TEnum>(this TIItems obj)
            where TIItems : IItems<TEnum>, new()
            where TEnum : struct, IConvertible {

            if (obj == null) {
                return null;
            }
            return String.Join(
                ItemsSeparator,
                GetValues<TEnum>()
                    .Where(item => obj[item] != null)
                    .Select(item => item.ToString() + KeyValueSeparator + Regex.Escape(obj[item]))
            );
        }

        public static TIItems FromString<TIItems, TEnum>(this string text)
            where TIItems : IItems<TEnum>, new()
            where TEnum : struct, IConvertible {

            if (String.IsNullOrEmpty(text)) {
                return default(TIItems);
            }
            var obj = new TIItems();
            var isValid = true;
            text.SplitTrim(new[] { ItemsSeparator })
                .ToList()
                .ForEach(item => {
                    var fragments = item.Split(new[] { KeyValueSeparator }, StringSplitOptions.None);
                    TEnum key;
                    if (Enum.TryParse<TEnum>(fragments.FirstOrDefault(), out key)) {
                        obj[key] = Regex.Unescape(String.Join(KeyValueSeparator, fragments.Skip(1)));
                    } else {
                        isValid = false;
                        return;
                    }
                });
            return isValid ?
                obj :
                default(TIItems);
        }

        public static bool Equals<TIItems, TEnum>(this TIItems obj, Object other)
            where TIItems : IItems<TEnum>, new()
            where TEnum : struct, IConvertible {

            if (Object.ReferenceEquals(obj, other)) {
                return true;
            }
            if ((Object)obj == null ||
                (Object)other == null ||
                !(other is TIItems)) {
                return false;
            }
            var objB = (TIItems)other;
            return GetValues<TEnum>()
                .Aggregate(true, (current, item) => current && (obj[item] == objB[item]));
        }

        private static TEnum[] GetValues<TEnum>()
            where TEnum : struct, IConvertible {

            return (TEnum[])Enum.GetValues(typeof(TEnum));
        }
    }
}
