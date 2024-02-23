using UnityEngine;

namespace SaveSystem.Internal.Scriptables
{
    internal abstract class SaveSystemScriptableGeneric<T> : ScriptableObject where T : SaveData
    {
        public T Data => _saveData;

        [SerializeField]
        private T _saveData;

        internal virtual void SaveData(T data)
        {
            _saveData = data;
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
}

