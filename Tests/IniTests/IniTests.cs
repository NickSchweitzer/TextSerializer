using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("INI")]
    public class IniTests
    {
        [TestMethod]
        public void DeserializeNoSectionIniTest()
        {
            IniSerializer<IniModel> iniSerializer = new IniSerializer<IniModel>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniNoSectionFile.ini"))
            {
                var model = iniSerializer.Deserialize(reader);
                Assert.AreEqual(1, model.IntValue);
                Assert.AreEqual(2.2, model.DoubleValue);
                Assert.AreEqual("Test String", model.StringValue);
                Assert.AreEqual(true, model.BoolValue);
            }
        }

        [TestMethod]
        public void DeserializeSingleSectionAndListIniTest()
        {
            IniSerializer<IniModelWithList> iniSerializer = new IniSerializer<IniModelWithList>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniSingleSectionAndListFile.ini"))
            {
                var model = iniSerializer.Deserialize(reader);
                Assert.AreEqual(1, model.IntValue);
                Assert.AreEqual(2.2, model.DoubleValue);
                Assert.AreEqual("Test String", model.StringValue);
                Assert.AreEqual(true, model.BoolValue);

                List<string> expectedList = new List<string>
                {
                    "Thing 1",
                    "Thing 2",
                    "Thing 3",
                    "Thing 4"
                };

                CollectionAssert.AreEqual(expectedList, model.StringList);
            }
        }

        [TestMethod]
        public void DeserializeSimpleDictionaryIniTest()
        {
            IniSerializer<IniSimpleDictionaryModel> iniSerializer = new IniSerializer<IniSimpleDictionaryModel>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniSimpleDictionary.ini"))
            {
                var model = iniSerializer.Deserialize(reader);
                for (int i = 1; i < 5; i++)
                {
                    Assert.IsTrue(model.Dictionary.ContainsKey($"Key{i}"));
                    Assert.AreEqual(model.Dictionary[$"Key{i}"], $"Value{i}");
                }
            }
        }

        [TestMethod]
        public void DeserializeDictionarySectionIniTest()
        {
            IniSerializer<IniDictionarySectionModel> iniSerializer = new IniSerializer<IniDictionarySectionModel>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniDictionarySectionFile.ini"))
            {
                var model = iniSerializer.Deserialize(reader);
                Assert.AreEqual(1, model.IntValue);
                Assert.AreEqual(2.2, model.DoubleValue);
                Assert.AreEqual("Test String", model.StringValue);
                Assert.AreEqual(true, model.BoolValue);

                for (int i = 1; i < 5; i++)
                {
                    Assert.IsTrue(model.Dictionary.ContainsKey($"Key{i}"));
                    Assert.AreEqual(i, model.Dictionary[$"Key{i}"]);
                }
            }
        }

        [TestMethod]
        public void DeserializeModelWithSubclassTest()
        {
            IniSerializer<IniModelWithSubclass> iniSerializer = new IniSerializer<IniModelWithSubclass>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniModelWithSubclassFile.ini"))
            {
                var model = iniSerializer.Deserialize(reader);
                Assert.AreEqual(1, model.IntValue);
                Assert.AreEqual(2.2, model.DoubleValue);
                Assert.AreEqual("Test String", model.StringValue);
                Assert.AreEqual(true, model.BoolValue);

                Assert.AreEqual(3, model.Subclass.MyValue);
                Assert.AreEqual(true, model.Subclass.BooleanValue);
                Assert.AreEqual("Subclass", model.Subclass.TestString);
            }
        }

        [TestMethod]
        public void DeserializeModelWithSubclassDictionaryTest()
        {
            IniSerializer<IniModelWithSubclassDictionary> iniSerializer = new IniSerializer<IniModelWithSubclassDictionary>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniModelWithSubclassDictionaryFile.ini"))
            {
                var model = iniSerializer.Deserialize(reader);
                Assert.AreEqual(1, model.IntValue);
                Assert.AreEqual(2.2, model.DoubleValue);
                Assert.AreEqual("Test String", model.StringValue);
                Assert.AreEqual(true, model.BoolValue);

                for (int i = 1; i < 5; i++)
                {
                    Assert.IsTrue(model.Dictionary.ContainsKey($"Instance {i}"));
                    var subclass = model.Dictionary[$"Instance {i}"];
                    Assert.AreEqual(i, subclass.MyValue);
                    Assert.AreEqual(true, subclass.BooleanValue);
                    Assert.AreEqual($"Test Instance {i}", subclass.TestString);
                }
            }
        }
    }
}