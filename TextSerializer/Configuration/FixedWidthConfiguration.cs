using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheCodingMonkey.Serialization.Configuration
{
    public class FixedWidthConfiguration<TTargetType> : TextConfiguration<TTargetType>
        where TTargetType : new()
    {
        private FixedWidthSerializer<TTargetType> FixedWidthSerializer { get; set; }

        internal FixedWidthConfiguration(FixedWidthSerializer<TTargetType> serializer)
        : base(serializer)
        {
            FixedWidthSerializer = serializer;
        }

    }
}