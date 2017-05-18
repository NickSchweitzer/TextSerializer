using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class FixedWidthExpectations : IExpectations<FixedWidthRecord>
    {
        private static List<FixedWidthRecord> recordList = new List<FixedWidthRecord>
        {
            new FixedWidthRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new FixedWidthRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description, with a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public FixedWidthRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<FixedWidthRecord> List()
        {
            return recordList;
        }
    }
}