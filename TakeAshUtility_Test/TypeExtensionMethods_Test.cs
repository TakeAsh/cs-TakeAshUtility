using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class TypeExtensionMethods_Test {

        public class Sample1 {

            [System.ComponentModel.Description("Job Name")]
            public string JobName { get; set; }

            [System.ComponentModel.Description("Create Date")]
            public DateTime CreateDate { get; set; }
        }

        [TestCase("JobName", "Job Name")]
        [TestCase("CreateDate", "Create Date")]
        [TestCase("NotExist", null)]
        public void GetAttribute_Test(string propertyName, string expected) {
            var attr = typeof(Sample1).GetAttribute<System.ComponentModel.DescriptionAttribute>(propertyName);
            if (expected != null) {
                Assert.IsNotNull(attr);
                Assert.AreEqual(expected, attr.Description);
            } else {
                Assert.IsNull(attr);
            }
        }
    }
}
