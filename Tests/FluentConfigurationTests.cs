using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class FluentConfigurationTests
    {
        [TestMethod]
        public void BasicCsvConfigurationTest()
        {
            CsvSerializer<PocoWithExtraFieldsRecord> serializer = new CsvSerializer<PocoWithExtraFieldsRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Name("UniqueId").Position(0))
                .ForMember(field => field.Value, opt => opt.Position(3))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4))
                .ForMember(field => field.Name, opt => opt.Size(20).Position(1))
                .ForMember(field => field.Description, opt => opt.Position(2))
                .Ignore(field => field.ExtraField));

            // Guarantee that all fields are in order with no gaps
            Assert.AreEqual(5, serializer.Fields.Count);
            for (int i = 0; i < serializer.Fields.Count; i++)
            {
                Assert.AreEqual(i, serializer.Fields[i].Position);
            }

            Assert.AreEqual("UniqueId", ((CsvField)serializer.Fields[0]).Name);
            Assert.AreEqual(20, serializer.Fields[1].Size);
            Assert.AreEqual(true, serializer.Fields[4].Optional);
        }

        [TestMethod]
        public void BasicFixedWidthConfigurationTest()
        {
            FixedWidthSerializer<PocoWithExtraFieldsRecord> serializer = new FixedWidthSerializer<PocoWithExtraFieldsRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0).Size(5).Padding('0'))
                .ForMember(field => field.Value, opt => opt.Position(3).Size(8).Padding('0'))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4).Size(5))
                .ForMember(field => field.Name, opt => opt.Position(1).Size(15))
                .ForMember(field => field.Description, opt => opt.Position(2))
                .ForMember(field => field.Description, opt => opt.Size(35))
                .Ignore(field => field.ExtraField));

            // Guarantee that all fields are in order with no gaps
            Assert.AreEqual(5, serializer.Fields.Count);
            for (int i = 0; i < serializer.Fields.Count; i++)
            {
                Assert.AreEqual(i, serializer.Fields[i].Position);
            }

            Assert.AreEqual(5, serializer.Fields[0].Size);
            Assert.AreEqual(15, serializer.Fields[1].Size);
            Assert.AreEqual(35, serializer.Fields[2].Size);
            Assert.AreEqual(8, serializer.Fields[3].Size);
            Assert.AreEqual(5, serializer.Fields[4].Size);

            Assert.AreEqual('0', ((FixedWidthField)serializer.Fields[0]).Padding);
            Assert.AreEqual('0', ((FixedWidthField)serializer.Fields[3]).Padding);
            Assert.AreEqual(' ', ((FixedWidthField)serializer.Fields[1]).Padding);

            Assert.AreEqual(true, serializer.Fields[4].Optional);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationConfigurationException))]
        public void MissingFieldTest()
        {
            CsvSerializer<PocoWithExtraFieldsRecord> serializer = new CsvSerializer<PocoWithExtraFieldsRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Name("UniqueId").Position(0))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4))
                .ForMember(field => field.Name, opt => opt.Size(20).Position(1))
                .ForMember(field => field.Description, opt => opt.Position(2))
                .Ignore(field => field.ExtraField));
        }

        [TestMethod]
        public void ConfigureEventExceptionTest()
        {
            CsvSerializer<PocoWithExtraFieldsRecord> serializer = new CsvSerializer<PocoWithExtraFieldsRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Name("UniqueId").Position(0))
                .ForMember(field => field.Value, opt => opt.Position(3))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4))
                .ForMember(field => field.Name, opt => opt.Size(20).Position(1))
                .ForMember(field => field.Description, opt => opt.Position(2))
                .Ignore(field => field.ExtraField));
        }
    }
}