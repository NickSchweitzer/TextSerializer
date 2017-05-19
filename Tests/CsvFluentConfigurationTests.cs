using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCodingMonkey.Serialization.Configuration;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvFluentConfigurationTests
    {
        [TestMethod]
        public void BasicConfigurationTest()
        {
            var serializer = new CsvSerializer<CsvPocoRecord>();
            var config = new CsvConfiguration<CsvPocoRecord>(serializer);
            config.AlwaysWriteQualifier()
                .Delimiter('|')
                .ForMember(field => field.Id, opt => opt.Optional().Position(1)); 

        }
    }
}