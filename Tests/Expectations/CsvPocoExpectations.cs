using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class CsvPocoExpectations : IExpectations<CsvPocoRecord>
    {
        private static List<CsvPocoRecord> recordList = new List<CsvPocoRecord>
        {
            new CsvPocoRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new CsvPocoRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public CsvPocoRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<CsvPocoRecord> List()
        {
            return recordList;
        }
    }
}