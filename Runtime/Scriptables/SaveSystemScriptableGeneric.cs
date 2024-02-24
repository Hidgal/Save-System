using UnityEngine;

namespace SaveSystem.Scriptables
{
    public abstract class SaveSystemScriptableGeneric<T> : ScriptableObject where T : SaveData
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

