using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Tests.Models;
using System.IO;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class BasicCsvWithHeaderTests
    {
        protected CsvSerializer<BasicCsvRecord> Serializer;
        protected readonly string TestFile;
        protected readonly string Extension;
        protected IComparer Comparer;

        public BasicCsvWithHeaderTests()
        {
            TestFile = "BasicCsvWithHeader";
            Extension = "csv";
            Serializer = new CsvSerializer<BasicCsvRecord>();
            Comparer = new RecordComparer();
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            ICollection csvRecords;
            using (var reader = Utilities.OpenEmbeddedFile(TestFile, Extension))
            {
                csvRecords = (ICollection)Serializer.DeserializeArray(reader, true);
            }

            var expectedRecords = (ICollection)Utilities.GetExpectations<BasicCsvRecord>("BasicCsv").List();

            CollectionAssert.AreEqual(expectedRecords, csvRecords, Comparer);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            var expectedRecords = Utilities.GetExpectations<BasicCsvRecord>("BasicCsv").List().ToArray();
            using (var reader = Utilities.OpenEmbeddedFile(TestFile, Extension))
            {
                int i = 0;
                foreach (var record in Serializer.DeserializeEnumerable(reader, true))
                {
                    Assert.AreEqual(0, Comparer.Compare(expectedRecords[i++], record));
                }
                Assert.AreEqual(expectedRecords.Length, i);
            }
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeWithoutHeaderTest()
        {
            using (var reader = Utilities.OpenEmbeddedFile("BasicCsv", Extension))
            {
                Serializer.DeserializeArray(reader, true);
            }
        }

        [TestMethod]
        public void SerializeTest()
        {
            var expectedRecords = Utilities.GetExpectations<BasicCsvRecord>("BasicCsv").List().ToArray();
            var expectedLines = Utilities.GetLines(TestFile, Extension);

            for (int i = 0; i < expectedRecords.Length; i++)
            {
                string line = Serializer.Serialize(expectedRecords[i]);
                Assert.AreEqual(expectedLines[i+1], line);
            }
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            var expectedRecords = Utilities.GetExpectations<BasicCsvRecord>("BasicCsv").List().ToArray();
            var expectedLines = Utilities.GetLines(TestFile, Extension);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    Serializer.SerializeArray(writer, expectedRecords, true);
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
                var left = (BasicCsvRecord)x;
                var right = (BasicCsvRecord)y;

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