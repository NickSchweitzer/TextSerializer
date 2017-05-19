using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections;
using System.IO;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvFluentConfigurationTests : BaseTests<CsvPocoRecord>
    {
        public CsvFluentConfigurationTests()
        : base("CsvWithOptions", "csv", Utilities.GetExpectations<CsvPocoRecord>("CsvPoco"))
        {
            Comparer = new RecordComparer();
            Serializer = new CsvSerializer<CsvPocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0))
                .ForMember(field => field.Name, opt => opt.Position(1))
                .ForMember(field => field.Description, opt => opt.Position(2))
                .ForMember(field => field.Value, opt => opt.Position(3))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4)));
        }

        [TestMethod]
        public override void SerializeTest()
        {
            var expectedRecords = Utilities.GetExpectations<CsvPocoRecord>("CsvPoco").List().ToArray();
            var expectedLines = Utilities.GetLines("Csv", Extension);

            for (int i = 0; i < expectedRecords.Length; i++)
            {
                string line = Serializer.Serialize(expectedRecords[i]);
                Assert.AreEqual(expectedLines[i], line);
            }
        }

        [TestMethod]
        public override void SerializeArrayTest()
        {
            var expectedRecords = Utilities.GetExpectations<CsvPocoRecord>("CsvPoco").List().ToArray();
            var expectedLines = Utilities.GetLines("Csv", Extension);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    Serializer.SerializeArray(writer, expectedRecords);
                    writer.Flush();
                    stream.Position = 0;
                    var lines = Utilities.GetLines(stream);

                    for (int i = 0; i < expectedLines.Count; i++)
                        Assert.AreEqual(expectedLines[i], lines[i]);
                }
            }
        }

        private class RecordComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var left = (CsvPocoRecord)x;
                var right = (CsvPocoRecord)y;

                bool equal = left.Id == right.Id &&
                             left.Name == right.Name &&
                             left.Description == right.Description &&
                             left.Value == right.Value &&
                             left.Enabled == right.Enabled;

                return equal ? 0 : 1;
            }
        }
    }
}