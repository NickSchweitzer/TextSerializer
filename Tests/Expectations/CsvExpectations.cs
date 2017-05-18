using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class CsvExpectations : IExpectations<CsvRecord>
    {
        private static List<CsvRecord> recordList = new List<CsvRecord>
        {
            new CsvRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new CsvRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public CsvRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<CsvRecord> List()
        {
            return recordList;
        }
    }
}