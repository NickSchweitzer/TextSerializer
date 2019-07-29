using System;
using System.Collections.Generic;
using System.Text;

namespace TheCodingMonkey.Serialization.Utilities
{
    /// <summary>Helper class which contains some common code for parsing CSV and Ini Files</summary>
    public static class ParsingHelper
    {
        /// <summary>Helper method which safely truncates a string to a given length</summary>
        /// <param name="str">String value to truncate</param>
        /// <param name="length">Maximum allowed length of the string</param>
        /// <returns>Truncated string value</returns>
        public static string Truncate(string str, int length)
        {
            if (length > -1 && str.Length > length)
                str = str.Substring(0, length);

            return str;
        }

        /// <summary>Parses a single line of text as a CSV record into component strings for each column.</summary>
        /// <param name="text">Complete line of text to be parsed</param>
        /// <param name="qualifier">Character to use as a text qualifier (usually ")</param>
        /// <param name="delimiter">Delimiter character to split fields (usually ,)</param>
        /// <returns>List of strings where each item is a column in the CSV</returns>
        public static List<string> ParseDelimited(string text, char qualifier, char delimiter)
        {
            List<string> returnList = new List<string>();       // Return value
            bool countDelimiter = true;                         // If we hit the delimiter character, should it be treated as a delimiter?
            StringBuilder currentField = new StringBuilder();   // Current field we're parsing through

            foreach (char ch in text)
            {
                if (ch == qualifier)
                {
                    // We found a Qualifier character (usually a quote), so should treat a delimiter character as part of the field
                    countDelimiter = !countDelimiter;
                }
                else if (ch == delimiter)
                {
                    if (countDelimiter)
                    {
                        // Found a delimiter, so end the field we're building up, add to our return list and clear the current field
                        returnList.Add(currentField.ToString());
                        currentField = new StringBuilder();
                    }
                    else
                    {
                        // Inside of a qualified field, so just add this to the field string as usual
                        currentField.Append(ch);
                    }
                }
                else
                {
                    // Add this to the field string... just a normal character
                    currentField.Append(ch);
                }
            }

            // End of record, so add whatever we have to the list
            if (countDelimiter)
                returnList.Add(currentField.ToString());

            return returnList;
        }

        /// <summary>Parses a single line from an INI file and determines the type of item it is. Also trims whitespace apropriately, and removes quotes</summary>
        /// <param name="text">Complete line of text to be parsed</param>
        /// <returns>A Tuple which contains the parsed line:
        /// Item1 - <see cref="IniLineType">IniLineType</see> for this line
        /// Item2 - For Blank Line, null. For Comment and Item, trimmed text. For KeyValuePair, the trimmed Key.
        /// Item3 - For KeyValuePair, the trimmed value. For all others null.</returns>
        public static Tuple<IniLineType, string, string> ParseIniLine(string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return new Tuple<IniLineType, string, string>(IniLineType.BlankLine, null, null);

            if (text.StartsWith(";"))
                return new Tuple<IniLineType, string, string>(IniLineType.Comment, text.Substring(1, text.Length - 1).Trim(), null);

            if (text.StartsWith("[") && text.EndsWith("]"))
                return new Tuple<IniLineType, string, string>(IniLineType.Section, text.Substring(1, text.Length - 2), null);

            List<string> parsedLine = ParsingHelper.ParseDelimited(text, '\"', '=');
            if (parsedLine.Count == 1)
                return new Tuple<IniLineType, string, string>(IniLineType.Item, parsedLine[0].Trim(), null);
            else if (parsedLine.Count == 2)
                return new Tuple<IniLineType, string, string>(IniLineType.KeyValuePair, parsedLine[0].Trim(), parsedLine[1].Trim());

            throw new TextSerializationException($"Cannot parse line in INI file '{text}'");
        }
    }
}