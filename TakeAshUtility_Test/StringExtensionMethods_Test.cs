﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class StringExtensionMethods_Test {

        const int DefaultIntValue = -100;
        const double DefaultDoubleValue = -100;
        static readonly DateTime DefaultDateTimeValue = new DateTime(2010, 11, 12, 13, 14, 15);
        const string DefaultStringValue = "xyz";

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("2", 2)]
        [TestCase("-1", -1)]
        [TestCase("-2", -2)]
        [TestCase("a", 0)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        public void TryParse_Int_Test(string input, int expected) {
            Assert.AreEqual(expected, input.TryParse<int>());
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("2", 2)]
        [TestCase("-1", -1)]
        [TestCase("-2", -2)]
        [TestCase("a", DefaultIntValue)]
        [TestCase("", DefaultIntValue)]
        [TestCase(null, DefaultIntValue)]
        public void TryParse_Int_WithDefalut_Test(string input, int expected) {
            Assert.AreEqual(expected, input.TryParse(DefaultIntValue));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("2", 2)]
        [TestCase("-1", -1)]
        [TestCase("-2", -2)]
        [TestCase("a", null)]
        [TestCase("", null)]
        [TestCase(null, null)]
        public void TryParse_IntNullable_Test(string input, int? expected) {
            var actual = input.TryParse<int?>();
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsNull(actual);
            }
        }

        [TestCase("0.0", 0)]
        [TestCase("1.0", 1)]
        [TestCase("2.0", 2)]
        [TestCase("-1.0", -1)]
        [TestCase("-2.0", -2)]
        [TestCase("a", 0)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        public void TryParse_Double_Test(string input, double expected) {
            Assert.AreEqual(expected, input.TryParse<double>());
        }

        [TestCase("0.0", 0)]
        [TestCase("1.0", 1)]
        [TestCase("2.0", 2)]
        [TestCase("-1.0", -1)]
        [TestCase("-2.0", -2)]
        [TestCase("a", DefaultDoubleValue)]
        [TestCase("", DefaultDoubleValue)]
        [TestCase(null, DefaultDoubleValue)]
        public void TryParse_Double_WithDefalut_Test(string input, double expected) {
            Assert.AreEqual(expected, input.TryParse(DefaultDoubleValue));
        }

        [TestCase("0.0", 0.0)]
        [TestCase("1.0", 1.0)]
        [TestCase("2.0", 2.0)]
        [TestCase("-1.0", -1.0)]
        [TestCase("-2.0", -2.0)]
        [TestCase("a", null)]
        [TestCase("", null)]
        [TestCase(null, null)]
        public void TryParse_DoubleNullable_Test(string input, double? expected) {
            var actual = input.TryParse<double?>();
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsNull(actual);
            }
        }

        [TestCase("2015/06/11 01:02:03", "2015-06-11 01:02:03")]
        [TestCase("a", "0001-01-01 00:00:00")]
        [TestCase("", "0001-01-01 00:00:00")]
        [TestCase(null, "0001-01-01 00:00:00")]
        public void TryParse_DateTime_Test(string input, string expected) {
            DateTime expectedDate;
            DateTime.TryParse(expected, out expectedDate);
            Assert.AreEqual(expectedDate, input.TryParse<DateTime>());
        }

        [TestCase("2015/06/11 01:02:03", "2015-06-11 01:02:03")]
        [TestCase("a", "2010-11-12 13:14:15")]
        [TestCase("", "0001-01-01 00:00:00")]
        [TestCase(null, "2010-11-12 13:14:15")]
        public void TryParse_DateTime_WithDefalut_Test(string input, string expected) {
            DateTime expectedDate;
            DateTime.TryParse(expected, out expectedDate);
            Assert.AreEqual(expectedDate, input.TryParse(DefaultDateTimeValue));
        }

        public enum Separators {
            Tab,
            Space,
            Comma,
            Semicolon,
        }

        [TestCase("Tab", Separators.Tab)]
        [TestCase("Space", Separators.Space)]
        [TestCase("Comma", Separators.Comma)]
        [TestCase("Semicolon", Separators.Semicolon)]
        [TestCase("a", Separators.Tab)]
        [TestCase("", Separators.Tab)]
        [TestCase(null, Separators.Tab)]
        public void TryParse_Enum_Test(string input, Separators expected) {
            Assert.AreEqual(expected, input.TryParse<Separators>());
        }

        [TestCase("Tab", Separators.Tab)]
        [TestCase("Space", Separators.Space)]
        [TestCase("Comma", Separators.Comma)]
        [TestCase("Semicolon", Separators.Semicolon)]
        [TestCase("a", Separators.Comma)]
        [TestCase("", Separators.Comma)]
        [TestCase(null, Separators.Comma)]
        public void TryParse_Enum_WithDefalut_Test(string input, Separators expected) {
            Assert.AreEqual(expected, input.TryParse(Separators.Comma));
        }

        [TestCase("Tab", Separators.Tab)]
        [TestCase("Space", Separators.Space)]
        [TestCase("Comma", Separators.Comma)]
        [TestCase("Semicolon", Separators.Semicolon)]
        [TestCase("a", null)]
        [TestCase("", null)]
        [TestCase(null, null)]
        public void TryParse_EnumNullable_Test(string input, Separators? expected) {
            var actual = input.TryParse<Separators?>();
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsNull(actual);
            }
        }

        [TestCase("http://example.com", false, "http", "example.com", "/", 80)]
        [TestCase("http://example.net:8080", false, "http", "example.net", "/", 8080)]
        [TestCase("http://example.net/path/to/folder", false, "http", "example.net", "/path/to/folder", 80)]
        [TestCase("https://example.org", false, "https", "example.org", "/", 443)]
        [TestCase("ftp://example.jp", false, "ftp", "example.jp", "/", 21)]
        [TestCase("telnet://example.com", false, "telnet", "example.com", "/", 23)]
        [TestCase("NotExist://example.com:1234", false, "notexist", "example.com", "/", 1234)]
        //[TestCase("invalid uri", true, null, null, null, 0)]
        //[TestCase("", true, null, null, null, 0)]
        [TestCase(null, true, null, null, null, 0)]
        public void TryParse_Uri_Test(string input, bool isNull, string scheme, string host, string path, int port) {
            var actual = input.TryParse<Uri>();
            if (!isNull) {
                Assert.IsNotNull(actual);
                Assert.AreEqual(scheme, actual.Scheme, "Scheme");
                Assert.AreEqual(host, actual.Host, "Host");
                Assert.AreEqual(path, actual.AbsolutePath, "AbsolutePath");
                Assert.AreEqual(port, actual.Port, "Port");
            } else {
                Assert.IsNull(actual);
            }
        }

        [TestCase("abc", "", "abc")]
        [TestCase("", "", "")]
        [TestCase(null, "", "")]
        [TestCase("abc", null, "abc")]
        [TestCase("", null, null)]
        [TestCase(null, null, null)]
        [TestCase("abc", DefaultStringValue, "abc")]
        [TestCase("", DefaultStringValue, DefaultStringValue)]
        [TestCase(null, DefaultStringValue, DefaultStringValue)]
        public void ToDefaultIfNullOrEmpty_Test(string input, string defaultValue, string expected) {
            Assert.AreEqual(expected, input.ToDefaultIfNullOrEmpty(defaultValue));
        }

        [TestCase(null, null, StringSplitOptions.RemoveEmptyEntries, null)]
        [TestCase("", null, StringSplitOptions.RemoveEmptyEntries, null)]
        [TestCase("a, b, c, d", null, StringSplitOptions.RemoveEmptyEntries, new[] { "a", "b", "c", "d" })]
        [TestCase("a, b, c, d,", null, StringSplitOptions.RemoveEmptyEntries, new[] { "a", "b", "c", "d" })]
        [TestCase("a, b, c, d, ", null, StringSplitOptions.RemoveEmptyEntries, new[] { "a", "b", "c", "d" })]
        [TestCase("a,\nb,\nc,\nd,\n", null, StringSplitOptions.RemoveEmptyEntries, new[] { "a", "b", "c", "d" })]
        [TestCase("a, b; c, d", new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries, new[] { "a", "b", "c", "d" })]
        [TestCase("a, b; c, d;", new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries, new[] { "a", "b", "c", "d" })]
        [TestCase("a, b; c, d; ", new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries, new[] { "a", "b", "c", "d" })]
        [TestCase("a,\nb;\nc,\nd;\n", new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries, new[] { "a", "b", "c", "d" })]
        [TestCase("a, b, c, d", null, StringSplitOptions.None, new[] { "a", "b", "c", "d" })]
        [TestCase("a, b, c, d,", null, StringSplitOptions.None, new[] { "a", "b", "c", "d", "" })]
        [TestCase("a, b, c, d, ", null, StringSplitOptions.None, new[] { "a", "b", "c", "d", "" })]
        [TestCase("a,\nb,\nc,\nd,\n", null, StringSplitOptions.None, new[] { "a", "b", "c", "d", "" })]
        [TestCase("a, b; c, d", new[] { ";", "," }, StringSplitOptions.None, new[] { "a", "b", "c", "d" })]
        [TestCase("a, b; c, d;", new[] { ";", "," }, StringSplitOptions.None, new[] { "a", "b", "c", "d", "" })]
        [TestCase("a, b; c, d; ", new[] { ";", "," }, StringSplitOptions.None, new[] { "a", "b", "c", "d", "" })]
        [TestCase("a,\nb;\nc,\nd;\n", new[] { ";", "," }, StringSplitOptions.None, new[] { "a", "b", "c", "d", "" })]
        public void SplitTrim_Test(string input, string[] separator, StringSplitOptions options, string[] expected) {
            var actualList = input.SplitTrim(separator, options);
            if (expected == null) {
                Assert.IsNull(actualList);
            } else {
                CollectionAssert.AreEqual(
                    expected, actualList,
                    "expected:'" + String.Join(", ", expected) + "', Actual:'" + String.Join(", ", expected) + "'"
                );
            }
        }

        [TestCase(null, 0, null)]
        [TestCase(null, 1, null)]
        [TestCase(null, 2, null)]
        [TestCase(null, 3, null)]
        [TestCase(null, -1, null)]
        [TestCase("", 0, "")]
        [TestCase("", 1, "")]
        [TestCase("", 2, "")]
        [TestCase("", 3, "")]
        [TestCase("", -1, null)]
        [TestCase("a", 0, "")]
        [TestCase("a", 1, "a")]
        [TestCase("a", 2, "a")]
        [TestCase("a", 3, "a")]
        [TestCase("a", -1, null)]
        [TestCase("ab", 0, "")]
        [TestCase("ab", 1, "b")]
        [TestCase("ab", 2, "ab")]
        [TestCase("ab", 3, "ab")]
        [TestCase("ab", -1, null)]
        [TestCase("abc", 0, "")]
        [TestCase("abc", 1, "c")]
        [TestCase("abc", 2, "bc")]
        [TestCase("abc", 3, "abc")]
        [TestCase("abc", -1, null)]
        [TestCase("abcd", 0, "")]
        [TestCase("abcd", 1, "d")]
        [TestCase("abcd", 2, "cd")]
        [TestCase("abcd", 3, "bcd")]
        [TestCase("abcd", -1, null)]
        [TestCase("あ", 0, "")]
        [TestCase("あ", 1, "あ")]
        [TestCase("あ", 2, "あ")]
        [TestCase("あ", 3, "あ")]
        [TestCase("あ", -1, null)]
        [TestCase("あい", 0, "")]
        [TestCase("あい", 1, "い")]
        [TestCase("あい", 2, "あい")]
        [TestCase("あい", 3, "あい")]
        [TestCase("あい", -1, null)]
        [TestCase("あいう", 0, "")]
        [TestCase("あいう", 1, "う")]
        [TestCase("あいう", 2, "いう")]
        [TestCase("あいう", 3, "あいう")]
        [TestCase("あいう", -1, null)]
        [TestCase("あいうえ", 0, "")]
        [TestCase("あいうえ", 1, "え")]
        [TestCase("あいうえ", 2, "うえ")]
        [TestCase("あいうえ", 3, "いうえ")]
        [TestCase("あいうえ", -1, null)]
        public void Right_Test(string input, int length, string expected) {
            var actual = input.Right(length);
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsNull(actual);
            }
        }

        public struct QuotemetaTestCase {

            public QuotemetaTestCase(string input, string expected)
                : this() {
                Input = input;
                Expected = expected;
            }

            public string Input { get; set; }
            public string Expected { get; set; }
        };

        public static readonly QuotemetaTestCase[] _quotemetaTestCasesRaw = new[] {
            new QuotemetaTestCase(null, null),
            new QuotemetaTestCase("", ""),
            new QuotemetaTestCase("0123456789", "0123456789"),
            new QuotemetaTestCase("ABCabcXYZxyz", "ABCabcXYZxyz"),
            new QuotemetaTestCase("_", "_"),
            new QuotemetaTestCase(" ", "\\u0020"),
            new QuotemetaTestCase("\t", "\\u0009"),
            new QuotemetaTestCase("\n", "\\u000a"),
            new QuotemetaTestCase("\r", "\\u000d"),
            new QuotemetaTestCase("\r\n", "\\u000d\\u000a"),
            new QuotemetaTestCase("012\t345\n678\r9", "012\\u0009345\\u000a678\\u000d9"),
            new QuotemetaTestCase("あいう", "\\u3042\\u3044\\u3046"),
            new QuotemetaTestCase("高髙崎﨑", "\\u9ad8\\u9ad9\\u5d0e\\ufa11"),
            new QuotemetaTestCase("剥\u525D填\u5861頬\u9830", "\\u5265\\u525d\\u586b\\u5861\\u982c\\u9830"),
            new QuotemetaTestCase("\uD842\uDF9F", "\\ud842\\udf9f"), // U+20B9F 𠮟
            new QuotemetaTestCase("\uD842\uDFB7", "\\ud842\\udfb7"), // U+20BB7 𠮷
        };

        public static IEnumerable<object[]> _quotemetaTestCases = _quotemetaTestCasesRaw.ToTestCases();
        public IEnumerable<string> inputs = _quotemetaTestCasesRaw.Select(testcase => testcase.Input);
        public IEnumerable<string> expecteds = _quotemetaTestCasesRaw.Select(testcase => testcase.Expected);

        [TestCaseSource("_quotemetaTestCases")]
        public void Quotemeta_string_Test(string input, string expected) {
            Assert.AreEqual(expected, input.Quotemeta());
        }

        [TestCase]
        public void Quotemeta_stringArray_Test() {
            CollectionAssert.AreEqual(expecteds, inputs.Quotemeta());
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("123 \t 123 1adc \n 222", "1231231adc222")]
        [TestCase(" test test    test", "testtesttest")]
        public void RemoveWhitespace_Test(string input, string expected) {
            var actual = input.RemoveWhitespace();
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.Null(actual);
            }
        }

        [TestCase]
        public void ToBytesAsAscii85Encoding_Test() {
            var input =
                @"9jqo^BlbD-BleB1DJ+*+F(f,q/0JhKF<GL>Cj@.4Gp$d7F!,L7@<6@)/0JDEF<G%<+EV:2F!," +
                @"O<DJ+*.@<*K0@<6L(Df-\0Ec5e;DffZ(EZee.Bl.9pF""AGXBPCsi+DGm>@3BB/F*&OCAfu2/AKY" +
                @"i(DIb:@FD,*)+C]U=@3BN#EcYf8ATD3s@q?d$AftVqCh[NqF<G:8+EV:.+Cf>-FD5W8ARlolDIa" +
                @"l(DId<j@<?3r@:F%a+D58'ATD4$Bl@l3De:,-DJs`8ARoFb/0JMK@qB4^F!,R<AKZ&-DfTqBG%G" +
                @">uD.RTpAKYo'+CT/5+Cei#DII?(E,9)oF*2M7/c";
            var expected =
                "Man is distinguished, not only by his reason, but by this singular passion " +
                "from other animals, which is a lust of the mind, that by a perseverance of delight " +
                "in the continued and indefatigable generation of knowledge, " +
                "exceeds the short vehemence of any carnal pleasure.";
            var actual = Encoding.UTF8.GetString(input.ToBytesAsAscii85Encoding());
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("　！\uff02＃＄％＆\uff07（）＊＋，－．／：；＜＝＞？＠［＼］＾＿\uff40", " !\"#$%&'()*+,-./:;<=>?@[\\]^_`")]
        [TestCase("０１２３４５６７８９", "0123456789")]
        [TestCase("ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [TestCase("ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ", "abcdefghijklmnopqrstuvwxyz")]
        [TestCase("！！！０００ＡＡＡａａａ　　　", "!!!000AAAaaa   ")]
        [TestCase("\u201c\u201d\u201e\u201f\u2e42\u275d\u275e\u301d\u301e\u301f", "\"\"\"\"\"\"\"\"\"\"")]
        [TestCase("\u2018\u2019\u201a\u201b\u275b\u275c", "''''''")]
        [TestCase("あいう　！０Ａａわをん", "あいう !0Aaわをん")]
        public void Latin1_ZenToHan_Test(string input, string expected) {
            var actual = input.Latin1_ZenToHan();
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.Null(actual);
            }
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase(" !\"#$%&'()*+,-./:;<=>?@[\\]^_`", "　！\uff02＃＄％＆\uff07（）＊＋，－．／：；＜＝＞？＠［＼］＾＿\uff40")]
        [TestCase("0123456789", "０１２３４５６７８９")]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ")]
        [TestCase("abcdefghijklmnopqrstuvwxyz", "ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ")]
        [TestCase("!!!000AAAaaa   ", "！！！０００ＡＡＡａａａ　　　")]
        [TestCase("あいう !0Aaわをん", "あいう　！０Ａａわをん")]
        public void Latin1_HanToZen_Test(string input, string expected) {
            var actual = input.Latin1_HanToZen();
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.Null(actual);
            }
        }
    }
}
