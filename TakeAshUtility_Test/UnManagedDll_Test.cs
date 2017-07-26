using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class UnManagedDll_Test {

        [TestCase(null, false)]
        [TestCase("kernel32", true)]
        [TestCase("user32", true)]
        public void UnManagedDll_Construct(string dllName, bool expected) {
            var dll = new UnManagedDll(dllName);
            Assert.AreEqual(expected, dll.Loaded);
        }
    }
}