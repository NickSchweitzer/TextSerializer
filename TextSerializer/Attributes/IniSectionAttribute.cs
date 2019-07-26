using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>This attribute is applied to Fields or Properties of a class to control how an INI Section is declared.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class IniSectionAttribute : Attribute
    {
    }
}