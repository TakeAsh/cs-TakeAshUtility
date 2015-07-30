using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class EndPointExtensionMethods_Test {

        [TestCase("2001:db8::c0a8:1", 123, "2001:db8::c0a8:1", 123, true, true)]
        [TestCase("2001:db8::c0a8:1", 123, "2001:db8::c0a8:1%0", 123, true, true)]
        [TestCase("2001:db8::c0a8:1", 123, "2001:db8::c0a8:1%1", 123, false, true)]
        [TestCase("2001:db8::c0a8:1", 123, "2001:db8::c0a8:1%16", 123, false, true)]
        [TestCase("2001:db8::c0a8:1%1", 123, "2001:db8::c0a8:1%0", 123, false, true)]
        [TestCase("2001:db8::c0a8:1%1", 123, "2001:db8::c0a8:1%1", 123, true, true)]
        [TestCase("2001:db8::c0a8:1%1", 123, "2001:db8::c0a8:1%16", 123, false, true)]
        [TestCase("2001:db8::c0a8:1", 123, "2001:db8::c0a8:2", 123, false, false)]
        [TestCase("2001:db8::c0a8:1", 123, "2001:db8::c0a8:1", 234, false, false)]
        public void EqualsIgnoreScopeId_Test(
            string sip1, int port1,
            string sip2, int port2,
            bool equalsOriginal,
            bool equalsIgnore
        ) {
            IPAddress ip1;
            IPAddress.TryParse(sip1, out ip1);
            var ipep1 = new IPEndPoint(ip1, port1);
            IPAddress ip2;
            IPAddress.TryParse(sip2, out ip2);
            var ipep2 = new IPEndPoint(ip2, port2);
            Assert.AreEqual(equalsOriginal, ipep1.Equals(ipep2), "Equals()");
            Assert.AreEqual(equalsIgnore, ipep1.EqualsIgnoreScopeId(ipep2), "EqualsIgnoreScopeId()");
        }
    }
}
