using System;
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

        public static readonly List<CsvRenamedHeaderRecord> CsvRenamedHeaderRecords = new List<CsvRenamedHeaderRecord>
        {
            new CsvRenamedHeaderRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new CsvRenamedHeaderRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public static readonly List<CsvStructRecord> CsvStructRecords = new List<CsvStructRecord>
        {
            new CsvStructRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new CsvStructRecord
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
                Enabled = true,
                IntOptions = OptionsEnum.FirstOption,
                StringOptions = OptionsEnum.SecondOption
            },
            new CsvWithFormattingRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description, with a Comma",
                Value = 123.4567,
                Enabled = false,
                IntOptions = OptionsEnum.SecondOption,
                StringOptions = OptionsEnum.FirstOption
            }
        };

        public static readonly List<CsvWithExtraFieldsRecord> CsvWithExtraFieldsRecords = new List<CsvWithExtraFieldsRecord>
        {
            new CsvWithExtraFieldsRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new CsvWithExtraFieldsRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
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
                Description = "Long Description without a Comma",
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
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public static readonly List<PocoRecord> PocoRecords = new List<PocoRecord>
        {
            new PocoRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new PocoRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public static readonly List<PocoWithEnumRecord> PocoWithEnumRecords = new List<PocoWithEnumRecord>
        {
            new PocoWithEnumRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Options = OptionsEnum.FirstOption
            },
            new PocoWithEnumRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Options = OptionsEnum.SecondOption
            }
        };

        public static readonly List<PocoStructRecord> PocoStructRecords = new List<PocoStructRecord>
        {
            new PocoStructRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new PocoStructRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public static readonly List<PocoMixedRecord> PocoMixedRecords = new List<PocoMixedRecord>
        {
            new PocoMixedRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new PocoMixedRecord
            {
                Id = 2,
                Name = "Second Record",
                Description = "Long Description without a Comma",
                Value = 123.4567,
                Enabled = false
            }
        };

        public static readonly List<PocoWithExtraFieldsRecord> PocoWithExtraFieldsRecords = new List<PocoWithExtraFieldsRecord>
        {
            new PocoWithExtraFieldsRecord
            {
                Id = 1,
                Name = "First Record",
                Description = "Long Description, with a Comma",
                Value = 3.14159,
                Enabled = true
            },
            new PocoWithExtraFieldsRecord
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
