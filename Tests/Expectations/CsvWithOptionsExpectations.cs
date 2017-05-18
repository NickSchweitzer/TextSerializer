using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class CsvWithOptionsExpectations : IExpectations<CsvWithOptionsRecord>
    {
        private static List<CsvWithOptionsRecord> recordList = new List<CsvWithOptionsRecord>
        {
            new CsvWithOptionsRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new CsvWithOptionsRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public CsvWithOptionsRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<CsvWithOptionsRecord> List()
        {
            return recordList;
        }
    }
}