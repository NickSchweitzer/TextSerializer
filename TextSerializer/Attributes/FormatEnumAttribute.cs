using System;

using TheCodingMonkey.Serialization.Formatters;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Controls how to format enumerations in a file, either String or Integer</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FormatEnumAttribute : Attribute
    {
        /// <summary>Default Constructor.</summary>
        /// <param name="options">Format to use for formatting enumerations, either String or Integer.</param>
        public FormatEnumAttribute(EnumOptions options)
        {
            Options = options;
        }

        /// <summary>Format to use for formatting enumerations, either String or Integer.</summary>
        public EnumOptions Options { get; set; }
    }
}