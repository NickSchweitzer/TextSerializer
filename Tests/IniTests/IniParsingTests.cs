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
                Assert.AreEqual(IniLineType.Comment, result.LineType);
                Assert.AreEqual("Comment Line", result.Key);
                Assert.IsNull(result.Value);
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
                Assert.AreEqual(IniLineType.KeyValuePair, result.LineType);
                Assert.AreEqual("Key", result.Key);
                Assert.AreEqual("Value", result.Value);
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
                Assert.AreEqual(IniLineType.KeyValuePair, result.LineType);
                Assert.AreEqual("Space Key", result.Key);
                Assert.AreEqual("Space Value", result.Value);
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
                Assert.AreEqual(IniLineType.Item, result.LineType);
                Assert.AreEqual("Item", result.Key);
                Assert.IsNull(result.Value);
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
                Assert.AreEqual(IniLineType.Item, result.LineType);
                Assert.AreEqual("Space Item", result.Key);
                Assert.IsNull(result.Value);
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
                Assert.AreEqual(IniLineType.BlankLine, result.LineType);
                Assert.IsNull(result.Key);
                Assert.IsNull(result.Value);
            }
        }
    }
}