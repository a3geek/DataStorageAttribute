using System;

namespace a3geek.Attributes
{
    /// <summary>
    /// 保存対象として指定するアトリビュート.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DataStorageAttribute : Attribute
    {
        /// <summary>保存キー.</summary>
        public string Key { get; private set; }


        /// <summary>
        /// コンストラクタ.
        /// </summary>
        public DataStorageAttribute() : this("") {; }

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="key">保存用のキー.</param>
        public DataStorageAttribute(string key)
        {
            this.Key = key ?? "";
        }
    }
}
