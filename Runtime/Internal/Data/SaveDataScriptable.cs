using UnityEngine;

namespace SaveSystem
{
    [CreateAssetMenu(menuName = "Save System/Save Data Scriptable", fileName = "Save Data Scriptable")]
    public class SaveDataScriptable : ScriptableObject
    {
        [SerializeField]
        private SaveData _saveData;
        
        public SaveData Data => _saveData;

        public void SaveData(SaveData data)
        {
            _saveData = data;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this); 
#endif
        }
    } 
}
