using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("INI")]
    public class IniAttributeTests
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
                Assert.AreEqual(IniModel.MyEnum.Second, model.EnumIntValue);
                Assert.AreEqual(IniModel.MyEnum.Third, model.EnumStringValue);
            }
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeFileWithExtraPropertiesIniTest()
        {
            IniSerializer<IniModel> iniSerializer = new IniSerializer<IniModel>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniNoSectionWithExtraFile.ini"))
            {
                var model = iniSerializer.Deserialize(reader);
            }
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeListToDictionaryIniTest()
        {
            IniSerializer<IniSimpleDictionaryModel> iniSerializer = new IniSerializer<IniSimpleDictionaryModel>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniSimpleListFile.ini"))
            {
                var model = iniSerializer.Deserialize(reader);
            }
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeDictionaryToListIniTest()
        {
            IniSerializer<IniSimpleListModel> iniSerializer = new IniSerializer<IniSimpleListModel>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniSimpleDictionaryFile.ini"))
            {
                var model = iniSerializer.Deserialize(reader);
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

                List<string> expectedList = new List<string> { "Thing 1", "Thing 2", "Thing 3", "Thing 4" };
                CollectionAssert.AreEqual(expectedList, model.StringList.ToList());
            }
        }

        [TestMethod]
        public void DeserializeSimpleDictionaryIniTest()
        {
            IniSerializer<IniSimpleDictionaryModel> iniSerializer = new IniSerializer<IniSimpleDictionaryModel>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniSimpleDictionaryFile.ini"))
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
        public void DeserializeSimpleListIniTest()
        {
            IniSerializer<IniSimpleListModel> iniSerializer = new IniSerializer<IniSimpleListModel>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniSimpleListFile.ini"))
            {
                List<IniSimpleListModel.MyEnum> expectedList = new List<IniSimpleListModel.MyEnum>
                {
                    IniSimpleListModel.MyEnum.Value1,
                    IniSimpleListModel.MyEnum.Value2,
                    IniSimpleListModel.MyEnum.Value3,
                    IniSimpleListModel.MyEnum.Value4
                };
                var model = iniSerializer.Deserialize(reader);
                CollectionAssert.AreEqual(expectedList, model.MyList);
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

        [TestMethod]
        public void DeserializeModelWithSubclassListTest()
        {
            IniSerializer<IniModelWithSubclassList> iniSerializer = new IniSerializer<IniModelWithSubclassList>();
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("IniModelWithSubclassDictionaryFile.ini"))   // Re-using this file (not a typo)
            {
                var model = iniSerializer.Deserialize(reader);
                Assert.AreEqual(1, model.IntValue);
                Assert.AreEqual(2.2, model.DoubleValue);
                Assert.AreEqual("Test String", model.StringValue);
                Assert.AreEqual(true, model.BoolValue);
                Assert.AreEqual(4, model.List.Count);

                int i = 0;
                foreach(var subclass in model.List)
                {
                    i++;
                    Assert.AreEqual($"Instance {i}", subclass.MySection);
                    Assert.AreEqual(i, subclass.MyValue);
                    Assert.AreEqual(true, subclass.BooleanValue);
                    Assert.AreEqual($"Test Instance {i}", subclass.TestString);
                }
            }
        }
    }
}