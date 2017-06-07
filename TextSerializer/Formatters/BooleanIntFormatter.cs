using System;

namespace TheCodingMonkey.Serialization.Formatters
{
    /// <summary>Formatter which allows serialization and deserialization of Booleans to Integer values in a file.</summary>
    public class BooleanIntFormatter : ITextFormatter
    {
        /// <summary>Deserializes a string and returns the boolean equivalent.</summary>
        /// <param name="strValue">String value from file that must be deserialized.</param>
        /// <returns>False for 0, True for any other integer value.</returns>
        public object Deserialize(string strValue)
        {
            if (int.TryParse(strValue, out int value))
                return value == 0 ? false : true;

            throw new FormatException($"{strValue} is not an integer that can be converted to boolean");
        }

        /// <summary>Serializes a boolean to an equivalent integer</summary>
        /// <param name="objValue">Boolean to serialize.</param>
        /// <returns>0 for False, 1 for True.</returns>
        public string Serialize(object objValue)
        {
            bool value = (bool)objValue;
            return value ? "1" : "0";
        }
    }
}