using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class TypeExtensionMethods_Test {

        public class PropertyDescriptionSample {

            public string JobName { get; set; }

            [System.ComponentModel.Description("[A]Customer Name")]
            public string CustomerName { get; set; }

            [System.ComponentModel.Description("[A]Create Date")]
            public DateTime CreateDate { get; set; }

            public DateTime UpdateDate { get; set; }
        }

        private void SetCurrentCulture(string cultureName) {
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestCase("JobName", null)]
        [TestCase("CustomerName", "[A]Customer Name")]
        [TestCase("CreateDate", "[A]Create Date")]
        [TestCase("UpdateDate", null)]
        [TestCase("NotExist", null)]
        public void GetAttribute_Test(string propertyName, string expected) {
            var attr = typeof(PropertyDescriptionSample)
                .GetAttribute<System.ComponentModel.DescriptionAttribute>(propertyName);
            if (expected != null) {
                Assert.IsNotNull(attr);
                Assert.AreEqual(expected, attr.Description);
            } else {
                Assert.IsNull(attr);
            }
        }

        [TestCase("en-US", "JobName", "[R_en]Job Name")]
        [TestCase("en-US", "CustomerName", "[R_en]Customer Name")]
        [TestCase("en-US", "CreateDate", "[A]Create Date")]
        [TestCase("en-US", "UpdateDate", "UpdateDate")]
        [TestCase("en-US", "NotExist", null)]
        [TestCase("ja-JP", "JobName",  "[R_ja]ジョブ名")]
        [TestCase("ja-JP", "CustomerName", "[R_en]Customer Name")]
        [TestCase("ja-JP", "CreateDate",  "[R_ja]作成日")]
        [TestCase("ja-JP", "UpdateDate",  "UpdateDate")]
        [TestCase("ja-JP", "NotExist", null)]
        public void ToDescription_Test(string culture, string propertyName, string expected) {
            SetCurrentCulture(culture);
            var actual = typeof(PropertyDescriptionSample).ToDescription(propertyName);
            if (expected != null) {
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsNull(actual);
            }
        }
    }
}
