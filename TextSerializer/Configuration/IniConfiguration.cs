using System;
using System.Collections.Generic;
using System.Text;

namespace TheCodingMonkey.Serialization.Configuration
{
    /// <summary>Fluent Configuration class for the <see cref="CsvSerializer{TTargetType}">CsvSerializer</see> class.</summary>
    public class IniConfiguration<TTargetType> : BaseConfiguration<TTargetType>
        where TTargetType : new()
    {
        private IniSerializer<TTargetType> IniSerializer { get; set; }

        internal IniConfiguration(IniSerializer<TTargetType> serializer)
        : base(serializer)
        {
            IniSerializer = serializer;
        }
    }
}
