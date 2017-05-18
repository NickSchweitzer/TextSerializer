using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class FixedWidthWithOptionsExpectations : IExpectations<FixedWidthWithOptionsRecord>
    {
        private static List<FixedWidthWithOptionsRecord> recordList = new List<FixedWidthWithOptionsRecord>
        {
            new FixedWidthWithOptionsRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new FixedWidthWithOptionsRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description, with a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public FixedWidthWithOptionsRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<FixedWidthWithOptionsRecord> List()
        {
            return recordList;
        }
    }
}