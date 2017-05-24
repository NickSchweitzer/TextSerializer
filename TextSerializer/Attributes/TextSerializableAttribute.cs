using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>This attribute must be applied to a class to be serialized, unless the Fluent Configuration model is used</summary>
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false )]
    public class TextSerializableAttribute : Attribute
    {
        
        /// <summary>Default Constructor</summary>
        public TextSerializableAttribute()
        { }
    }
}