using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace Attributes.DataStorage.Components
{
    using Common;

    /// <summary>
    /// 型情報を元に<see cref="DataSet"/>の生成を行う.
    /// </summary>
    public sealed class DataCreator
    {
        /// <summary>保存キー生成時の連結文字.</summary>
        public const string ConjunctionOfName = "->";

        /// <summary>メンバー情報を取得する時のバインドフラグ.</summary>
        private BindingFlags binding = BindingFlags.Default;


        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="binding">メンバー情報を取得する時のバインドフラグ.</param>
        public DataCreator(BindingFlags binding)
        {
            this.binding = binding;
        }

        /// <summary>
        /// <paramref name="type"/>からメンバー情報を取得して<see cref="DataSet"/>を生成する.
        /// </summary>
        /// <param name="type">メンバー情報を取得する型.</param>
        /// <param name="storage">既に<see cref="DataSet"/>が存在する場合は重複判定、更新処理を行う.</param>
        /// <returns><see cref="DataSet"/>群.</returns>
        public IEnumerable<DataSet> Create(Type type, DataStorage storage = null)
        {
            var fields = this.GetFieldInfos(type);
            if(fields.Count() <= 0)
            {
                yield break;
            }

            var components = new List<Component>(UnityEngine.Object.FindObjectsOfType(type).Select(c => (Component)c));
            foreach(var com in components)
            {
                var data = this.CreateDataSet(storage, com, fields);

                if(data == null)
                {
                    continue;
                }

                yield return data;
            }
        }

        /// <summary>
        /// <see cref="DataSet"/>を生成する.
        /// </summary>
        /// <param name="storage">既存の<see cref="DataSet"/>群.</param>
        /// <param name="com"><paramref name="fields"/>のインスタンス.</param>
        /// <param name="fields">フィールド情報.</param>
        /// <returns>生成した<see cref="DataSet"/>.</returns>
        private DataSet CreateDataSet(DataStorage storage, Component com, IEnumerable<FieldInfo> fields)
        {
            // 継承元のメンバ変数は登録されない.
            // 同一のコンポーネントが存在する場合は更新だけを行う.
            var first = storage == null ? null : storage.Data.FirstOrDefault(d => d.Component == com);
            var data = first ?? new DataSet(com);

            foreach(var field in fields)
            {
                var attri = field.GetAttribute<DataStorageAttribute>(false);

                var variableName = field.Name;
                var saveKey = string.IsNullOrEmpty(attri.Key) == true
                    ? data.GenerateSaveKey(ConjunctionOfName, field)
                    : attri.Key;

                data.AddField(variableName, saveKey, field);
            }

            // first != nullの時、既に登録されている情報を更新しただけなのでnullを返す.
            return first != null ? null : data;
        }

        /// <summary>
        /// <see cref="binding"/>フラグで<paramref name="type"/>からフィールド情報を取得する.
        /// </summary>
        /// <param name="type">取得する型.</param>
        /// <returns>フィールド情報.</returns>
        private IEnumerable<FieldInfo> GetFieldInfos(Type type)
        {
            if(type == null)
            {
                yield break;
            }

            var fields = type.GetFields(this.binding);
            for(var i = 0; i < fields.Length; i++)
            {
                var attribute = fields[i].GetAttribute<DataStorageAttribute>(false);

                if(attribute != null)
                {
                    yield return fields[i];
                }
            }
        }
    }
}
