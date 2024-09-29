using System;
using UnityEngine;
using SaveSystem.Internal.DataList;

namespace SaveSystem
{
    [Serializable]
    public class SaveData
    {
        [SerializeField]
        private SaveSystemDataList _dataList = new();

        private Action _saveAction;
        private Action _clearAction;

        /// <typeparam name="T">Type of saveable class</typeparam>
        /// <returns>True if has data in save</returns>
        public bool HasData<T>() where T : SaveableData
        {
            return _dataList.Contains<T>();
        }
        /// <summary>
        /// Returns instance of required type. 
        /// If there`s no instance in save - creates new and immediately saves if autoSave is true.
        /// </summary>
        /// <typeparam name="T">Type of saveable class</typeparam>
        /// <returns>Instance of required type</returns>
        public T GetData<T>() where T : SaveableData
        {
            if (!_dataList.Contains<T>())
            {
                _dataList.Add<T>();
            }

            return _dataList.GetData<T>() as T;
        }
        /// <summary>
        /// If autoSave is true - immediately saves data to file
        /// </summary>
        /// <typeparam name="T">Type of saveable class</typeparam>
        /// <param name="autoSave">If true - immediately saves data to file</param>
        public void SetData<T>(T data, bool autoSave = true) where T : SaveableData
        {
            if (data == null)
            {
                Debug.LogError("Saving failed: Can`t save null data!");
                return;
            }

            _dataList.Set(data);

            if (autoSave)
                Save();
        }

        /// <summary>
        /// Saves all data to file.
        /// </summary>
        public void Save()
        {
            _saveAction?.Invoke();
        }

        /// <summary>
        /// Clear all data from file
        /// </summary>
        public void Clear()
        {
            _clearAction?.Invoke();
        }

        internal void Initialize(Action saveAction, Action clearAction)
        {
            _saveAction = saveAction;
            _clearAction = clearAction;
        }
    }  
}
