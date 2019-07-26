using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>This attribute is applied to Fields or Properties of a class to control how an INI field is handled.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class IniFieldAttribute : Attribute
    {
    }
}