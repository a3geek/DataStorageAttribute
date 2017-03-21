using UnityEngine;
using System;
using System.Linq.Expressions;

namespace a3geek.Attributes.DataStorage.Examples
{
    using Common;

    /// <summary>
    /// OnGUI用の描画関数格納クラス.
    /// </summary>
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public class GuiDrawer : MonoBehaviour
    {
        /// <summary>
        /// <see cref="bool"/>型を描画する.
        /// </summary>
        /// <param name="label">ラベル.</param>
        /// <param name="value">現在値.</param>
        /// <returns>GUIからの入力値.</returns>
        protected bool Draw(string label, ref bool value)
        {
            using(var horizon = new GUILayout.HorizontalScope())
            {
                GUILayout.Label(label);
                return (value = GUILayout.Toggle(value, ""));
            }
        }

        /// <summary>
        /// <see cref="Vector3"/>型を描画する.
        /// </summary>
        /// <param name="label">ラベル.</param>
        /// <param name="vec3">現在値.</param>
        /// <returns>GUIからの入力値.</returns>
        protected Vector3 Draw(string label, ref Vector3 vec3)
        {
            GUILayout.Label(label);
            using(var horizon = new GUILayout.HorizontalScope())
            {
                vec3.x = this.TextField(ref vec3.x);
                vec3.y = this.TextField(ref vec3.y);
                vec3.z = this.TextField(ref vec3.z);

                return vec3;
            }
        }

        /// <summary>
        /// <see cref="Vector4"/>型を描画する.
        /// </summary>
        /// <param name="label">ラベル.</param>
        /// <param name="vec4">現在値.</param>
        /// <returns>GUIからの入力値.</returns>
        protected Vector4 Draw(string label, ref Vector4 vec4)
        {
            GUILayout.Label(label);
            using(var horizon = new GUILayout.HorizontalScope())
            {
                vec4.x = this.TextField(ref vec4.x);
                vec4.y = this.TextField(ref vec4.y);
                vec4.z = this.TextField(ref vec4.z);
                vec4.w = this.TextField(ref vec4.w);

                return vec4;
            }
        }

        /// <summary>
        /// <see cref="Quaternion"/>型を描画する.
        /// </summary>
        /// <param name="label">ラベル.</param>
        /// <param name="qua">現在値.</param>
        /// <returns>GUIからの入力値.</returns>
        protected Quaternion Draw(string label, ref Quaternion qua)
        {
            GUILayout.Label(label);
            using(var horizon = new GUILayout.HorizontalScope())
            {
                qua.x = this.TextField(ref qua.x);
                qua.y = this.TextField(ref qua.y);
                qua.z = this.TextField(ref qua.z);
                qua.w = this.TextField(ref qua.w);
            }

            GUILayout.Label(label + ".eulerAngles");
            using(var horizon = new GUILayout.HorizontalScope())
            {
                var euler = qua.eulerAngles;
                euler.x = this.TextField(ref euler.x);
                euler.y = this.TextField(ref euler.y);
                euler.z = this.TextField(ref euler.z);

                qua.eulerAngles = euler;
            }

            return qua;
        }

        /// <summary>
        /// <see cref="Color"/>型を描画する.
        /// </summary>
        /// <param name="label">ラベル.</param>
        /// <param name="col">現在値.</param>
        /// <returns>GUIからの入力値.</returns>
        protected Color Draw(string label, ref Color col)
        {
            GUILayout.Label(label);
            using(var horizon = new GUILayout.HorizontalScope())
            {
                col.r = this.TextField(ref col.r);
                col.g = this.TextField(ref col.g);
                col.b = this.TextField(ref col.b);
                col.a = this.TextField(ref col.a);
            }
            
            return col;
        }

        /// <summary>
        /// 任意の型を描画する.
        /// </summary>
        /// <typeparam name="T">描画する型.</typeparam>
        /// <param name="label">ラベル.</param>
        /// <param name="value">現在値.</param>
        /// <returns>GUIからの入力値.</returns>
        protected T Draw<T>(string label, ref T value)
        {
            using(var horizon = new GUILayout.HorizontalScope())
            {
                GUILayout.Label(label);
                return this.TextField(ref value);
            }
        }

        /// <summary>
        /// 任意の型をテキストフィールドに描画する.
        /// </summary>
        /// <typeparam name="T">描画する型.</typeparam>
        /// <param name="value">現在値.</param>
        /// <returns>GUIからの入力値.</returns>
        protected T TextField<T>(ref T value)
        {
            try
            {
                var str = GUILayout.TextField(value.ToString());
                value = (T)Convert.ChangeType(str, typeof(T));
            }
            catch (Exception e)
            {
                Debug.LogError(e, gameObject);
            }

            return value;
        }

        /// <summary>
        /// GUI用のラベルを取得する.
        /// </summary>
        /// <typeparam name="T">描画する値.</typeparam>
        /// <param name="type">描画する型.</param>
        /// <param name="func">描画する値を取得するデリゲート.</param>
        /// <returns>ラベル.</returns>
        protected string GetLabel<T>(Type type, Expression<Func<T>> func)
        {
            return ObjectExtender.GetMyName(type, func);
        }
    }
}
