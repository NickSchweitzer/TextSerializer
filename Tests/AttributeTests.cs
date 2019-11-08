using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Formatters;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Attribute")]
    public class AttributeTests
    {
        [TestMethod]
        public void CsvAttributeTest()
        {
            TextFieldAttribute attribute = new TextFieldAttribute(1)
            {
                Size = 50,
                Name = "TestField",
                Optional = true,
                FormatterType = typeof(BooleanIntFormatter)
            };

            Assert.AreEqual(1, attribute.Position);
            Assert.AreEqual(50, attribute.Size);
            Assert.AreEqual("TestField", attribute.Name);
            Assert.AreEqual(true, attribute.Optional);
            Assert.AreEqual(typeof(BooleanIntFormatter), attribute.FormatterType);
        }

        [TestMethod]
        public void FixedWidthAttributeTest()
        {
            FixedWidthFieldAttribute attribute = new FixedWidthFieldAttribute(1, 50)
            {
                Padding = '0'
            };

            Assert.AreEqual(1, attribute.Position);
            Assert.AreEqual(50, attribute.Size);
            Assert.AreEqual('0', attribute.Padding);
        }
    }
}