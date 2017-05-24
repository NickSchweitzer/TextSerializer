using System;
using System.Linq.Expressions;
using System.Reflection;

using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization.Utilities
{
    internal static class ReflectionHelper
    {
        public static MemberInfo GetFieldOrProperty(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            return memberExpression != null
                ? memberExpression.Member
                : throw new ArgumentOutOfRangeException(nameof(expression), "Expected a property/field access expression, not " + expression);
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
    }
}
