using UnityEngine;
using UnityEditor;

namespace Attributes.DataStorage.Editors
{
    [CustomEditor(typeof(DataStorageController))]
    public class DataStorageControllerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var controller = (DataStorageController)target;

            GUILayout.Space(20f);
            if(GUILayout.Button("Initialize Data"))
            {
                controller.Initialize();
            }
        }
    }
}
