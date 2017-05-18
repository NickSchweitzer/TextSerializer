using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class CsvWithFormattingExpectations : IExpectations<CsvWithFormattingRecord>
    {
        private static List<CsvWithFormattingRecord> recordList = new List<CsvWithFormattingRecord>
        {
            new CsvWithFormattingRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new CsvWithFormattingRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description, with a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public CsvWithFormattingRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<CsvWithFormattingRecord> List()
        {
            return recordList;
        }
    }
}