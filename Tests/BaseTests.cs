using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TheCodingMonkey.Serialization.Tests
{
    public abstract class BaseTests<T>
        where T : new()
    {
        protected readonly string TestFile;
        protected readonly string Extension;
        protected readonly IExpectations<T> Expectations;

        protected TextSerializer<T> Serializer;
        protected IComparer Comparer;

        protected BaseTests(string file, string ext)
        {
            TestFile = file;
            Extension = ext;
            Expectations = Utilities.GetExpectations<T>(TestFile);
        }

        protected BaseTests(string file, string ext, IExpectations<T> expectations)
        {
            TestFile = file;
            Extension = ext;
            Expectations = expectations;
        }

        [TestMethod]
        public virtual void DeserializeArrayTest()
        {
            ICollection csvRecords;
            using (var reader = Utilities.OpenEmbeddedFile(TestFile, Extension))
            {
                csvRecords = (ICollection)Serializer.DeserializeArray(reader);
            }

            var expectedRecords = (ICollection)Expectations.List();

            CollectionAssert.AreEqual(expectedRecords, csvRecords, Comparer);
        }

        [TestMethod]
        public virtual void DeserializeEnumerableTest()
        {
            var expectedRecords = Expectations.List().ToArray();
            using (var reader = Utilities.OpenEmbeddedFile(TestFile, Extension))
            {
                int i = 0;
                foreach (var record in Serializer.DeserializeEnumerable(reader))
                {
                    Assert.AreEqual(0, Comparer.Compare(expectedRecords[i++], record));
                }
                Assert.AreEqual(expectedRecords.Length, i);
            }
        }

        [TestMethod]
        public virtual void SerializeTest()
        {
            var expectedRecords = Expectations.List().ToArray();
            var expectedLines = Utilities.GetLines(TestFile, Extension);

            for (int i = 0; i < expectedRecords.Length; i++)
            {
                string line = Serializer.Serialize(expectedRecords[i]);
                Assert.AreEqual(expectedLines[i], line);
            }
        }

        [TestMethod]
        public virtual void SerializeArrayTest()
        {
            var expectedRecords = Expectations.List().ToArray();
            var expectedLines = Utilities.GetLines(TestFile, Extension);

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
    }
}