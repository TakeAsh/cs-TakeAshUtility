using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    using NewLineCodesLocalizedPair = KeyValuePair<Options.NewLineCodes, string>;

    [TestFixture]
    class EnumHelper_Test {

        private Options.NewLineCodes[] _newLineCodesValues = new[] {
            Options.NewLineCodes.Lf,
            Options.NewLineCodes.Cr,
            Options.NewLineCodes.CrLf,
            Options.NewLineCodes.LfCr,
        };

        private string[] _langs = new[] { "en-US", "ja-JP", };

        private string[][] _newLineCodesLocalizations = new[] {
            // en-US
            new[] {
                "[R_en] Unix(LF)",
                "[R_en] Mac(CR)",
                "[A] Windows(CR+LF)",
                "LfCr",
            },
            // ja-JP
            new[] {
                "[R_ja] ユニックス(LF)",
                "[R_en] Mac(CR)",
                "[R_ja] ウィンドウズ(CR+LF)",
                "LfCr",
            },
        };

        private NewLineCodesLocalizedPair[][] _newLineCodesLocalizedPairs = new[] {
            // en-US
            new[] {
                new NewLineCodesLocalizedPair(Options.NewLineCodes.Lf, "[R_en] Unix(LF)"),
                new NewLineCodesLocalizedPair(Options.NewLineCodes.Cr, "[R_en] Mac(CR)"),
                new NewLineCodesLocalizedPair(Options.NewLineCodes.CrLf, "[A] Windows(CR+LF)"),
                new NewLineCodesLocalizedPair(Options.NewLineCodes.LfCr, "LfCr"),
            },
            // ja-JP
            new[] {
                new NewLineCodesLocalizedPair(Options.NewLineCodes.Lf, "[R_ja] ユニックス(LF)"),
                new NewLineCodesLocalizedPair(Options.NewLineCodes.Cr, "[R_en] Mac(CR)"),
                new NewLineCodesLocalizedPair(Options.NewLineCodes.CrLf, "[R_ja] ウィンドウズ(CR+LF)"),
                new NewLineCodesLocalizedPair(Options.NewLineCodes.LfCr, "LfCr"),
            },
        };

        [Flags]
        [HexDigit(2)]
        [TypeConverter(typeof(EnumTypeConverter<WDays>))]
        public enum WDays {
            None = 0x00,
            Sunday = 0x01,
            Monday = 0x02,
            Tuesday = 0x04,
            Wednesday = 0x08,
            Thursday = 0x10,
            Friday = 0x20,
            Saturday = 0x40
        }

        private void SetCurrentCulture(string cultureName) {
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestCase("NewLineCodes")]
        public void Values_Test(string enumName) {
            CollectionAssert.AreEqual(_newLineCodesValues, EnumHelper.GetValues<Options.NewLineCodes>());
        }

        [TestCase(0)]
        [TestCase(1)]
        public void ToLocalizationEx_Test(int index) {
            var culture = _langs[index];
            SetCurrentCulture(culture);
            var actual1 = EnumHelper.GetValues<Options.NewLineCodes>()
                .Select(item => item.ToLocalizationEx());
            CollectionAssert.AreEqual(_newLineCodesLocalizations[index], actual1, culture);
            var converter = TypeDescriptor.GetConverter(typeof(Options.NewLineCodes));
            var actual2 = EnumHelper.GetValues<Options.NewLineCodes>()
                .Select(item => converter.ConvertToString(item));
            CollectionAssert.AreEqual(_newLineCodesLocalizations[index], actual2, culture);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void ToLocalizedPairs_Test(int index) {
            var culture = _langs[index];
            SetCurrentCulture(culture);
            var actual = EnumHelper.GetValues<Options.NewLineCodes>()
                .ToLocalizedPairs();
            CollectionAssert.AreEqual(_newLineCodesLocalizedPairs[index], actual, culture);
        }

        [TestCase(Options.NewLineCodes.Lf, "\n", "\"\'\t")]
        [TestCase(Options.NewLineCodes.Cr, "\r", "\'\"")]
        [TestCase(Options.NewLineCodes.CrLf, "\r\n", "あ")]
        [TestCase(Options.NewLineCodes.LfCr, "\n\r", "\uD842\uDFB7")]
        [TestCase((Options.NewLineCodes)2, "\n", "\x22\u0027\x09")]
        [TestCase((Options.NewLineCodes)8, "\r", "\u0027\x22")]
        [TestCase((Options.NewLineCodes)16, "\r\n", "\u3042")]
        [TestCase((Options.NewLineCodes)64, "\n\r", "\xD842\xDFB7")]
        [TestCase((Options.NewLineCodes)0, null, null)]
        [TestCase((Options.NewLineCodes)3, null, null)]
        public void GetEnumProperty_Test(Options.NewLineCodes item, string expectedEntity, string expectedEscaped) {
            var actualEntity = item.GetEnumProperty(Options.EntityProperty);
            if (expectedEntity != null) {
                Assert.AreEqual(expectedEntity, actualEntity);
            } else {
                Assert.Null(actualEntity);
            }
            var actualEscaped = item.GetEnumProperty(Options.EscapedProperty);
            if (expectedEscaped != null) {
                Assert.AreEqual(expectedEscaped, actualEscaped);
            } else {
                Assert.Null(actualEscaped);
            }
        }

        [TestCase(WDays.None, "00", "00: None")]
        [TestCase(WDays.Sunday, "01", "01: Sunday")]
        [TestCase(WDays.Monday, "02", "02: Monday")]
        [TestCase(WDays.Tuesday, "04", "04: Tuesday")]
        [TestCase(WDays.Wednesday, "08", "08: Wednesday")]
        [TestCase(WDays.Thursday, "10", "10: Thursday")]
        [TestCase(WDays.Friday, "20", "20: Friday")]
        [TestCase(WDays.Saturday, "40", "40: Saturday")]
        [TestCase(WDays.Sunday | WDays.Monday, "03", "03: Sunday, Monday")]
        [TestCase(WDays.Monday | WDays.Tuesday, "06", "06: Monday, Tuesday")]
        [TestCase(WDays.Tuesday | WDays.Wednesday, "0C", "0C: Tuesday, Wednesday")]
        [TestCase(WDays.Wednesday | WDays.Thursday, "18", "18: Wednesday, Thursday")]
        [TestCase(WDays.Thursday | WDays.Friday, "30", "30: Thursday, Friday")]
        [TestCase(WDays.Friday | WDays.Saturday, "60", "60: Friday, Saturday")]
        [TestCase(WDays.Saturday | WDays.Sunday, "41", "41: Sunday, Saturday")]
        [TestCase(WDays.None | WDays.Sunday | WDays.Monday | WDays.Tuesday | WDays.Wednesday | WDays.Thursday | WDays.Friday | WDays.Saturday, "7F", "7F: Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday")]
        public void ToHex_WDays_Test(WDays wday, string expectedHex, string expectedHexFlags) {
            Assert.AreEqual(expectedHex, wday.ToHex());
            Assert.AreEqual(expectedHexFlags, wday.ToHexWithFlags());
        }

        [TestCase(Options.NewLineCodes.Lf, "2")]
        [TestCase(Options.NewLineCodes.Cr, "8")]
        [TestCase(Options.NewLineCodes.CrLf, "10")]
        [TestCase(Options.NewLineCodes.LfCr, "40")]
        [TestCase(Options.NewLineCodes.Lf | Options.NewLineCodes.Cr, "A")]
        [TestCase(Options.NewLineCodes.Cr | Options.NewLineCodes.CrLf, "18")]
        [TestCase(Options.NewLineCodes.CrLf | Options.NewLineCodes.LfCr, "50")]
        [TestCase(Options.NewLineCodes.LfCr | Options.NewLineCodes.Lf, "42")]
        [TestCase(Options.NewLineCodes.Lf | Options.NewLineCodes.Cr | Options.NewLineCodes.CrLf | Options.NewLineCodes.LfCr, "5A")]
        public void ToHex_NewLineCodes_Test(Options.NewLineCodes code, string expect) {
            Assert.AreEqual(expect, code.ToHex());
        }

        [TestCase(BkBRMGCYW.Black, "Bk", ColorGroups.Undefined)]
        [TestCase(BkBRMGCYW.Blue, "B", ColorGroups.Primary)]
        [TestCase(BkBRMGCYW.Red, "R", ColorGroups.Primary)]
        [TestCase(BkBRMGCYW.Magenta, "M", ColorGroups.Secondary)]
        [TestCase(BkBRMGCYW.Green, "G", ColorGroups.Primary)]
        [TestCase(BkBRMGCYW.Cyan, "C", ColorGroups.Secondary)]
        [TestCase(BkBRMGCYW.Yellow, "Y", ColorGroups.Secondary)]
        [TestCase(BkBRMGCYW.White, "White", ColorGroups.Other)]
        public void BkBRMGCYW_Test(BkBRMGCYW input, string expectedShortName, ColorGroups expectedGroup) {
            Assert.AreEqual(expectedShortName, input.ToShortName());
            Assert.AreEqual(expectedGroup, input.ToGroup());
        }
    }

    public static class Options {

        public const string EntityProperty = "Entity";
        public const string EscapedProperty = "Escaped";

        [TypeConverter(typeof(EnumTypeConverter<NewLineCodes>))]
        public enum NewLineCodes {
            [EnumProperty(EntityProperty + ":'\n'")]
            [EnumProperty(EscapedProperty + ":'\\x22\\u0027\t'")]
            Lf = 2,

            [EnumProperty(EntityProperty + " : \"\r\"" + EscapedProperty + " : '\\x0027\\x0022'")]
            [System.ComponentModel.Description("[A] Mac(CR)")]
            Cr = 8,

            [EnumProperty(EntityProperty + ":\t'\r\n';;;")]
            [EnumProperty(EscapedProperty + ":\t\"\\x3042\"")] // U+3042 あ
            [System.ComponentModel.Description("[A] Windows(CR+LF)")]
            CrLf = 16,

            [EnumProperty(EntityProperty + ":\n\t'\n\r'\n" + EscapedProperty + ":\n\t'\\uD842\\uDFB7'")] // U+00020BB7 𠮷
            LfCr = 64,
        }
    }

    public enum BkBRMGCYW {
        [EnumProperty(BkBRMGCYWHelper.ShortNameProperty + ":'Bk'")]
        Black,
        [EnumProperty(BkBRMGCYWHelper.ShortNameProperty + ":'B'")]
        [EnumProperty(BkBRMGCYWHelper.GroupProperty + ":'Primary'")]
        Blue,
        [EnumProperty(BkBRMGCYWHelper.ShortNameProperty + ":'R'")]
        [EnumProperty(BkBRMGCYWHelper.GroupProperty + ":'Primary'")]
        Red,
        [EnumProperty(BkBRMGCYWHelper.ShortNameProperty + ":'M'")]
        [EnumProperty(BkBRMGCYWHelper.GroupProperty + ":'Secondary'")]
        Magenta,
        [EnumProperty(BkBRMGCYWHelper.ShortNameProperty + ":'G'")]
        [EnumProperty(BkBRMGCYWHelper.GroupProperty + ":'Primary'")]
        Green,
        [EnumProperty(BkBRMGCYWHelper.ShortNameProperty + ":'C'")]
        [EnumProperty(BkBRMGCYWHelper.GroupProperty + ":'Secondary'")]
        Cyan,
        [EnumProperty(BkBRMGCYWHelper.ShortNameProperty + ":'Y'")]
        [EnumProperty(BkBRMGCYWHelper.GroupProperty + ":'Secondary'")]
        Yellow,
        [EnumProperty(BkBRMGCYWHelper.GroupProperty + ":'Other'")]
        White,
    }

    public enum ColorGroups {
        Undefined,
        Primary,
        Secondary,
        Other,
    }

    public static class BkBRMGCYWHelper {

        public const string ShortNameProperty = "ShortName";
        public const string GroupProperty = "Group";

        public static string ToShortName(this BkBRMGCYW en) {
            return en.GetEnumProperty(ShortNameProperty) ?? en.ToString();
        }

        public static ColorGroups ToGroup(this BkBRMGCYW en) {
            return en.GetEnumProperty(GroupProperty)
                .TryParse<ColorGroups>();
        }
    }
}
