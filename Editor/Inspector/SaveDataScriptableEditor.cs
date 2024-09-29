using UnityEngine;
using UnityEditor;
using SaveSystem.Editor.Utils;
using SaveSystem.Internal.Data;

namespace SaveSystem.Editor.Inspector
{
    [CustomEditor(typeof(SaveContainerScriptable))]
    public class SaveDataScriptableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("Load from JSON"))
            {
                JsonToScriptableParser.ImportDataFromJson(target as SaveContainerScriptable);
            }

            if (GUILayout.Button("Save to JSON"))
            {
                JsonToScriptableParser.ExportDataToJson(target as SaveContainerScriptable);
            }

            base.OnInspectorGUI();
        }
    }
}
