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
    }

    public class Options {

        //[TypeConverter(typeof(EnumTypeConverter<NewLineCodes>))]
        public enum NewLineCodes {
            //[ExtraProperties("Entity:'\n', Escaped:'\\x22\\u0027\t'")]
            Lf = 1,

            //[ExtraProperties("Entity : \"\r\"Escaped : '\\x0027\\x0022'")]
            [System.ComponentModel.Description("[A] Mac(CR)")]
            Cr = 2,

            //[ExtraProperties("Entity:\t'\r\n';;;Escaped:\t\"\\x3042\"")] // U+3042 あ
            [System.ComponentModel.Description("[A] Windows(CR+LF)")]
            CrLf = 4,

            //[ExtraProperties("Entity:\n\t'\n\r'\nEscaped:\n\t'\\uD842\\uDFB7'")] // U+00020BB7 𠮷
            LfCr = 8,
        }
    }
}
