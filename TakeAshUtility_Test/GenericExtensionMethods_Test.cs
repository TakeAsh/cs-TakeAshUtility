using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class GenericExtensionMethods_Test {

        [TestCase(-2, -1, 10, false)]
        [TestCase(-1, -1, 10, true)]
        [TestCase(0, -1, 10, true)]
        [TestCase(1, -1, 10, true)]
        [TestCase(9, -1, 10, true)]
        [TestCase(10, -1, 10, true)]
        [TestCase(11, -1, 10, false)]
        public void Between_int_Test(int x, int min, int max, bool expected) {
            var actual = x.Between(min, max);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(-2, -1, 10, false)]
        [TestCase(-1, -1, 10, true)]
        [TestCase(0, -1, 10, true)]
        [TestCase(1, -1, 10, true)]
        [TestCase(9, -1, 10, true)]
        [TestCase(10, -1, 10, true)]
        [TestCase(11, -1, 10, false)]
        public void Between_double_Test(double x, double min, double max, bool expected) {
            var actual = x.Between(min, max);
            Assert.AreEqual(expected, actual);
        }

        class Person :
            IStringify<Person>,
            IComparable<Person> {

            [ToStringMember]
            public int ID { get; set; }

            [ToStringMember]
            public string Name { get; set; }

            #region IStringify

            public override string ToString() {
                return this.ToStringMembers();
            }

            public Person FromString(string source) {
                if (String.IsNullOrEmpty(source)) {
                    return null;
                }
                var pairs = source.SplitTrim();
                if (pairs.SafeCount() == 0) {
                    return null;
                }
                pairs.ForEach(pair => {
                    var separatorIndex = pair.IndexOf(':');
                    if (separatorIndex < 0) {
                        return;
                    }
                    var key = pair.Substring(0, separatorIndex);
                    var value = pair.Substring(separatorIndex + 1);
                    switch (key) {
                        case "ID":
                            ID = value.TryParse<int>();
                            break;
                        case "Name":
                            Name = value;
                            break;
                    }
                });
                return this;
            }

            #endregion

            #region IComparable

            public int CompareTo(Person other) {
                if (other == null) {
                    return -1;
                }
                if (ReferenceEquals(this, other)) {
                    return 0;
                }
                return this.ID.CompareTo(other.ID);
            }

            #endregion
        }

        [TestCase(null, "ID:1, Name:{John Doe}", "ID:10, Name:{Jane Doe}", false)]
        [TestCase("ID:0, Name:{Sam Doe}", "ID:1, Name:{John Doe}", "ID:10, Name:{Jane Doe}", false)]
        [TestCase("ID:2, Name:{Sam Doe}", "ID:1, Name:{John Doe}", "ID:10, Name:{Jane Doe}", true)]
        public void Between_Person_Test(string x, string min, string max, bool expected) {
            var x2 = x.To<Person>();
            var min2 = min.To<Person>();
            var max2 = max.To<Person>();
            var actual = x2.Between(min2, max2);
            Assert.AreEqual(expected, actual, "x:{" + x2 + "}, min:{" + min + "}, min:{" + max + "}");
        }
    }
}
