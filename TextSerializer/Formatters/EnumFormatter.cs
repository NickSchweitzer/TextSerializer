using System;

namespace TheCodingMonkey.Serialization.Formatters
{
    /// <summary>Formatter which allows serialization and deserialization of Enums to Integer or String values in a file.</summary>
    public class EnumFormatter : ITextFormatter
    {
        private readonly Type EnumType;
        private readonly EnumOptions Options;

        /// <summary>EnumFormatter constructor which defines how to serialize an enumerated value.</summary>
        /// <param name="enumType">The type definition of the enumeration</param>
        /// <param name="options">Controls whether to write out the enumeration as its string or integer equivalent.</param>
        public EnumFormatter(Type enumType, EnumOptions options = EnumOptions.String)
        {
            EnumType = enumType;
            Options = options;
        }

        /// <summary>Deserializes a string and returns the enum equivalent.</summary>
        /// <param name="strValue">String value from file that must be deserialized.</param>
        /// <returns>Enum of the correct type based on the string value</returns>
        public object Deserialize(string strValue)
        {
            return Enum.Parse(EnumType, strValue);
        }

        /// <summary>Serializes an enum to an equivalent integer or string depending on the configuration</summary>
        /// <param name="objValue">Enum to serialize.</param>
        /// <returns>Correctly serialized value based on the EnumOptions</returns>
        public string Serialize(object objValue)
        {
            if (Options == EnumOptions.String)
                return objValue.ToString();
            else
                return ((int)objValue).ToString();
        }
    }

    /// <summary>Used to control how Enumerations are serialized to files.</summary>
    public enum EnumOptions
    {
        /// <summary>Enumerations will be Serialized using the Name of the Enum</summary>
        String,
        /// <summary>Enumerations will be Serialized using the Integer value of the Enum</summary>
        Integer
    }
}