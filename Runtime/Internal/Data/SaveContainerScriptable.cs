using UnityEngine;

namespace SaveSystem.Internal.Data
{
    [CreateAssetMenu(menuName = "Save System/Save Container Scriptable", fileName = "Save Container Scriptable")]
    public class SaveContainerScriptable : ScriptableObject
    {
        [SerializeField]
        private SaveContainer _saveData;
        
        public SaveContainer Data
        {
            get => _saveData;
            set
            {
                _saveData = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
#endif
            }
        }
    } 
}
