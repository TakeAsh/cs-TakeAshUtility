using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class IEquatableExtensionMethods_Test {

        struct Complex :
            IEquatable<Complex>,
            IStringify<Complex> {

            public Complex(double real, double imaginary)
                : this() {

                Real = real;
                Imaginary = imaginary;
            }

            [ToStringMember]
            public double Real;

            [ToStringMember]
            public double Imaginary;

            #region IStringify

            public override string ToString() {
                return this.ToStringMembers();
            }

            public Complex FromString(string source) {
                if (String.IsNullOrEmpty(source)) {
                    return default(Complex);
                }
                var pairs = source.SplitTrim();
                if (pairs == null) {
                    return default(Complex);
                }
                foreach (var pair in pairs) {
                    var kv = pair.SplitTrim(new[] { ":" })
                        .SafeToArray();
                    if (kv.SafeCount() < 2) {
                        continue;
                    }
                    switch (kv[0]) {
                        case "Real":
                            Real = kv[1].TryParse<double>();
                            break;
                        case "Imaginary":
                            Imaginary = kv[1].TryParse<double>();
                            break;
                    }
                }
                return this;
            }
            
            #endregion

            #region IEquatable

            public bool Equals(Complex other) {
                return this.Real == other.Real &&
                    this.Imaginary == other.Imaginary;
            }

            public override bool Equals(object obj) {
                return this.EqualsEx(obj);
            }

            public override int GetHashCode() {
                return Real.GetHashCode() | Imaginary.GetHashCode() * 7;
            }

            public static bool operator ==(Complex a, Complex b) {
                return a.EqualsEx(b);
            }

            public static bool operator !=(Complex a, Complex b) {
                return !a.EqualsEx(b);
            }

            #endregion
        }

        [TestCase("Complex", "Real:3, Imaginary:-5", true)]
        [TestCase("Complex", "Real:-5, Imaginary:-3", false)]
        [TestCase("int", "7", false)]
        [TestCase("double", "11.13", false)]
        [TestCase("DateTime", "2001/01/01", false)]
        [TestCase(null, null, false)]
        public void EqualsWithObject_Test(string type, string value, bool expected) {
            var standard = new Complex(3, -5);
            object input = null;
            switch (type) {
                case "Complex":
                    input = value.To<Complex>();
                    break;
                case "int":
                    input = value.TryParse<int>();
                    break;
                case "double":
                    input = value.TryParse<double>();
                    break;
                case "DateTime":
                    input = value.TryParse<DateTime>();
                    break;
            }
            Assert.AreEqual(expected, standard.Equals(input));
        }

        [TestCase(0, 0, 0, 0, true)]
        [TestCase(3, -5, 3, -5, true)]
        [TestCase(3, -5, -5, 3, false)]
        public void OpEquals_Test(double ar, double ai, double br, double bi, bool expected) {
            var a = new Complex(ar, ai);
            var b = new Complex(br, bi);
            Assert.AreEqual(expected, a == b);
        }
    }
}
