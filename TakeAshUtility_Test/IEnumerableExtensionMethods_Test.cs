using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    class Person {

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Person(int id, string name, DateTime dob) {
            ID = id;
            Name = name;
            DateOfBirth = dob;
        }

        public override string ToString() {
            return String.Join(", ", new[] {
                "ID:" + ID,
                "Name:{" + Name + "}",
                "DateOfBirth:{" + DateOfBirth + "}" 
            });
        }
    }

    [TestFixture]
    class IEnumerableExtensionMethods_Test {

        private List<Person> _persons = new List<Person>() {
            new Person(10000, "John", new DateTime(2000, 10, 1)),
            new Person(10001, "Sam", new DateTime(2001, 2, 1)),
            new Person(10002, "Jane", new DateTime(2000, 1, 1)),
            new Person(10003, "Alice", new DateTime(2010, 4, 1)),
            new Person(10004, "Bob", new DateTime(2020, 5, 1)),
            new Person(10005, "Clarisse", new DateTime(2005, 6, 1)),
        };

        [TestCase("ID", 0)]
        [TestCase("Name", 3)]
        [TestCase("DateOfBirth", 2)]
        public void MinBy_Test(string key, int expected) {
            switch (key) {
                case "ID":
                    Assert.AreEqual(_persons[expected], _persons.MinBy(x => x.ID));
                    break;
                case "Name":
                    Assert.AreEqual(_persons[expected], _persons.MinBy(x => x.Name));
                    break;
                case "DateOfBirth":
                    Assert.AreEqual(_persons[expected], _persons.MinBy(x => x.DateOfBirth));
                    break;
            }
        }

        [TestCase("ID", 5)]
        [TestCase("Name", 1)]
        [TestCase("DateOfBirth", 4)]
        public void MaxBy_Test(string key, int expected) {
            switch (key) {
                case "ID":
                    Assert.AreEqual(_persons[expected], _persons.MaxBy(x => x.ID));
                    break;
                case "Name":
                    Assert.AreEqual(_persons[expected], _persons.MaxBy(x => x.Name));
                    break;
                case "DateOfBirth":
                    Assert.AreEqual(_persons[expected], _persons.MaxBy(x => x.DateOfBirth));
                    break;
            }
        }
    }
}
