using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Tests.Models;
using System.Collections;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fixed Width")]
    public class BasicFixedWidthTests : BaseTests<BasicFixedWidthRecord>
    {
        public BasicFixedWidthTests() : base("BasicFixedWidth", "txt")
        {
            Serializer = new FixedWidthSerializer<BasicFixedWidthRecord>();
            Comparer = new RecordComparer();
        }

        private class RecordComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var left = (BasicFixedWidthRecord)x;
                var right = (BasicFixedWidthRecord)y;

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