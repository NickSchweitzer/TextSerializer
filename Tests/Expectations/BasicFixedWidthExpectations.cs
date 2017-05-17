using System;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Expectations
{
    internal class BasicFixedWidthExpectations : IExpectations<BasicFixedWidthRecord>
    {
        private static List<BasicFixedWidthRecord> recordList = new List<BasicFixedWidthRecord>
        {
            new BasicFixedWidthRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new BasicFixedWidthRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description, with a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public BasicFixedWidthRecord Item(int index)
        {
            return recordList[index];
        }

        public ICollection<BasicFixedWidthRecord> List()
        {
            return recordList;
        }
    }
}