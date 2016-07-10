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
        [TestCase("ja-JP", "JobName", "[R_ja]ジョブ名")]
        [TestCase("ja-JP", "CustomerName", "[R_en]Customer Name")]
        [TestCase("ja-JP", "CreateDate", "[R_ja]作成日")]
        [TestCase("ja-JP", "UpdateDate", "UpdateDate")]
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

        [TestCase(null, null)]
        [TestCase(typeof(int), default(int))] // primitive
        [TestCase(typeof(double), default(double))]
        [TestCase(typeof(bool), default(bool))]
        [TestCase(typeof(DayOfWeek), default(DayOfWeek))] // enum
        [TestCase(typeof(DateTime), null)] // struct
        [TestCase(typeof(TimeSpan), null)]
        [TestCase(typeof(string), null)] // class
        [TestCase(typeof(int[]), null)]
        [TestCase(typeof(List<int>), null)]
        [TestCase(typeof(Nullable<DateTime>), null)]
        [TestCase(typeof(Nullable<TimeSpan>), null)]
        public void GetDefaultValue_Test(Type type, object expected) {
            if (type == null) {
                Assert.Null(type.GetDefaultValue());
                return;
            }
            switch (type.Name) {
                case "DateTime":
                    expected = default(DateTime);
                    break;
                case "TimeSpan":
                    expected = default(TimeSpan);
                    break;
            }
            Assert.AreEqual(expected, type.GetDefaultValue());
        }

        [TestCase(typeof(BkBRMGCYW), "ToShortName", true)]
        [TestCase(typeof(BkBRMGCYW), "ToGroup", true)]
        [TestCase(typeof(BkBRMGCYW), "NotExist", false)]
        [TestCase(typeof(Enum), "ToLocalizationEx", true)]
        public void GetExtensionMethod_Test(Type type, string name, bool exist) {
            var actual = type.GetExtensionMethod(name);
            if (exist) {
                Assert.NotNull(actual);
                Assert.AreEqual(name, actual.Name);
            } else {
                Assert.Null(actual);
            }
        }
    }
}
