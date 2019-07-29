using System;
using System.Collections;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheCodingMonkey.Serialization.Tests.Helpers
{
    public static class Tests
    {
        public static ReflectionComparer GenericComparer = new ReflectionComparer();

        public static void DeserializeArrayTest<T>(string testFile, RecordSerializer<T> serializer, ICollection expectedResults, int count = -1)
            where T: new()
        {
            ICollection csvRecords;
            using (var reader = Utilities.OpenEmbeddedFile(testFile))
            {
                if (count > 0)
                    csvRecords = (ICollection)serializer.DeserializeArray(reader, count);
                else
                    csvRecords = (ICollection)serializer.DeserializeArray(reader);
            }

            if (count > 0)
            {
                T[] countArray = new T[count];
                IEnumerator resultsEnum = expectedResults.GetEnumerator();
                for (int i = 0; i < count; i++)
                {
                    resultsEnum.MoveNext();
                    countArray[i] = (T)resultsEnum.Current;
                }
                expectedResults = countArray;
            }

            CollectionAssert.AreEqual(expectedResults, csvRecords, GenericComparer);
        }

        public static void DeserializeArrayWithHeaderTest<T>(string testFile, CsvSerializer<T> serializer, ICollection expectedResults)
            where T : new()
        {
            ICollection csvRecords;
            using (var reader = Utilities.OpenEmbeddedFile(testFile))
            {
                csvRecords = (ICollection)serializer.DeserializeArray(reader, true);
            }

            CollectionAssert.AreEqual(expectedResults, csvRecords, GenericComparer);
        }

        public static void DeserializeEnumerableTest<T>(string testFile, RecordSerializer<T> serializer, ICollection expectedResults)
            where T : new()
        {
            var expectedArray = expectedResults.ToArray<T>();
            using (var reader = Utilities.OpenEmbeddedFile(testFile))
            {
                int i = 0;
                foreach (var record in serializer.DeserializeEnumerable(reader))
                {
                    Assert.AreEqual(0, GenericComparer.Compare(expectedArray[i++], record));
                }
                Assert.AreEqual(expectedArray.Length, i);
            }
        }

        public static void DeserializeEnumerableWithHeaderTest<T>(string testFile, CsvSerializer<T> serializer, ICollection expectedResults)
            where T : new()
        {
            var expectedArray = expectedResults.ToArray<T>();
            using (var reader = Utilities.OpenEmbeddedFile(testFile))
            {
                int i = 0;
                foreach (var record in serializer.DeserializeEnumerable(reader, true))
                {
                    Assert.AreEqual(0, GenericComparer.Compare(expectedArray[i++], record));
                }
                Assert.AreEqual(expectedArray.Length, i);
            }
        }

        public static void SerializeTest<T>(string testFile, RecordSerializer<T> serializer, ICollection testRecords)
            where T : new()
        {
            var expectedRecords = testRecords.ToArray<T>();
            var expectedLines = Utilities.GetLines(testFile);

            for (int i = 0; i < expectedRecords.Length; i++)
            {
                string line = serializer.Serialize(expectedRecords[i]);
                Assert.AreEqual(expectedLines[i], line);
            }
        }

        public static void SerializeWithHeaderTest<T>(string testFile, CsvSerializer<T> serializer, ICollection testRecords)
            where T : new()
        {
            var expectedRecords = testRecords.ToArray<T>();
            var expectedLines = Utilities.GetLines(testFile);

            string header = serializer.SerializeHeader();
            Assert.AreEqual(expectedLines[0], header);

            for (int i = 0; i < expectedRecords.Length; i++)
            {
                string line = serializer.Serialize(expectedRecords[i]);
                Assert.AreEqual(expectedLines[i + 1], line);
            }
        }

        public static void SerializeArrayTest<T>(string testFile, RecordSerializer<T> serializer, ICollection testRecords)
            where T : new()
        {
            var expectedRecords = testRecords.ToArray<T>();
            var expectedLines = Utilities.GetLines(testFile);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    serializer.SerializeArray(writer, expectedRecords);
                    writer.Flush();
                    stream.Position = 0;
                    var lines = Utilities.GetLines(stream);

                    for (int i = 0; i < expectedLines.Count; i++)
                        Assert.AreEqual(expectedLines[i], lines[i]);
                }
            }
        }

        [TestMethod]
        public static void SerializeArrayWithHeaderTest<T>(string testFile, CsvSerializer<T> serializer, ICollection testRecords)
            where T : new()
        {
            var expectedRecords = testRecords.ToArray<T>();
            var expectedLines = Utilities.GetLines(testFile);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    serializer.SerializeArray(writer, expectedRecords, true);
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