using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class FormattedCsvTests : BaseTests<FormattedCsvRecord>
    {
        public FormattedCsvTests() : base("FormattedCsv", "csv")
        {
            Serializer = new CsvSerializer<FormattedCsvRecord>();
            Comparer = new RecordComparer();
        }

        private class RecordComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var left = (FormattedCsvRecord)x;
                var right = (FormattedCsvRecord)y;

                bool equal = left.Id == right.Id &&
                             left.Name == right.Name &&
                             left.Description == right.Description &&
                             left.Value == right.Value &&
                             left.Enabled == right.Enabled;

                return equal ? 0 : 1; 
            }
        }
    }
}