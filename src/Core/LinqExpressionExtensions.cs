using System;
using System.Linq.Expressions;
using System.Reflection;

namespace YuzuDelivery.Umbraco.Core
{
    public static class LinqExpressionExtensions
    {
        public static string GetMemberName<Type, Member>(this Expression<Func<Type, Member>> lambda)
        {
            switch (lambda.Body)
            {
                case MethodCallExpression methodExpression:
                {
                    var propInfo = methodExpression.Method;
                    if (propInfo == null)
                        throw new ArgumentException($"Expression '{lambda}' refers to a field, not a property.");
                    return propInfo.Name;
                }
                case MemberExpression memberExpression:
                {
                    var propInfo = memberExpression.Member as PropertyInfo;
                    if (propInfo == null)
                        throw new ArgumentException($"Expression '{lambda}' refers to a field, not a property.");
                    return propInfo.Name;
                }
                default:
                    throw new ArgumentException($"Expression '{lambda}' not supported.");
            }
        }
    }
}
