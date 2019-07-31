using System;
using System.Linq.Expressions;
using System.Reflection;

using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization.Utilities
{
    internal static class ReflectionHelper
    {
        public static bool IsUserDefinedStruct(this Type type)
        {
            return type.IsValueType && !type.IsPrimitive && !type.IsEnum;
        }

        public static bool IsUserDefinedClass(this Type type)
        {
            return type.IsClass && type.FullName != "System.String";
        }

        public static MemberInfo FindProperty(LambdaExpression lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;
            var done = false;

            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = ((LambdaExpression)expressionToCheck).Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = ((MemberExpression)expressionToCheck);

                        if (memberExpression.Expression.NodeType != ExpressionType.Parameter &&
                            memberExpression.Expression.NodeType != ExpressionType.Convert)
                        {
                            throw new ArgumentException(
                                $"Expression '{lambdaExpression}' must resolve to top-level member and not any child object's properties.",
                                nameof(lambdaExpression));
                        }

                        var member = memberExpression.Member;

                        return member;
                    default:
                        done = true;
                        break;
                }
            }

            throw new TextSerializationConfigurationException(
                "Custom configuration for members is only supported for top-level individual members on a type.");
        }

        /// <summary>Helper method which uses reflection to assign a value to a property of a class.</summary>
        /// <param name="obj">Class object which contains the property receiving the value</param>
        /// <param name="val">Value which is bieng assigned</param>
        /// <param name="member">MemberInfo of the Property or Field being assigned to.</param>
        public static void AssignToClass(object obj, object val, MemberInfo member)
        {
            if (member is PropertyInfo)
                ((PropertyInfo)member).SetValue(obj, val, null);
            else if (member is FieldInfo)
                ((FieldInfo)member).SetValue(obj, val);
            else
                throw new TextSerializationException("Invalid MemberInfo type encountered");
        }

        /// <summary>Helper method which uses reflection to assign a value to a property of a structure.</summary>
        /// <param name="obj">Structure object which contains the property receiving the value</param>
        /// <param name="val">Value which is bieng assigned</param>
        /// <param name="member">MemberInfo of the Property or Field being assigned to.</param>
        public static void AssignToStruct(ValueType obj, object val, MemberInfo member)
        {
            if (member is PropertyInfo)
                ((PropertyInfo)member).SetValue(obj, val, null);
            else if (member is FieldInfo)
                ((FieldInfo)member).SetValue(obj, val);
            else
                throw new TextSerializationException("Invalid MemberInfo type encountered");
        }
    }
}
