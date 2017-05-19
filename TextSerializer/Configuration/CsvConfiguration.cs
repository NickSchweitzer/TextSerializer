using System;
using System.Linq.Expressions;

using TheCodingMonkey.Serialization.Utilities;

namespace TheCodingMonkey.Serialization.Configuration
{
    public class CsvConfiguration<TTargetType> : TextConfiguration<TTargetType>
        where TTargetType : new()
    {
        private CsvSerializer<TTargetType> CsvSerializer { get; set; }

        public CsvConfiguration(CsvSerializer<TTargetType> serializer)
        : base(serializer)
        {
            CsvSerializer = serializer;

            AlwaysWriteQualifier(true);
            Delimiter(',');
            Qualifier('"');
        }

        /// <summary>True if should wrap every field in the <see cref="Qualifier">Qualifier</see> during serialization.  If false, then
        /// the qualifier is only written if the field contains the <see cref="Delimiter">Delimiter</see>.</summary>
        public CsvConfiguration<TTargetType> AlwaysWriteQualifier(bool alwaysWrite = true)
        {
            CsvSerializer.AlwaysWriteQualifier = alwaysWrite;
            return this;
        }

        /// <summary>Character which is used to delimit fields in the record.</summary>
        public CsvConfiguration<TTargetType> Delimiter(char delim)
        {
            CsvSerializer.Delimiter = delim;
            return this;
        }

        /// <summary>Character used to wrap a field if the field contins the <see cref="Delimiter">Delimiter</see>.</summary>
        public CsvConfiguration<TTargetType> Qualifier(char qualifier)
        {
            CsvSerializer.Qualifier = qualifier;
            return this;
        }

        public CsvConfiguration<TTargetType> ForMember(Expression<Func<TTargetType, object>> field, Action<TextFieldConfiguration> opt)
        {
            var member = ReflectionHelper.FindProperty(field);
            var attr = new TextFieldAttribute
            {
                Member = member,
                Name = member.Name
            };

            TextFieldConfiguration fieldConfig = new TextFieldConfiguration(attr);
            opt.Invoke(fieldConfig);
            CsvSerializer.Fields.Add(attr.Position, attr);
            return this;
        }
    }
}