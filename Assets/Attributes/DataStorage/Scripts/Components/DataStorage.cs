using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace Attributes.DataStorage.Components
{
    using Common;
    using XmlStorage;

    /// <summary>
    /// <see cref="DataSet"/>の集約、管理する.
    /// </summary>
    [Serializable]
    public sealed class DataStorage
    {
        /// <summary><see cref="DataSet"/>群.</summary>
        public List<DataSet> Data
        {
            get { return this.data; }
        }
        /// <summary>データを保存する時のイベント</summary>
        public Action<List<DataSet>> Saver
        {
            private get; set;
        }
        /// <summary>データを読み込む時のイベント</summary>
        public Action<List<DataSet>> Loader
        {
            private get; set;
        }

        /// <summary><see cref="DataSet"/>群.</summary>
        [SerializeField]
        private List<DataSet> data = new List<DataSet>();

        /// <summary>メンバー情報を取得する時のバインドフラグ.</summary>
        private BindingFlags binding = BindingFlags.Default;


        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="binding">メンバー情報を取得する時のバインドフラグ.</param>
        public DataStorage(BindingFlags binding)
        {
            this.binding = binding;
        }

        /// <summary>
        /// 全<see cref="DataSet"/>に対して処理を行う.
        /// </summary>
        /// <param name="action">処理内容.</param>
        public void ForEach(Action<DataSet, DataField> action)
        {
            this.data.ForEach(data =>
            {
                data.Fields.ForEach(field =>
                {
                    action(data, field);
                });
            });
        }

        /// <summary>
        /// データを保存する.
        /// </summary>
        public void Save()
        {
            if(this.Saver != null)
            {
                this.Saver(this.data);
                return;
            }

            this.ForEach((data, field) =>
            {
                var validity = this.CheckValidity(data, ref field, "Save");
                if(validity == false)
                {
                    return;
                }

                var value = field.GetValue(data.Component);
                XmlStorage.Storage.Set(field.FieldType, field.SaveKey, value);
            });

            XmlStorage.Storage.Save();
        }

        /// <summary>
        /// データを読み込む.
        /// </summary>
        public void Load()
        {
            if(this.Loader != null)
            {
                this.Loader(this.data);
                return;
            }

            this.ForEach((data, field) =>
            {
                var validity = this.CheckValidity(data, ref field, "Load");
                if(validity == false)
                {
                    return;
                }

                var value = XmlStorage.Storage.Get(field.FieldType, field.SaveKey, default(object));
                if(value == null)
                {
                    return;
                }

                field.SetValue(data.Component, value);
            });
        }

        /// <summary>
        /// ストレージに<see cref="DataSet"/>を追加する.
        /// </summary>
        /// <param name="dataSet">追加する<see cref="DataSet"/>.</param>
        public void AddDataSet(DataSet dataSet)
        {
            if(dataSet != null)
            {
                this.data.Add(dataSet);
            }
        }

        /// <summary>
        /// ストレージ内の<see cref="DataSet"/>保存リストをソートする.
        /// </summary>
        /// <typeparam name="T">ソート処理の基準とする型.</typeparam>
        /// <param name="selector">ソート処理.</param>
        public void SortData<T>(Func<DataSet, T> selector)
        {
            if(selector == null)
            {
                return;
            }

            this.data = this.data.OrderBy(selector).ToList();
        }

        /// <summary>
        /// <see cref="DataSet"/>と<see cref="DataField"/>に問題がないか確認する.
        /// </summary>
        /// <param name="data"><see cref="DataSet"/>インスタンス.</param>
        /// <param name="field"><see cref="DataField"/>インスタンス.</param>
        /// <param name="errorPrefix">エラーログの接頭語.</param>
        /// <returns>問題ないかどうか.</returns>
        private bool CheckValidity(DataSet data, ref DataField field, string errorPrefix)
        {
            if(string.IsNullOrEmpty(field.VariableName) == true
                || string.IsNullOrEmpty(field.SaveKey) == true
                || data.Component == null)
            {
                Debug.Log("Skip " + errorPrefix + " : " + field.SaveKey, data.Component);
                return false;
            }

            if(field.FieldInfo == null)
            {
                var fieldInfo = data.Component.GetType().GetFieldInfoInParents(field.VariableName, this.binding);
                if(fieldInfo == null)
                {
                    Debug.Log("Skip " + errorPrefix + " :: " + field.VariableName, data.Component);
                    return false;
                }

                field = new DataField(field.VariableName, field.SaveKey, fieldInfo, field.FieldType);
            }

            return true;
        }
    }
}
