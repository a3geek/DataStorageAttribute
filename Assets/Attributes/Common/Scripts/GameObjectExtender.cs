using UnityEngine;

namespace Attributes.Common
{
    public static class GameObjectExtender
    {
        public static string GetFullName(this GameObject obj, string conjunctionOfName = "/")
        {
            var parents = obj.GetComponentsInParent<Transform>();

            string name = null;
            for(var i = 0; i < parents.Length; i++)
            {
                name = parents[i].gameObject.name + (name == null ? "" : conjunctionOfName + name);
            }

            return name ?? "";
        }
    }
}
