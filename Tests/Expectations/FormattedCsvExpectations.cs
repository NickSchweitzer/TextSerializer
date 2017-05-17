using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class FormattedCsvExpectations : IExpectations<FormattedCsvRecord>
    {
        private static List<FormattedCsvRecord> recordList = new List<FormattedCsvRecord>
        {
            new FormattedCsvRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new FormattedCsvRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description, with a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public FormattedCsvRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<FormattedCsvRecord> List()
        {
            return recordList;
        }
    }
}