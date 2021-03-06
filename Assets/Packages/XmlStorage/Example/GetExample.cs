﻿using UnityEngine;
using System.Linq;

namespace XmlStorage.Examples
{
    /// <summary>
    /// 値のゲットのテスト
    /// </summary>
    [AddComponentMenu("")]
    public class GetExample : MonoBehaviour
    {
        /// <summary>
        /// 値をゲットしてログに出力する
        /// </summary>
        public void Execute()
        {
            /*
             * 1
             * 1.111
             * XmlStorage.Examples.ExampleController+Test
             * "TestString"
             * 0.1f
             * 10.1f
             * 
             * 0
             * ""
             * 
             * 0
             * "del_tes2"
             * 
             * "lab-interactive@team-lab.com"
             * 
             * (0.1, 0.2)
             * (1.0, 2.0, 3.0)
             * (10.0, 20.0, 30.0)
             */
            Debug.Log("Default Aggregatpion");
            Debug.Log("Directory Path : " + Storage.DirectoryPath);
            Debug.Log("File Name : " + Storage.FileName);
            this.GetDataFromXmlStorage();

            if(Application.isEditor)
            {
                /*
                 * 11
                 * 1.111
                 * XmlStorage.Examples.ExampleController+Test
                 * "TestString"
                 * 0.1f
                 * 10.1f
                 * 
                 * 0
                 * ""
                 * 
                 * 0
                 * "del_tes2"
                 * 
                 * "lab-interactive@team-lab.com"
                 * 
                 * (0.1, 0.2)
                 * (1.0, 2.0, 3.0)
                 * (10.0, 20.0, 30.0)
                 */
                Debug.Log("Test1 Aggregation");
                Storage.ChangeAggregation("Test1");
                Debug.Log("Directory Path : " + Storage.DirectoryPath);
                Debug.Log("File Name : " + Storage.FileName);
                this.GetDataFromXmlStorage();
            }

            /*
             * 111
             * 1.111
             * XmlStorage.Examples.ExampleController+Test
             * "TestString"
             * 0.1f
             * 10.1f
             * 
             * 0
             * ""
             * 
             * 0
             * "del_tes2"
             * 
             * "lab-interactive@team-lab.com"
             * 
             * (0.1, 0.2)
             * (1.0, 2.0, 3.0)
             * (10.0, 20.0, 30.0)
             */
            Debug.Log("Test2 Aggregation");
            Storage.ChangeAggregation("Test2");
            Debug.Log("Directory Path : " + Storage.DirectoryPath);
            Debug.Log("File Name : " + Storage.FileName);
            this.GetDataFromXmlStorage();
        }

        /// <summary>
        /// <see cref="Storage"/>から値を取得してログに出力する
        /// </summary>
        private void GetDataFromXmlStorage()
        {
            Debug.Log(Storage.GetInt("integer", 0));
            Debug.Log(Storage.GetFloat("float", 0f));

            Debug.Log(Storage.Get(typeof(object), "obj_int", 0));
            Debug.Log(Storage.Get<object>("obj_int"));
            Debug.Log(Storage.Get(typeof(int), "obj_int", 0));
            Debug.Log(Storage.Get<int>("obj_int"));

            Debug.Log("");

            Debug.Log(Storage.Get<ExampleController.Test1>("Test1Class"));
            Debug.Log(Storage.Get<ExampleController.Test1>("Test1Class").str);
            Debug.Log(Storage.Get<ExampleController.Test1>("Test1Class").list1.First());
            Debug.Log(Storage.Get<ExampleController.Test1>("Test1Class").list1.Last());

            Debug.Log(Storage.Get<ExampleController.Test2>("Test2Class", null));
            Debug.Log(Storage.Get<ExampleController.Test2>("Test2Class", null).vec2);
            Debug.Log(Storage.Get<ExampleController.Test2>("Test2Class", null).vec3);

            Debug.Log("");

            Debug.Log(Storage.GetInt("del_tes1"));
            Debug.Log(Storage.GetString("del_tes1"));

            Debug.Log("");

            Debug.Log(Storage.GetInt("del_tes2"));
            Debug.Log(Storage.GetString("del_tes2"));

            Debug.Log("");

            Debug.Log(Storage.Get<string>("address"));

            Debug.Log("");

            Debug.Log(Storage.Get("vec2", Vector2.zero));
            Debug.Log(Storage.Get("vec3", Vector3.zero));
            Debug.Log(Storage.Get("qua", Quaternion.identity).eulerAngles);

            Debug.Log("");
            Debug.Log("");
        }
    }
}
