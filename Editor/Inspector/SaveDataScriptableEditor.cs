using UnityEngine;
using UnityEditor;
using SaveSystem.Editor.Utils;

namespace SaveSystem.Editor.Inspector
{
    [CustomEditor(typeof(SaveDataScriptable))]
    public class SaveDataScriptableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("Load from JSON"))
            {
                JsonToScriptableParser.ImportDataFromJson(target as SaveDataScriptable);
            }

            if (GUILayout.Button("Save to JSON"))
            {
                JsonToScriptableParser.ExportDataToJson(target as SaveDataScriptable);
            }

            base.OnInspectorGUI();
        }
    }
}
