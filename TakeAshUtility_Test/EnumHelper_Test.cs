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
        //[TypeConverter(typeof(EnumTypeConverter<WDays>))]
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
            var actual = EnumHelper.GetValues<Options.NewLineCodes>()
                .Select(item => item.ToLocalizationEx());
            CollectionAssert.AreEqual(_newLineCodesLocalizations[index], actual, culture);
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

        [TestCase(WDays.None, "00")]
        [TestCase(WDays.Sunday, "01")]
        [TestCase(WDays.Monday, "02")]
        [TestCase(WDays.Tuesday, "04")]
        [TestCase(WDays.Wednesday, "08")]
        [TestCase(WDays.Thursday, "10")]
        [TestCase(WDays.Friday, "20")]
        [TestCase(WDays.Saturday, "40")]
        [TestCase(WDays.Sunday | WDays.Monday, "03")]
        [TestCase(WDays.Monday | WDays.Tuesday, "06")]
        [TestCase(WDays.Tuesday | WDays.Wednesday, "0C")]
        [TestCase(WDays.Wednesday | WDays.Thursday, "18")]
        [TestCase(WDays.Thursday | WDays.Friday, "30")]
        [TestCase(WDays.Friday | WDays.Saturday, "60")]
        [TestCase(WDays.Saturday | WDays.Sunday, "41")]
        [TestCase(WDays.None | WDays.Sunday | WDays.Monday | WDays.Tuesday | WDays.Wednesday | WDays.Thursday | WDays.Friday | WDays.Saturday, "7F")]
        public void ToHex_WDays_Test(WDays wday, string expect) {
            Assert.AreEqual(expect, wday.ToHex());
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
    }

    public static class Options {

        public const string EntityProperty = "Entity";
        public const string EscapedProperty = "Escaped";

        //[TypeConverter(typeof(EnumTypeConverter<NewLineCodes>))]
        [Flags]
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
}
