﻿using System;
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
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("NoSectionIniFile.ini"))
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
            using (var reader = Helpers.Utilities.OpenEmbeddedFile("SingleSectionAndListIniFile.ini"))
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
    }
}