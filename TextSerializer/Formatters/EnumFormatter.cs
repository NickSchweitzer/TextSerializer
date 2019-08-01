using System;

namespace TheCodingMonkey.Serialization.Formatters
{
    // TODO - Consider making this public and writing unit tests around it
    internal class EnumFormatter : ITextFormatter
    {
        private readonly Type EnumType;
        private readonly EnumOptions Options;

        public EnumFormatter(Type enumType, EnumOptions options = EnumOptions.String)
        {
            EnumType = enumType;
            Options = options;
        }
        public object Deserialize(string strValue)
        {
            return Enum.Parse(EnumType, strValue);
        }

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