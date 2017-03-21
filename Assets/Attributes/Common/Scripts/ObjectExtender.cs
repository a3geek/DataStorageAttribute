using System;
using System.Linq.Expressions;

namespace a3geek.Attributes.Common
{
    public static class ObjectExtender
    {
        public static string GetName<T>(Expression<Func<T>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

        public static string GetMyName<T>(Type type , Expression<Func<T>> expression)
        {
            return type.FullName.TrimStart(type.Namespace.ToCharArray()) + "." + GetName(expression);
        }
    }
}
