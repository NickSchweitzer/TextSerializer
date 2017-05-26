using System;

namespace TheCodingMonkey.Serialization.Formatters
{
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

    public enum EnumOptions
    {
        String,
        Integer
    }
}
