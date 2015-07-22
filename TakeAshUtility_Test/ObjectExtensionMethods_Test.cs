using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class ObjectExtensionMethods_Test {

        public enum Weekdays {
            Sun,
            Mon,
            Tue,
            Wed,
            Thu,
            Fri,
            Sat,
        }

        [TestCase(null, null, null)]
        [TestCase(null, "", "")]
        [TestCase(null, "undefined", "undefined")]
        [TestCase(1, null, "1")]
        [TestCase(1, "", "1")]
        [TestCase(1, "undefined", "1")]
        [TestCase(2.2, null, "2.2")]
        [TestCase(2.2, "", "2.2")]
        [TestCase(2.2, "undefined", "2.2")]
        [TestCase(Weekdays.Mon, null, "Mon")]
        [TestCase(Weekdays.Mon, "", "Mon")]
        [TestCase(Weekdays.Mon, "Null", "Mon")]
        public void SafeToString_Test(object obj, string ifNull, string expected) {
            var actual = obj.SafeToString(ifNull);
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsNull(actual);
            }
        }

        [TestCase(null, "#,##0.000", null, null)]
        [TestCase(null, "#,##0.000", "", "")]
        [TestCase(null, "#,##0.000", "undefined", "undefined")]
        [TestCase(2345.67, "#,##0.000", null, "2,345.670")]
        [TestCase(2345.67, "#,##0.000", "", "2,345.670")]
        [TestCase(2345.67, "#,##0.000", "undefined", "2,345.670")]
        public void SafeToString_Opt1_String_Test(object obj, string opt1, string ifNull, string expected) {
            var actual = obj.SafeToString(opt1, ifNull);
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsNull(actual);
            }
        }

        [TestCase(null, "en-US", null, null)]
        [TestCase(null, "en-US", "", "")]
        [TestCase(null, "en-US", "undefined", "undefined")]
        [TestCase(null, "ja-JP", null, null)]
        [TestCase(null, "ja-JP", "", "")]
        [TestCase(null, "ja-JP", "undefined", "undefined")]
        [TestCase("2015/04/01", "en-US", null, "4/1/2015 12:00:00 AM")]
        [TestCase("2015/04/01", "en-US", "", "4/1/2015 12:00:00 AM")]
        [TestCase("2015/04/01", "en-US", "undefined", "4/1/2015 12:00:00 AM")]
        [TestCase("2015/04/01", "ja-JP", null, "2015/04/01 00:00:00")]
        [TestCase("2015/04/01", "ja-JP", "", "2015/04/01 00:00:00")]
        [TestCase("2015/04/01", "ja-JP", "undefined", "2015/04/01 00:00:00")]
        public void SafeToString_Opt1_CultureInfo_Test(string date, string opt1, string ifNull, string expected) {
            DateTime? obj = null;
            if (date != null) {
                obj = DateTime.Parse(date);
            }
            var ci = new CultureInfo(opt1);
            var actual = obj.SafeToString(ci, ifNull);
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsNull(actual);
            }
        }

        [TestCase(null, "yyyy-MM-dd(ddd)", "en-US", null, null)]
        [TestCase(null, "yyyy-MM-dd(ddd)", "en-US", "", "")]
        [TestCase(null, "yyyy-MM-dd(ddd)", "en-US", "undefined", "undefined")]
        [TestCase(null, "yyyy-MM-dd(ddd)", "ja-JP", null, null)]
        [TestCase(null, "yyyy-MM-dd(ddd)", "ja-JP", "", "")]
        [TestCase(null, "yyyy-MM-dd(ddd)", "ja-JP", "undefined", "undefined")]
        [TestCase("2015/04/01", "yyyy-MM-dd(ddd)", "en-US", null, "2015-04-01(Wed)")]
        [TestCase("2015/04/01", "yyyy-MM-dd(ddd)", "en-US", "", "2015-04-01(Wed)")]
        [TestCase("2015/04/01", "yyyy-MM-dd(ddd)", "en-US", "undefined", "2015-04-01(Wed)")]
        [TestCase("2015/04/01", "yyyy-MM-dd(ddd)", "ja-JP", null, "2015-04-01(水)")]
        [TestCase("2015/04/01", "yyyy-MM-dd(ddd)", "ja-JP", "", "2015-04-01(水)")]
        [TestCase("2015/04/01", "yyyy-MM-dd(ddd)", "ja-JP", "undefined", "2015-04-01(水)")]
        public void SafeToString_Opt2_String_CultureInfo_Test(string date, string opt1, string opt2, string ifNull, string expected) {
            DateTime? obj = null;
            if (date != null) {
                obj = DateTime.Parse(date);
            }
            var ci = new CultureInfo(opt2);
            var actual = obj.SafeToString(opt1, ci, ifNull);
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsNull(actual);
            }
        }
    }
}
