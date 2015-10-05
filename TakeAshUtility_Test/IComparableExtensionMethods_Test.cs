using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class IComparableExtensionMethods_Test {

        [TestCase(0, 0, 10, 0)]
        [TestCase(-1, 0, 10, 0)]
        [TestCase(-2, 0, 10, 0)]
        [TestCase(-10, 0, 10, 0)]
        [TestCase(1, 0, 10, 1)]
        [TestCase(5, 0, 10, 5)]
        [TestCase(10, 0, 10, 10)]
        [TestCase(12, 0, 10, 10)]
        [TestCase(15, 0, 10, 10)]
        [TestCase(20, 0, 10, 10)]
        [TestCase(0, -2, 15, 0)]
        [TestCase(-1, -2, 15, -1)]
        [TestCase(-2, -2, 15, -2)]
        [TestCase(-10, -2, 15, -2)]
        [TestCase(1, -2, 15, 1)]
        [TestCase(5, -2, 15, 5)]
        [TestCase(10, -2, 15, 10)]
        [TestCase(12, -2, 15, 12)]
        [TestCase(15, -2, 15, 15)]
        [TestCase(20, -2, 15, 15)]
        public void Clamp_int_Test(int val, int min, int max, int expected) {
            Assert.AreEqual(expected, val.Clamp(min, max));
        }

        [TestCase(0, 0, 10, 0)]
        [TestCase(-1, 0, 10, 0)]
        [TestCase(-2, 0, 10, 0)]
        [TestCase(-10, 0, 10, 0)]
        [TestCase(1, 0, 10, 1)]
        [TestCase(5, 0, 10, 5)]
        [TestCase(10, 0, 10, 10)]
        [TestCase(12, 0, 10, 10)]
        [TestCase(15, 0, 10, 10)]
        [TestCase(20, 0, 10, 10)]
        [TestCase(0, -2, 15, 0)]
        [TestCase(-1, -2, 15, -1)]
        [TestCase(-2, -2, 15, -2)]
        [TestCase(-10, -2, 15, -2)]
        [TestCase(1, -2, 15, 1)]
        [TestCase(5, -2, 15, 5)]
        [TestCase(10, -2, 15, 10)]
        [TestCase(12, -2, 15, 12)]
        [TestCase(15, -2, 15, 15)]
        [TestCase(20, -2, 15, 15)]
        public void Clamp_double_Test(double val, double min, double max, double expected) {
            Assert.AreEqual(expected, val.Clamp(min, max));
        }
    }
}
