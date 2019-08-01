using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>This attribute is applied to Fields, Properties, or the class/struct definition itself to control how an INI Section is declared.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class IniSectionAttribute : Attribute
    {
        /// <summary>Used when the section name is to be stored in a property of a class, and added to a list in another class.</summary>
        public IniSectionAttribute()
        { }

        /// <summary>Section name to be used. If no Name is provided, then the Class/Struct class name will be used, or the Property/Field name if applied at that level.</summary>
        public IniSectionAttribute(string name)
        {
            Name = name;
        }

        /// <summary>Section name to be used. If no Name is provided, then the Class/Struct class name will be used, or the Property/Field name if applied at that level.</summary>
        public string Name { get; set; }
    }
}