using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Helpers;
using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class FixedWidthFluentConfigurationTests : CsvFluentConfigurationTests
    {
        public FixedWidthFluentConfigurationTests()
        {
            DeserializeTestFile = "FixedWidthWithOptionsFile.txt";
            SerializeTestFile = "FixedWidthFile.txt";
            Serializer = new FixedWidthSerializer<PocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0).Size(5).Padding('0'))
                .ForMember(field => field.Name, opt => opt.Position(1).Size(15))
                .ForMember(field => field.Description, opt => opt.Position(2).Size(35))
                .ForMember(field => field.Value, opt => opt.Position(3).Size(8).Padding('0'))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4).Size(5)));
        }

        [TestMethod, ExpectedException(typeof(TextSerializationConfigurationException))]
        public void SizeNotSpecifiedTest()
        {
            Serializer = new FixedWidthSerializer<PocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0).Padding('0')));
        }

        [TestMethod, ExpectedException(typeof(TextSerializationConfigurationException))]
        public void ZeroSizeTest()
        {
            Serializer = new FixedWidthSerializer<PocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0).Size(0).Padding('0')));
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void ConfigurationTooLongTest()
        {
            Serializer = new FixedWidthSerializer<PocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0).Size(5).Padding('0'))         
                .ForMember(field => field.Name, opt => opt.Position(1).Size(15))
                .ForMember(field => field.Description, opt => opt.Position(2).Size(35))
                .ForMember(field => field.Value, opt => opt.Position(3).Size(8).Padding('0'))
                .ForMember(field => field.Enabled, opt => opt.Position(4).Size(10)));               // Makes it too long

            Helpers.Tests.DeserializeArrayTest("FixedWidthFile.txt", Serializer, Records.PocoRecords);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void ConfigurationTooShortTest()
        {
            Serializer = new FixedWidthSerializer<PocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0).Size(5).Padding('0'))
                .ForMember(field => field.Name, opt => opt.Position(1).Size(15))
                .ForMember(field => field.Description, opt => opt.Position(2).Size(35))
                .ForMember(field => field.Value, opt => opt.Position(3).Size(8).Padding('0')));
                //.ForMember(field => field.Enabled, opt => opt.Position(4).Size(5)));          // Commented out on purpose, makes record too short

            Helpers.Tests.DeserializeArrayTest("FixedWidthFile.txt", Serializer, Records.PocoRecords);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationConfigurationException))]
        public void MissingFieldTest()
        {
            Serializer = new FixedWidthSerializer<PocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0).Size(5).Padding('0'))
                .ForMember(field => field.Name, opt => opt.Position(1).Size(15))
                //.ForMember(field => field.Description, opt => opt.Position(2).Size(35)) // Commented on purpose to have a missing field
                .ForMember(field => field.Value, opt => opt.Position(3).Size(8).Padding('0'))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4).Size(5)));
        }
    }
}