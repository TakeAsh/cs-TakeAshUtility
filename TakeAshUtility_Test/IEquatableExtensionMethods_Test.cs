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
        public void EqualsComplexWithObject_Test(string type, string value, bool expected) {
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
        public void OpEqualsComplex_Test(double ar, double ai, double br, double bi, bool expected) {
            var a = new Complex(ar, ai);
            var b = new Complex(br, bi);
            Assert.AreEqual(expected, a == b);
        }

        class Person :
            IEquatable<Person>,
            IStringify<Person> {

            public Person() { }

            public Person(string first, string middle, string last)
                : this() {
                FirstName = first;
                MiddleName = middle;
                LastName = last;
            }

            [ToStringMember]
            public string FirstName { get; set; }

            [ToStringMember]
            public string MiddleName { get; set; }

            [ToStringMember]
            public string LastName { get; set; }

            #region IStringify

            public override string ToString() {
                return this.ToStringMembers();
            }

            public Person FromString(string source) {
                if (String.IsNullOrEmpty(source)) {
                    return default(Person);
                }
                var pairs = source.SplitTrim();
                if (pairs == null) {
                    return default(Person);
                }
                foreach (var pair in pairs) {
                    var kv = pair.SplitTrim(new[] { ":" })
                        .SafeToArray();
                    if (kv.SafeCount() < 2) {
                        continue;
                    }
                    switch (kv[0]) {
                        case "FirstName":
                            FirstName = kv[1].Trim(new[] { '{', '}', });
                            break;
                        case "MiddleName":
                            MiddleName = kv[1].Trim(new[] { '{', '}', });
                            break;
                        case "LastName":
                            LastName = kv[1].Trim(new[] { '{', '}', });
                            break;
                    }
                }
                return this;
            }

            #endregion

            #region IEquatable

            public bool Equals(Person other) {
                if (other == null) {
                    return false;
                }
                if (ReferenceEquals(this, other)) {
                    return true;
                }
                return this.FirstName == other.FirstName &&
                    this.MiddleName == other.MiddleName &&
                    this.LastName == other.LastName;
            }

            public override bool Equals(object obj) {
                return this.EqualsEx(obj);
            }

            public override int GetHashCode() {
                return FirstName.GetHashCode() |
                    MiddleName.GetHashCode() * 7 |
                    LastName.GetHashCode() * 13;
            }

            public static bool operator ==(Person a, Person b) {
                return a.EqualsEx(b);
            }

            public static bool operator !=(Person a, Person b) {
                return !a.EqualsEx(b);
            }

            #endregion
        }

        [TestCase("Person", "FirstName:{Andrew}, MiddleName:{Buzz}, LastName:{Carnegie}", true)]
        [TestCase("Person", "FirstName:{Carnegie}, MiddleName:{Andrew}, LastName:{Buzz}", false)]
        [TestCase("int", "7", false)]
        [TestCase("double", "11.13", false)]
        [TestCase("DateTime", "2001/01/01", false)]
        [TestCase(null, null, false)]
        public void EqualsPersonWithObject_Test(string type, string value, bool expected) {
            var standard = new Person("Andrew", "Buzz", "Carnegie");
            object input = null;
            switch (type) {
                case "Person":
                    input = value.To<Person>();
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

        [TestCase(null, null, null, null, null, null, true)]
        [TestCase("Andrew", "Buzz", "Carnegie", "Andrew", "Buzz", "Carnegie", true)]
        [TestCase("Andrew", "Buzz", "Carnegie", "Carnegie", "Andrew", "Buzz", false)]
        public void OpEqualsPerson_Test(
            string af, string am, string al,
            string bf, string bm, string bl,
            bool expected
        ) {
            var a = new Person(af, am, al);
            var b = new Person(bf, bm, bl);
            Assert.AreEqual(expected, a == b);
        }
    }
}
