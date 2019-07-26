using System;
using System.Collections.Generic;
using System.Text;

namespace TheCodingMonkey.Serialization.Utilities
{
    internal static class ParsingHelper
    {
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
    }
}