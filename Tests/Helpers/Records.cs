﻿using System;
using System.Collections.Generic;

using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests.Helpers
{
    public static class Records
    {
        public static readonly List<CsvRecord> CsvRecords = new List<CsvRecord>
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

        public static readonly List<CsvRecord> CsvPipeDelimitedRecords = new List<CsvRecord>
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
                Description = "Long Description, with a Comma",
                Value = 123.4567,
                Enabled = false
            },
            new CsvRecord
            {
                Id = 3,
                Name = "Third Record",
                Description = "Long Description| with a Pipe",
                Value = 8675309,
                Enabled = true
            }
        };

        public static readonly List<CsvWithOptionsRecord> CsvWithOptionsRecords = new List<CsvWithOptionsRecord>
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

        public static readonly List<CsvWithFormattingRecord> CsvWithFormattingRecords = new List<CsvWithFormattingRecord>
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

        public static readonly List<FixedWidthRecord> FixedWidthRecords = new List<FixedWidthRecord>
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

        public static readonly List<FixedWidthWithOptionsRecord> FixedWidthWithOptionsRecords = new List<FixedWidthWithOptionsRecord>
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

        public static readonly List<CsvPocoRecord> CsvPocoRecords = new List<CsvPocoRecord>
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
    }
}
