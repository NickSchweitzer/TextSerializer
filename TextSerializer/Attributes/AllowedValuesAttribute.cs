using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Defines the allowed characters that can be used for a field in the file.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    class AllowedValuesAttribute : Attribute
    {
        /// <summary>Default Constructor.</summary>
        /// <param name="allowedValues">Array of characters for each value that is permitted.</param>
        public AllowedValuesAttribute( params object[] allowedValues )
        {
            AllowedValues = allowedValues;
        }

        /// <summary>Array of characters for each value that is permitted.</summary>
        public object[] AllowedValues { get; set; }
    }
}