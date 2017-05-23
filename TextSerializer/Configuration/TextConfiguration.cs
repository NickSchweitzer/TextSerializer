using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TheCodingMonkey.Serialization.Utilities;

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

        /// <summary>Marks a field as not serializable.</summary>
        /// <param name="field">Field to ignore.</param>
        protected void InternalIgnore(Expression<Func<TTargetType, object>> field)
        {
            var member = ReflectionHelper.FindProperty(field);
            var kvp = GetFieldPair(member);
            if (kvp != null)
                Serializer.Fields.Remove(kvp.Value.Key);
        }

        protected KeyValuePair<int, Field>? GetFieldPair(MemberInfo member)
        {
            KeyValuePair<int, Field>? foundKvp = null;
            foreach (var kvp in Serializer.Fields)
            {
                if (kvp.Value.Member.Name == member.Name)
                {
                    foundKvp = kvp;
                    break;
                }
            }
            return foundKvp;
        }
    }
}