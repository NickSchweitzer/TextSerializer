using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Tests.Models;
using System.Collections;
using System.IO;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fixed Width")]
    public class FixedWidthWithOptionsTests : BaseTests<FixedWidthWithOptionsRecord>
    {
        public FixedWidthWithOptionsTests() : base("FixedWidthWithOptions", "txt")
        {
            Serializer = new FixedWidthSerializer<FixedWidthWithOptionsRecord>();
            Comparer = new RecordComparer();
        }

        [TestMethod]
        public override void SerializeTest()
        {
            var expectedRecords = Utilities.GetExpectations<FixedWidthWithOptionsRecord>(TestFile).List().ToArray();
            var expectedLines = Utilities.GetLines("FixedWidth", Extension);

            for (int i = 0; i < expectedRecords.Length; i++)
            {
                string line = Serializer.Serialize(expectedRecords[i]);
                Assert.AreEqual(expectedLines[i], line);
            }
        }

        [TestMethod]
        public override void SerializeArrayTest()
        {
            var expectedRecords = Utilities.GetExpectations<FixedWidthWithOptionsRecord>(TestFile).List().ToArray();
            var expectedLines = Utilities.GetLines("FixedWidth", Extension);

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
                var left = (FixedWidthWithOptionsRecord)x;
                var right = (FixedWidthWithOptionsRecord)y;

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