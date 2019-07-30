using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}