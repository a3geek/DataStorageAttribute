using UnityEngine;
using System;
using System.Xml.Serialization;

namespace a3geek.Attributes.DataStorage.Examples
{
    /// <summary>
    /// 継承時動作確認用テストクラス
    /// </summary>
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public class TestClass2 : TestClass
    {
        [Serializable]
        public class UnityTypesTest
        {
            public Vector3 vec3 = new Vector3(1f, 10f, 100f);
            public Vector4 vec4 = new Vector4(1f, 1f, 1f, 1f);
            public Quaternion quaternion = new Quaternion(1f, 1f, 1f, 1f);
            public Matrix4x4 matrix4x4 = new Matrix4x4();
            public Color color = Color.black;
        }

        [Serializable]
        public class IgnoreTest
        {
            public int yes = 1;
            [XmlIgnore]
            public int no = 0;
        }
        
        [Space(10f)]
        [Header("TestClass2's variables")]
        [DataStorage]
        public int intVariable2 = 100;

        [SerializeField, DataStorage]
        private double doubleVariable = 100d;
        [SerializeField, DataStorage]
        private UnityTypesTest unityTypesTest = new UnityTypesTest();
        [SerializeField, DataStorage]
        private IgnoreTest ignoreTest = new IgnoreTest();

        
        public override void DrawGui()
        {
            base.DrawGui();

            GUILayout.Space(20f);

            var type = this.GetType();
            this.Draw(this.GetLabel(type, () => this.intVariable2), ref this.intVariable2);
            this.Draw(this.GetLabel(type, () => this.doubleVariable), ref this.doubleVariable);

            GUILayout.Space(10f);

            var utt = this.unityTypesTest;
            type = utt.GetType();
            this.Draw(this.GetLabel(type, () => utt.vec3), ref utt.vec3);
            this.Draw(this.GetLabel(type, () => utt.vec4), ref utt.vec4);
            this.Draw(this.GetLabel(type, () => utt.quaternion), ref utt.quaternion);
            this.Draw(this.GetLabel(type, () => utt.color), ref utt.color);

            GUILayout.Space(10f);

            var igt = this.ignoreTest;
            type = igt.GetType();
            this.Draw(this.GetLabel(type, () => igt.yes), ref igt.yes);
            this.Draw(this.GetLabel(type, () => igt.no) + "(XmlIgnore)", ref igt.no);
        }
    }
}
