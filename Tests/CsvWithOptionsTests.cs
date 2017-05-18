using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Tests.Models;
using System.IO;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class CsvWithOptionsTests : BaseTests<CsvWithOptionsRecord>
    {
        public CsvWithOptionsTests() : base("CsvWithOptions", "csv")
        {
            Serializer = new CsvSerializer<CsvWithOptionsRecord>();
            Comparer = new RecordComparer();
        }

        [TestMethod]
        public override void SerializeTest()
        {
            var expectedRecords = Utilities.GetExpectations<CsvWithOptionsRecord>(TestFile).List().ToArray();
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
            var expectedRecords = Utilities.GetExpectations<CsvWithOptionsRecord>(TestFile).List().ToArray();
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
                var left = (CsvWithOptionsRecord)x;
                var right = (CsvWithOptionsRecord)y;

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