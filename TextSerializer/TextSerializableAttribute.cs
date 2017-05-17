using System;

namespace TheCodingMonkey.Serialization
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false )]
    public class TextSerializableAttribute : Attribute
    {
        public TextSerializableAttribute()
        { }
    }
}