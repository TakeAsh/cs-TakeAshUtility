using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class BytesExtensionMethods_Test {

        byte[] data = new byte[] {
            23, 36, 43, 76, 125, 56, 34, 234,
            12, 3, 5, 76, 8, 0, 6, 125,
            234, 56, 211, 122, 22, 4, 7, 89,
            76, 64, 12, 3, 5, 76, 8, 0,
            6, 125, 12, 3, 5, 76, 8, 0,
            6, 125, 12, 3, 5, 76, 8, 0,
        };
        byte[] pattern = new byte[] {
            12, 3, 5, 76, 8, 0, 6, 125,
        };

        [TestCase(0, int.MaxValue, "8 26 34")]
        [TestCase(8, int.MaxValue, "8 26 34")]
        [TestCase(9, int.MaxValue, "26 34")]
        [TestCase(26, int.MaxValue, "26 34")]
        [TestCase(27, int.MaxValue, "34")]
        [TestCase(34, int.MaxValue, "34")]
        [TestCase(35, int.MaxValue, null)]
        [TestCase(100, int.MaxValue, null)]
        [TestCase(0, 15, null)]
        [TestCase(0, 16, "8")]
        [TestCase(0, 33, "8")]
        [TestCase(0, 34, "8 26")]
        [TestCase(0, 41, "8 26")]
        [TestCase(0, 42, "8 26 34")]
        [TestCase(0, 100, "8 26 34")]
        [TestCase(8, 41, "8 26")]
        [TestCase(8, 42, "8 26 34")]
        [TestCase(9, 41, "26")]
        [TestCase(9, 42, "26 34")]
        public void IndexesOf_Test(int startIndex, int endIndex, string expected) {
            var actual = data.IndexesOf(pattern, startIndex, endIndex);
            if (expected != null) {
                var expected2 = expected.Split()
                    .Select(item => item.TryParse<int>())
                    .ToList();
                Assert.AreEqual(expected2, actual);
            } else {
                Assert.Null(actual);
            }
        }
    }
}
