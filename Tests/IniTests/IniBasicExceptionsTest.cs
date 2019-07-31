using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Configuration;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("INI")]
    public class IniBasicExceptionsTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DeserializeNullReaderTest()
        {
            IniSerializer<IniModel> iniSerializer = new IniSerializer<IniModel>();
            iniSerializer.Deserialize(null);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationConfigurationException))]
        public void MissingTextSerializableExceptionTest()
        {
            IniSerializer<PocoRecord> iniSerializer = new IniSerializer<PocoRecord>();
        }
    }
}