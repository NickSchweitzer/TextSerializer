using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheCodingMonkey.Serialization.Tests
{
    public abstract class BaseTests<T>
        where T : new()
    {
        protected TextSerializer<T> Serializer;
        protected readonly string TestFile;
        protected readonly string Extension;
        protected IComparer Comparer;

        protected BaseTests(string file, string ext)
        {
            TestFile = file;
            Extension = ext;
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            ICollection csvRecords;
            using (var reader = Utilities.OpenEmbeddedFile(TestFile, Extension))
            {
                csvRecords = (ICollection)Serializer.DeserializeArray(reader);
            }

            var expectedRecords = (ICollection)Utilities.GetExpectations<T>(TestFile).List();

            CollectionAssert.AreEqual(expectedRecords, csvRecords, Comparer);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            var expectedRecords = Utilities.GetExpectations<T>(TestFile).List().ToArray();
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

    }
}
