using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class BasicCsvExpectations : IExpectations<BasicCsvRecord>
    {
        private static List<BasicCsvRecord> recordList = new List<BasicCsvRecord>
        {
            new BasicCsvRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new BasicCsvRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description, with a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public BasicCsvRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<BasicCsvRecord> List()
        {
            return recordList;
        }
    }
}