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
            6, 125,
        };
        byte[] pattern = new byte[] {
            12, 3, 5, 76, 8, 0, 6, 125,
        };

        [TestCase(0, "8 26")]
        [TestCase(10, "26")]
        public void IndexesOf_Test(int startIndex, string expected) {
            var expected2 = expected.Split()
                .Select(item => item.TryParse<int>())
                .ToList();
            var actual = data.IndexesOf(pattern, startIndex);
            Assert.AreEqual(expected2, actual);
        }
    }
}
