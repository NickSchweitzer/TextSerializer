using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class BasicCsvWithOptionsExpectations : IExpectations<BasicCsvWithOptionsRecord>
    {
        private static List<BasicCsvWithOptionsRecord> recordList = new List<BasicCsvWithOptionsRecord>
        {
            new BasicCsvWithOptionsRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new BasicCsvWithOptionsRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public BasicCsvWithOptionsRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<BasicCsvWithOptionsRecord> List()
        {
            return recordList;
        }
    }
}