using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TypeConverter(typeof(StringifyConverter<Sample1>))]
    public class Sample1 :
        IItems<Sample1.Items>,
        IStringify<Sample1>,
        IEquatable<Sample1> {

        public enum Items {
            ID,
            Name,
            Sex,
            DOB,
            Comment,
        }

        public enum SexesCodes {
            NotKnown = 0,
            Male = 1,
            Female = 2,
            NotApplicable = 9,
        }

        private static XmlSerializer _serializer = new XmlSerializer(typeof(Sample1));
        private static XmlSerializerNamespaces _blankNameSpace = new XmlSerializerNamespaces();
        static private XmlWriterSettings _settings = new XmlWriterSettings() {
            Indent = false,
            Encoding = Encoding.UTF8,
        };
        private static readonly Regex _regXmlHeader = new Regex(
            @"^(?<head1><\?xml\s+version=""[^""]+""\s+encoding="")[^""]+(?<head2>""\s*\?>)"
        );

        static Sample1() {
            _blankNameSpace.Add("", "");
        }

        [XmlAttribute]
        public int ID { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public SexesCodes Sex { get; set; }

        [XmlIgnore]
        public DateTime DOB { get; set; }

        [XmlAttribute("DOB")]
        public string DOBAttr {
            get { return DOB.SafeToString<string>("s"); }
            set { DOB = value.TryParse<DateTime>(); }
        }

        public string Comment { get; set; }

        #region IItems<Sample1.Items> members

        public string this[Items item] {
            get {
                switch (item) {
                    case Items.ID:
                        return ID.ToString();
                    case Items.Name:
                        return Name;
                    case Items.Sex:
                        return Sex.ToString();
                    case Items.DOB:
                        return DOBAttr;
                    case Items.Comment:
                        return Comment;
                    default:
                        throw new NotImplementedException("Not Implemented: " + item);
                }
            }
            set {
                switch (item) {
                    case Items.ID:
                        ID = value.TryParse<int>();
                        break;
                    case Items.Name:
                        Name = value;
                        break;
                    case Items.Sex:
                        Sex = value.TryParse<SexesCodes>();
                        break;
                    case Items.DOB:
                        DOBAttr = value;
                        break;
                    case Items.Comment:
                        Comment = value;
                        break;
                    default:
                        throw new NotImplementedException("Not Implemented: " + item);
                }
            }
        }

        #endregion

        #region IStringify<Sample1> members

        public override string ToString() { return this.ToString<Sample1, Items>(); }

        public Sample1 FromString(string source) { return source.FromString<Sample1, Items>(); }

        #endregion

        #region IEquatable<Sample1> members

        public bool Equals(Sample1 other) { return this.Equals<Sample1, Items>(other); }

        public override int GetHashCode() { return this.GetHashCode<Sample1, Items>(); }

        #endregion

        public string ToXml() {
            try {
                var sb = new StringBuilder();
                using (var writer = XmlWriter.Create(sb, _settings)) {
                    _serializer.Serialize(writer, this, _blankNameSpace);
                    return _regXmlHeader.Replace(
                        sb.ToString(),
                        (Match m) => m.Groups["head1"].Value + "utf-8" + m.Groups["head2"].Value
                    );
                }
            }
            catch (Exception ex) {
                Debug.Print(ex.Message);
            }
            return null;
        }

        public static Sample1 FromXml(string text) {
            try {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text ?? ""))) {
                    return _serializer.Deserialize(stream) as Sample1;
                }
            }
            catch (Exception ex) {
                Debug.Print(ex.Message);
            }
            return default(Sample1);
        }
    }

    [TestFixture]
    class IItems_Test {

        const string XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        [TestCase(null, null)]
        [TestCase("<Sample1 />",
            "ID\t0\nSex\tNotKnown\nDOB\t0001-01-01T00:00:00")]
        [TestCase("<Sample1 ID=\"1\" Name=\"name\" Sex=\"NotKnown\" DOB=\"2015-08-01T12:34:56\"><Comment>Commnet\tCommnet\nCommnet</Comment></Sample1>",
            "ID\t1\nName\tname\nSex\tNotKnown\nDOB\t2015-08-01T12:34:56\nComment\tCommnet\\tCommnet\\nCommnet")]
        public void ToString_Test(string xml, string expected) {
            if (xml == null) {
                Assert.IsNull((null as Sample1).ToString<Sample1, Sample1.Items>());
            } else {
                var input = Sample1.FromXml(XmlHeader + xml);
                Assert.AreEqual(expected, input.ToString());
            }
        }

        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase("not valid", null)]
        [TestCase("ID\t0",
            "<Sample1 ID=\"0\" Sex=\"NotKnown\" DOB=\"0001-01-01T00:00:00\" />")]
        [TestCase("ID\t-1\nName\tJohn Doe\nSex\tFemale\nDOB\t2015-08-01T12:34:56\nComment\tCommnet\\tCommnet\\nCommnet",
            "<Sample1 ID=\"-1\" Name=\"John Doe\" Sex=\"Female\" DOB=\"2015-08-01T12:34:56\"><Comment>Commnet\tCommnet\r\nCommnet</Comment></Sample1>")]
        public void FromString_Test(string input, string expected) {
            var actual = new Sample1().FromString(input);
            if (String.IsNullOrEmpty(expected)) {
                Assert.IsNull(actual);
            } else {
                Assert.AreEqual(XmlHeader + expected, actual.ToXml());
            }
        }

        [TestCase(null, null, true)]
        [TestCase("ID\t0", null, false)]
        [TestCase(null, "ID\t0", false)]
        [TestCase("ID\t0", "ID\t0", true)]
        [TestCase("ID\t0", "ID\t1", false)]
        [TestCase("Name\tJohn Doe", "Name\tJohn Doe", true)]
        [TestCase("Name\tJohn Doe", "Name\tJane Doe", false)]
        [TestCase("Sex\tMale", "Sex\tMale", true)]
        [TestCase("Sex\tMale", "Sex\tNotApplicable", false)]
        [TestCase("DOB\t2015-08-01T12:34:56", "DOB\t2015-08-01T12:34:56", true)]
        [TestCase("DOB\t2015-08-01T12:34:56", "DOB\t2001-02-03T23:45:01", false)]
        [TestCase("Comment\ta\\tb\\nc", "Comment\ta\\tb\\nc", true)]
        [TestCase("Comment\ta\\tb\\nc", "Comment\td\\te\\nf", false)]
        public void Equals_Test(string input1, string input2, bool expected) {
            var obj1 = new Sample1().FromString(input1);
            var obj2 = new Sample1().FromString(input2);
            Assert.AreEqual(
                expected,
                obj1.Equals<Sample1, Sample1.Items>(obj2),
                "obj1:{" + obj1 + "}, obj2:{" + obj2 + "}"
            );
        }

        [TestCase("ID\t0", "ID\t0", true)]
        [TestCase("ID\t0", "ID\t1", false)]
        [TestCase("ID\t1", "ID\t1", true)]
        [TestCase("Name\tabc", "Name\tabc", true)]
        [TestCase("Name\tabc", "Name\tdef", false)]
        [TestCase("Name\tdef", "Name\tdef", true)]
        [TestCase("Comment\tabc", "Comment\tabc", true)]
        [TestCase("Comment\tabc", "Comment\tdef", false)]
        [TestCase("Comment\tdef", "Comment\tdef", true)]
        [TestCase("Name\tabc", "Comment\tabc", false)]
        [TestCase("Name\tabc", "Comment\tdef", false)]
        [TestCase("Name\tdef", "Comment\tdef", false)]
        public void GetHashCode_Test(string input1, string input2, bool expectedEqual) {
            var obj1 = new Sample1().FromString(input1);
            var obj2 = new Sample1().FromString(input2);
            if (expectedEqual) {
                Assert.AreEqual(
                    obj1.GetHashCode(),
                    obj2.GetHashCode(),
                    "obj1:{" + obj1 + "}, obj2:{" + obj2 + "}"
                );
            } else {
                Assert.AreNotEqual(
                    obj1.GetHashCode(),
                    obj2.GetHashCode(),
                    "obj1:{" + obj1 + "}, obj2:{" + obj2 + "}"
                );
            }
        }
    }
}
