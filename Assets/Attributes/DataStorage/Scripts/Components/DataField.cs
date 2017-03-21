using UnityEngine;
using System;
using System.Reflection;

namespace a3geek.Attributes.DataStorage.Components
{
    using Common;

    /// <summary>
    /// 一つのメンバー情報(変数情報)を管理する.
    /// </summary>
    [Serializable]
    public sealed class DataField
    {
        /// <summary>変数名.</summary>
        public string VariableName
        {
            get { return this.key; }
            private set { this.key = value; }
        }

        /// <summary>変数の型のフルネーム.</summary>
        public string FieldTypeName
        {
            get { return this.fieldTypeName; }
        }
        /// <summary>保存キー.</summary>
        public string SaveKey
        {
            get { return this.saveKey; }
        }
        /// <summary><see cref="FieldInfo"/>インスタンス.</summary>
        public FieldInfo FieldInfo
        {
            get { return this.fieldInfo; }
        }
        /// <summary>変数の型</summary>
        public Type FieldType
        {
            get { return this.fieldType ?? (this.fieldType = this.fieldTypeName.GetTypeFromString()); }
        }

        /// <summary>変数名 (Inspector用).</summary>
        [SerializeField, HideInInspector]
        private string key = "";
        /// <summary>変数の型のフルネーム.</summary>
        [SerializeField, HideInInspector]
        private string fieldTypeName = "";

        /// <summary>保存キー.</summary>
        [SerializeField]
        private string saveKey = "";

        /// <summary><see cref="FieldInfo"/>インスタンス.</summary>
        private FieldInfo fieldInfo = null;
        /// <summary>変数の型.</summary>
        private Type fieldType = null;


        /// <summary>
        /// コンストラクタ.
        /// </summary>
        public DataField() : this("", "") {; }

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="variableName">変数名.</param>
        /// <param name="saveKey">保存キー.</param>
        /// <param name="fieldInfo"><see cref="FieldInfo"/>インスタンス.</param>
        public DataField(string variableName, string saveKey, FieldInfo fieldInfo = null)
        {
            this.VariableName = variableName;
            this.saveKey = saveKey;

            this.fieldInfo = fieldInfo;
            this.fieldTypeName = (fieldInfo == null ? "" : fieldInfo.FieldType.FullName);
            this.fieldType = this.fieldTypeName.GetTypeFromString();
        }

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="variableName">変数名.</param>
        /// <param name="saveKey">保存キー.</param>
        /// <param name="fieldInfo"><see cref="FieldInfo"/>インスタンス.</param>
        /// <param name="fieldType">変数の型</param>
        public DataField(string variableName, string saveKey, FieldInfo fieldInfo, Type fieldType)
        {
            this.VariableName = variableName;
            this.saveKey = saveKey;

            this.fieldInfo = fieldInfo;
            this.fieldTypeName = (fieldType == null ? "" : fieldType.FullName);
            this.fieldType = fieldType;
        }

        /// <summary>
        /// 変数の現在の値を取得する.
        /// </summary>
        /// <param name="component">取得するインスタンス.</param>
        /// <returns>値.</returns>
        public object GetValue(Component component)
        {
            if(component == null || this.FieldInfo == null)
            {
                return null;
            }

            return this.FieldInfo.GetValue(component);
        }

        /// <summary>
        /// 変数に値をセットする.
        /// </summary>
        /// <param name="component">セットするインスタンス.</param>
        /// <param name="value">値.</param>
        /// <returns>成功したかどうか.</returns>
        public bool SetValue(Component component, object value)
        {
            if(component == null || this.FieldInfo == null)
            {
                return false;
            }

            try
            {
                this.FieldInfo.SetValue(component, value);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
