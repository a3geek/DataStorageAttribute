using System.Collections.Generic;
using System;
using System.Reflection;

namespace Attributes.Common
{
    public static class TypeExtender
    {
        public static bool IsInheriting(this Type self, Type baseType)
        {
            for(var bt = self.BaseType; bt != null; bt = bt.BaseType)
            {
                if(bt == baseType)
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<FieldInfo> GetFieldInfos(this Type self, BindingFlags binding)
        {
            if(self == null)
            {
                yield break;
            }

            var fields = self.GetFields(binding);
            for(var i = 0; i < fields.Length; i++)
            {
                var attribute = fields[i].GetAttribute<DataStorageAttribute>(false);

                if(attribute != null)
                {
                    yield return fields[i];
                }
            }
        }

        public static FieldInfo GetFieldInfoInParents(this Type self, string key, BindingFlags binding)
        {
            if(self == null || string.IsNullOrEmpty(key) == true)
            {
                return null;
            }

            for(var bt = self; bt != null; bt = bt.BaseType)
            {
                var fieldInfo = bt.GetField(key, binding);

                if(fieldInfo != null)
                {
                    return fieldInfo;
                }
            }

            return null;
        }

        // http://answers.unity3d.com/questions/206665/typegettypestring-does-not-work-in-unity.html
        /// <summary>
        /// 型名からType型に変換する
        /// </summary>
        /// <param name="typeName">型名</param>
        /// <returns>変換した型</returns>
        public static Type GetTypeFromString(this string typeName)
        {
            if(string.IsNullOrEmpty(typeName))
            {
                return null;
            }

            var type = Type.GetType(typeName);
            if(type != null)
            {
                return type;
            }

            if(typeName.Contains("."))
            {
                var assembly = Assembly.Load(typeName.Substring(0, typeName.IndexOf('.')));
                if(assembly == null)
                {
                    return null;
                }

                type = assembly.GetType(typeName);
                if(type != null)
                {
                    return type;
                }
            }
            
            foreach(var assembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                var assem = Assembly.Load(assembly);

                if(assem != null)
                {
                    type = assem.GetType(typeName);

                    if(type != null)
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }
}
