using UnityEngine;

namespace SaveSystem.Internal.Data
{
    [CreateAssetMenu(menuName = "Save System/Save Container Scriptable", fileName = "Save Container Scriptable")]
    public class SaveContainerScriptable : ScriptableObject
    {
        [SerializeField]
        private SaveContainer _saveData;
        
        public SaveContainer Data => _saveData;

        public void SaveData(SaveContainer data)
        {
            _saveData = data;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this); 
#endif
        }
    } 
}
