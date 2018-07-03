using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Attributes.DataStorage.Components
{
    using Common;

    /// <summary>
    /// 一つの型(クラス)に属する<see cref="DataField"/>(変数情報)を集約、管理する.
    /// </summary>
    [Serializable]
    public sealed class DataSet
    {
        /// <summary><see cref="Fields"/>が属しているインスタンス.</summary>
        public Component Component
        {
            get { return this.component; }
        }
        /// <summary><see cref="DataField"/>(変数情報)群.</summary>
        public List<DataField> Fields
        {
            get { return this.fields; }
        }

        /// <summary>クラス名 (Inspector用).</summary>
#pragma warning disable 0414
        [SerializeField, HideInInspector]
        private string key = "";

        /// <summary><see cref="Fields"/>が属しているインスタンス.</summary>
        [SerializeField]
        private Component component = null;
        /// <summary><see cref="DataField"/>(変数情報)群.</summary>
        [SerializeField]
        private List<DataField> fields = new List<DataField>();


        /// <summary>
        /// コンストラクタ.
        /// </summary>
        public DataSet() : this(null) {; }

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="component">管理する型のインスタンス.</param>
        public DataSet(Component component)
        {
            this.fields = new List<DataField>();
            
            if(component == null)
            {
                return;
            }

            this.key = component.GetType().FullName;
            this.component = component;
        }

        /// <summary>
        /// 新しい変数情報を管理対象に加える.
        /// </summary>
        /// <param name="variableName">変数名.</param>
        /// <param name="saveKey">保存キー.</param>
        /// <param name="fieldInfo">変数の<see cref="FieldInfo"/>.</param>
        /// <returns>管理のために生成された<see cref="DataField"/>.</returns>
        public DataField AddField(string variableName, string saveKey, FieldInfo fieldInfo)
        {
            var first = this.fields.FirstOrDefault(d => d.VariableName == variableName);

            if(first != null)
            {
                return first;
            }

            var dataField = new DataField(variableName, saveKey, fieldInfo);
            this.fields.Add(dataField);

            return dataField;
        }

        /// <summary>
        /// 保存用のフルネームを取得する.
        /// </summary>
        /// <param name="conjunctionOfName">接続文字.</param>
        /// <returns>フルネーム.</returns>
        public string GetFullName(string conjunctionOfName)
        {
            if(this.component == null)
            {
                return "";
            }

            var prefix = Application.companyName + ":" + Application.productName + ":" + SceneManager.GetActiveScene().name;
            var name = this.component.gameObject.GetFullName("/");

            return prefix + conjunctionOfName + name;
        }

        /// <summary>
        /// <see cref="DataField"/>用の保存キーを生成する.
        /// </summary>
        /// <param name="conjunctionOfName">接続文字.</param>
        /// <param name="fieldInfo">変数の<see cref="FieldInfo"/>.</param>
        /// <returns>保存キー.</returns>
        public string GenerateSaveKey(string conjunctionOfName, FieldInfo fieldInfo)
        {
            return this.GetFullName(conjunctionOfName)
                + conjunctionOfName
                + this.component.GetType().FullName
                + "." + fieldInfo.Name;
        }
    }
}
