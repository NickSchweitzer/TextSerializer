using System;
using System.Collections;
using System.Reflection;

namespace TheCodingMonkey.Serialization.Tests.Helpers
{
    public class ReflectionComparer : IComparer
    {
        public int Compare(object left, object right)
        {
            if (left.GetType() != right.GetType())
                throw new ArgumentException("Type mismatch");

            // Get all the public properties and fields on the class
            MemberInfo[] members = left.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty);
            foreach (MemberInfo member in members)
            {
                object leftValue = GetValue(member, left);
                object rightValue = GetValue(member, right);

                if (leftValue == null && rightValue == null)    // Not a property or field
                    continue;

                if (!leftValue.Equals(rightValue))  // Found a field/property that's not equal
                    return 1;
            }

            return 0;   // If got here then everything is equal
        }

        private static object GetValue(MemberInfo member, object obj)
        {
            object objValue = null;

            // Get the object from the field
            if (member is PropertyInfo)
                objValue = ((PropertyInfo)member).GetValue(obj, null);
            else if (member is FieldInfo)
                objValue = ((FieldInfo)member).GetValue(obj);

            return objValue;
        }
    }
}