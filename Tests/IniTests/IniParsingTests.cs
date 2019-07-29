using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Utilities;

namespace TheCodingMonkey.Serialization.Tests
{

    [TestClass, TestCategory("Parsing")]
    public class IniParsingTests
    {
        [TestMethod]
        public void IniParseCommentsTest()
        {
            string[] sampleComments =
            {
                "; Comment Line",
                "    ;   Comment Line     ",
                "\t; Comment Line"
            };

            foreach (string comment in sampleComments)
            {
                var result = ParsingHelper.ParseIniLine(comment);
                Assert.AreEqual(IniLineType.Comment, result.Item1);
                Assert.AreEqual("Comment Line", result.Item2);
                Assert.IsNull(result.Item3);
            }
        }

        [TestMethod]
        public void IniParseKeyValuePairs()
        {
            string[] sampleKvps =
            {
                "Key=Value",
                " Key = Value   ",
                "\tKey\t=\tValue",
                "\"Key\" = \"Value\""
            };

            foreach (string kvp in sampleKvps)
            {
                var result = ParsingHelper.ParseIniLine(kvp);
                Assert.AreEqual(IniLineType.KeyValuePair, result.Item1);
                Assert.AreEqual("Key", result.Item2);
                Assert.AreEqual("Value", result.Item3);
            }
        }

        [TestMethod]
        public void IniParseKeyValuePairsWithSpaces()
        {
            string[] sampleKvps =
            {
                "Space Key=Space Value",
                " Space Key = Space Value   ",
                "\tSpace Key\t=\tSpace Value",
                "\"Space Key\" = \"Space Value\""
            };

            foreach (string kvp in sampleKvps)
            {
                var result = ParsingHelper.ParseIniLine(kvp);
                Assert.AreEqual(IniLineType.KeyValuePair, result.Item1);
                Assert.AreEqual("Space Key", result.Item2);
                Assert.AreEqual("Space Value", result.Item3);
            }
        }

        [TestMethod]
        public void IniParseItem()
        {
            string[] sampleItems =
            {
                "Item",
                " Item   ",
                "\tItem \t",
                "\"Item\""
            };

            foreach (string item in sampleItems)
            {
                var result = ParsingHelper.ParseIniLine(item);
                Assert.AreEqual(IniLineType.Item, result.Item1);
                Assert.AreEqual("Item", result.Item2);
                Assert.IsNull(result.Item3);
            }
        }

        [TestMethod]
        public void IniParseItemsWithSpaces()
        {
            string[] sampleItems =
            {
                "Space Item",
                " Space Item    ",
                "\tSpace Item\t ",
                "\"Space Item\""
            };

            foreach (string item in sampleItems)
            {
                var result = ParsingHelper.ParseIniLine(item);
                Assert.AreEqual(IniLineType.Item, result.Item1);
                Assert.AreEqual("Space Item", result.Item2);
                Assert.IsNull(result.Item3);
            }
        }

        [TestMethod]
        public void IniParseBlankLines()
        {
            string[] sampleItems =
            {
                "",
                "         ",
                " ",
                "\t",
                " \t "
            };

            foreach (string item in sampleItems)
            {
                var result = ParsingHelper.ParseIniLine(item);
                Assert.AreEqual(IniLineType.BlankLine, result.Item1);
                Assert.IsNull(result.Item2);
                Assert.IsNull(result.Item3);
            }
        }
    }
}