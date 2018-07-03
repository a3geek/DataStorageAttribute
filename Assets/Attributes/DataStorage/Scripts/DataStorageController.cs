using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Attributes.DataStorage
{
    using Common;
    using Components;

    /// <summary>
    /// <see cref="DataStorageAttribute"/>によって指定された変数を管理する.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("a3geek/Attributes/Data Storage/Data Storage Controller")]
    public sealed partial class DataStorageController : MonoBehaviour
    {
        #region "Singleton"
        /// <summary>シングルトンインスタンス.</summary>
        public DataStorageController Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<DataStorageController>();
                }

                return instance;
            }
        }

        /// <summary>シングルトンインスタンス.</summary>
        private DataStorageController instance = null;
        #endregion

        /// <summary>メンバー情報を取得する時のバインドフラグ.</summary>
        public const BindingFlags Binding =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
            BindingFlags.DeclaredOnly;

        /// <summary><see cref="Awake"/>時に<see cref="Load"/>するかどうか.</summary>
        public bool LoadOnAwake
        {
            get { return this.loadOnAwake; }
        }
        /// <summary><see cref="OnApplicationQuit"/>時に<see cref="Save"/>するかどうか.</summary>
        public bool SaveOnQuit
        {
            get { return this.saveOnQuit; }
        }
        /// <summary><see cref="Application.isEditor"/>がfalseの時、<see cref="Awake"/>時に<see cref="Initialize"/>するかどうか.</summary>
        public bool InitializeOnAwakeInRuntime
        {
            get { return this.initializeOnAwakeInRuntime; }
        }
        /// <summary><see cref="Application.isEditor"/>がtrueの時、<see cref="Awake"/>時に<see cref="Initialize"/>するかどうか.</summary>
        public bool InitializeOnAwakeInEditor
        {
            get { return this.initializeOnAwakeInEditor; }
        }

        /// <summary>変数が宣言されているクラスが継承している親クラス.</summary>
        /// <remarks><see cref="UnityEngine.Object.FindObjectsOfType{T}"/>を利用するので、<see cref="MonoBehaviour"/>を継承している必要がある.</remarks>
        private readonly Type ParentType = typeof(MonoBehaviour);

        /// <summary><see cref="Awake"/>時に<see cref="Load"/>するかどうか.</summary>
        [Header("Behaviour")]
        [Header("Runtime")]
        [SerializeField]
        private bool loadOnAwake = true;
        /// <summary><see cref="OnApplicationQuit"/>時に<see cref="Save"/>するかどうか.</summary>
        [SerializeField]
        private bool saveOnQuit = true;
        /// <summary><see cref="Application.isEditor"/>がfalseの時、<see cref="Awake"/>時に<see cref="Initialize"/>するかどうか.</summary>
        [SerializeField]
        private bool initializeOnAwakeInRuntime = false;

        /// <summary><see cref="Application.isEditor"/>がtrueの時、<see cref="Awake"/>時に<see cref="Initialize"/>するかどうか.</summary>
        [Header("Editor")]
        [SerializeField]
        private bool initializeOnAwakeInEditor = true;

        /// <summary><see cref="DataSet"/>を管理する.</summary>
        [Space(10f), Header("Operation check")]
        [SerializeField]
        private DataStorage storage = new DataStorage(Binding);

        /// <summary>データを保存する時のイベント</summary>
        /// <remarks>一つもイベントが登録されていない時は、デフォルトの保存機構(<see cref="XmlStorage.Storage"/>)を利用する.</remarks>
        [Space(10f), Header("Extension")]
        [SerializeField]
        private IOEvent saver = new IOEvent();
        /// <summary>データを読み込む時のイベント</summary>
        /// <remarks>一つもイベントが登録されていない時は、デフォルトの保存機構(<see cref="XmlStorage.Storage"/>)を利用する.</remarks>
        [SerializeField]
        private IOEvent loader = new IOEvent();

        /// <summary><see cref="DataSet"/>の生成を行う.</summary>
        private DataCreator creator = new DataCreator(Binding);
        

        void Awake()
        {
            instance = this;

            if(this.saver.GetPersistentEventCount() > 0)
            {
                this.storage.Saver = data => this.saver.Invoke(data);
            }
            if(this.loader.GetPersistentEventCount() > 0)
            {
                this.storage.Loader = data => this.loader.Invoke(data);
            }

            if(Application.isEditor == true && this.initializeOnAwakeInEditor == true)
            {
                this.Initialize();
            }
            else if(Application.isEditor == false && this.initializeOnAwakeInRuntime == true)
            {
                this.Initialize();
            }
            
            if(this.loadOnAwake == true)
            {
                this.Load();
            }
        }

        void OnApplicationQuit()
        {
            if(this.saveOnQuit == true)
            {
                this.Save();
            }
        }
        
        /// <summary>
        /// データを保存する.
        /// </summary>
        public void Save()
        {
            this.storage.Save();
        }

        /// <summary>
        /// データを読み込む.
        /// </summary>
        public void Load()
        {
            this.storage.Load();
        }

        /// <summary>
        /// <see cref="DataStorageAttribute"/>を検索して初期化する.
        /// </summary>
        [ContextMenu("Initialize data")]
        public void Initialize()
        {
            this.storage.Data.Clear();

            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsInheriting(ParentType));
            foreach(var type in types)
            {
                foreach(var data in this.creator.Create(type, this.storage))
                {
                    this.storage.AddDataSet(data);
                }
            }

            this.storage.SortData(data => data.Component.name);

#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(this, "Initialized data");
            EditorUtility.SetDirty(this);
            Undo.IncrementCurrentGroup();
#endif
        }
    }

    /// <summary>
    /// 保存と読み込み用のイベント.
    /// </summary>
    [Serializable]
    public class IOEvent : UnityEvent<List<DataSet>> { }
}
