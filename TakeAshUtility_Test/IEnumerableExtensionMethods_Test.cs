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

        IComparer<Person> _idComparer = new InlineComparer<Person>((x, y) => x.ID.CompareTo(y.ID));
        IComparer<Person> _nameComparer = new InlineComparer<Person>((x, y) => x.Name.CompareTo(y.Name));
        IComparer<Person> _dateOfBirthComparer = new InlineComparer<Person>((x, y) => x.DateOfBirth.CompareTo(y.DateOfBirth));

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

        [TestCase("ID", 0)]
        [TestCase("Name", 3)]
        [TestCase("DateOfBirth", 2)]
        public void IndexOfMin_Test(string key, int expected) {
            switch (key) {
                case "ID":
                    Assert.AreEqual(expected, _persons.IndexOfMin(_idComparer));
                    break;
                case "Name":
                    Assert.AreEqual(expected, _persons.IndexOfMin(_nameComparer));
                    break;
                case "DateOfBirth":
                    Assert.AreEqual(expected, _persons.IndexOfMin(_dateOfBirthComparer));
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

        [TestCase("ID", 5)]
        [TestCase("Name", 1)]
        [TestCase("DateOfBirth", 4)]
        public void IndexOfMax_Test(string key, int expected) {
            switch (key) {
                case "ID":
                    Assert.AreEqual(expected, _persons.IndexOfMax(_idComparer));
                    break;
                case "Name":
                    Assert.AreEqual(expected, _persons.IndexOfMax(_nameComparer));
                    break;
                case "DateOfBirth":
                    Assert.AreEqual(expected, _persons.IndexOfMax(_dateOfBirthComparer));
                    break;
            }
        }

        [TestCase("Double", "1;2;3;4;5", "2;4;6;8;10")]
        [TestCase("Triple", "1;2;3;4;5", "3;6;9;12;15")]
        [TestCase("Single", "1;2;3;4;5", "1;2;3;4;5")]
        public void ForEach_Test(string func, string input, string expected) {
            Func<double, double> converter;
            switch (func) {
                case "Double":
                    converter = (x) => x * 2;
                    break;
                case "Triple":
                    converter = (x) => x * 3;
                    break;
                default:
                    converter = (x) => x;
                    break;
            }
            var work = new List<double>();
            input.Split(new[] { ";" }, StringSplitOptions.None)
                .Select(x => x.TryParse<double>())
                .ForEach(x => work.Add(converter(x)));
            var expectedSequence = expected.Split(new[] { ";" }, StringSplitOptions.None)
                .Select(x => x.TryParse<double>())
                .ToArray();
            CollectionAssert.AreEqual(expectedSequence, work);
        }

        [TestCase("Double", "1;2;3;4;5", "2;5;8;11;14")]
        [TestCase("Triple", "1;2;3;4;5", "3;7;11;15;19")]
        [TestCase("Single", "1;2;3;4;5", "1;3;5;7;9")]
        public void ForEach_with_Index_Test(string func, string input, string expected) {
            Func<double, int, double> converter;
            switch (func) {
                case "Double":
                    converter = (x, index) => x * 2 + index;
                    break;
                case "Triple":
                    converter = (x, index) => x * 3 + index;
                    break;
                default:
                    converter = (x, index) => x + index;
                    break;
            }
            var work = new List<double>();
            input.Split(new[] { ";" }, StringSplitOptions.None)
                .Select(x => x.TryParse<double>())
                .ForEach((x, index) => work.Add(converter(x, index)));
            var expectedSequence = expected.Split(new[] { ";" }, StringSplitOptions.None)
                .Select(x => x.TryParse<double>())
                .ToArray();
            CollectionAssert.AreEqual(expectedSequence, work);
        }
    }
}
