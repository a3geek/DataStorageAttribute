using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace a3geek.Attributes.Common
{
    public static class AttributeExtender
    {
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = false)
        {
            return memberInfo
                .GetAttributes<T>(inherit)
                .SingleOrDefault();
        }

        public static IEnumerable<T> GetAttributes<T>(this MemberInfo memberInfo, bool inherit = false)
        {
            return memberInfo
                .GetCustomAttributes(typeof(T), inherit)
                .OfType<T>();
        }
    }
}
