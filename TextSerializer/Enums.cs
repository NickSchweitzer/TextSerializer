using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Enumeration which defines the different types of items that can be in an INI file for parsing purposes</summary>
    public enum IniLineType
    {
        /// <summary>Blank line, or a line with only whitepsace</summary>
        BlankLine,
        /// <summary>Comment line which begins with ;</summary>
        Comment,
        /// <summary>INI Section which is wrapped in []</summary>
        Section,
        /// <summary>Single item that may or may not be wrapped in quotes</summary>
        Item,
        /// <summary>Key Value Pair which may use quotes, and uses an = to split the key and value</summary>
        KeyValuePair
    }
}