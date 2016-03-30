using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class CRC32_Test {

        /// <summary>
        /// ComputeHash Test
        /// </summary>
        /// <param name="text">source string</param>
        /// <param name="expected">expected value</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>[Hash Collision Probabilities](http://preshing.com/20110504/hash-collision-probabilities/)</item>
        /// <item>[Trash CRC32 | Fortinet Blog](https://blog.fortinet.com/post/trash-crc32)</item>
        /// </list>
        /// </remarks>
        [TestCase("spaceship", "AA-70-8C-8E")]
        [TestCase("banana", "03-8B-67-CF")]
        [TestCase("plumless", "4D-DB-0C-25")]
        [TestCase("buckeroo", "4D-DB-0C-25")]
        [TestCase("a1sellers", "D6-E4-66-3B")]
        [TestCase("advertees", "D6-E4-66-3B")]
        public void ComputeHash_Test(string text, string expected) {
            var crc32 = new CRC32();
            var actual = BitConverter.ToString(crc32.ComputeHash(Encoding.UTF8.GetBytes(text)));
            Assert.AreEqual(expected, actual);
        }
    }
}
