using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Tests.Models;
using System.Collections;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fixed Width")]
    public class FixedWidthTests : BaseTests<FixedWidthRecord>
    {
        public FixedWidthTests() : base("FixedWidth", "txt")
        {
            Serializer = new FixedWidthSerializer<FixedWidthRecord>();
            Comparer = new RecordComparer();
        }

        private class RecordComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var left = (FixedWidthRecord)x;
                var right = (FixedWidthRecord)y;

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