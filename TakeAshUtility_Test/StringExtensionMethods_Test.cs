using System;
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
    }
}
