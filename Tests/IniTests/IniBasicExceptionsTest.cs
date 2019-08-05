using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Configuration;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("INI")]
    public class IniBasicExceptionsTest
    {
        [TestMethod]
        public void DeserializeNullReaderTest()
        {
            IniSerializer<IniModel> iniSerializer = new IniSerializer<IniModel>();
            Assert.ThrowsException<ArgumentNullException>(() => iniSerializer.Deserialize(null));
        }

        [TestMethod]
        public void MissingTextSerializableExceptionTest()
        {
            Assert.ThrowsException<TextSerializationConfigurationException>(() =>
                new IniSerializer<PocoRecord>());
        }
    }
}