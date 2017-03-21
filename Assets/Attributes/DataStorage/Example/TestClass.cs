using UnityEngine;
using System;

namespace a3geek.Attributes.DataStorage.Examples
{
    using Common;

    /// <summary>
    /// 動作確認用テストクラス
    /// </summary>
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public class TestClass : GuiDrawer
    {
        [Serializable]
        public class Test
        {
            public int i = 10;
            public string str = "str";
            public bool b = false;
        }

        [Header("Debug")]
        public bool drawGui = true;
        public int windowId = 0;
        public Rect window = new Rect(20f, 20f, 300f, 200f);

        [Space(10f), Header("TestClass's variables")]
        [DataStorage]
        public int intVariable = 1;
        [DataStorage]
        public float floatVariable = 0f;
        [DataStorage]
        public Test test = new Test();

        #pragma warning disable 414
        [SerializeField, DataStorage]
        private string str = "str";


        protected void OnGUI()
        {
            if(this.drawGui == false)
            {
                return;
            }

            this.window = GUILayout.Window(this.windowId, this.window, i =>
            {
                this.DrawGui();
                GUI.DragWindow();
            }, gameObject.GetFullName("/") + "." + this.GetType().Name);
        }

        public virtual void DrawGui()
        {
            this.Draw(this.GetLabel(this.test.GetType(), () => this.test.i), ref this.test.i);
            this.Draw(this.GetLabel(this.test.GetType(), () => this.test.str), ref this.test.str);
            this.Draw(this.GetLabel(this.test.GetType(), () => this.test.b), ref this.test.b);

            GUILayout.Space(10f);

            this.Draw(this.GetLabel(this.GetType(), () => this.intVariable), ref this.intVariable);
            this.Draw(this.GetLabel(this.GetType(), () => this.floatVariable), ref this.floatVariable);
            this.Draw(this.GetLabel(this.GetType(), () => this.str), ref this.str);
        }
    }
}
