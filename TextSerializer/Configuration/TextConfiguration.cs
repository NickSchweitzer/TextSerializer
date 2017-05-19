using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TheCodingMonkey.Serialization.Configuration
{
    public abstract class TextConfiguration<TTargetType>
        where TTargetType : new()
    {
        protected TextSerializer<TTargetType> Serializer { get; set; }

        internal TextConfiguration(TextSerializer<TTargetType> serializer)
        {
            Serializer = serializer;
        }
    }
}