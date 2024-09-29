using System;
using UnityEngine;
using System.Collections.Generic;

namespace SaveSystem.Internal.Data
{
    [Serializable]
    public class SaveContainer : ISave, ISerializationCallbackReceiver
    {
        [SerializeReference]
        private List<SaveData> _dataList;

        private Action _saveAction;
        private Action _clearAction;
        private Dictionary<Type, int> _indexesDictionary;

        public List<SaveData> DataList => _dataList;
        public IReadOnlyDictionary<Type, int> IndexesDictionary => _indexesDictionary;

        public SaveContainer()
        {
            _dataList = new();
            _indexesDictionary = new();
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            _indexesDictionary.Clear();

            for (int i = 0; i < _dataList.Count; i++)
            {
                if (_dataList[i] == null) continue;
                _indexesDictionary.Add(_dataList[i].GetType(), i);
            }
        }

        /// <typeparam name="T">Type of saveable class</typeparam>
        /// <returns>True if has data in save</returns>
        public bool Contains<T>() where T : SaveData
        {
            return Contains(typeof(T));
        }
        public bool Contains(Type dataType)
        {
            return _indexesDictionary.ContainsKey(dataType);
        }

        /// <summary>
        /// Returns instance of required type. 
        /// If there`s no instance in save - creates new and immediately saves if autoSave is true.
        /// </summary>
        /// <typeparam name="T">Type of saveable class</typeparam>
        /// <returns>Instance of required type</returns>
        public T Get<T>() where T : SaveData
        {
            var type = typeof(T);
            return Get(type) as T;
        }
        public SaveData Get(Type dataType)
        {
            if (_indexesDictionary.TryGetValue(dataType, out var index))
            {
                return _dataList[index];
            }
            else
            {
                Add(dataType);
                return _dataList[^1];
            }
        }

        /// <summary>
        /// If autoSave is true - immediately saves data to file
        /// </summary>
        /// <typeparam name="T">Type of saveable class</typeparam>
        /// <param name="autoSave">If true - immediately saves data to file</param>
        public void Set<T>(T data, bool autoSave = true) where T : SaveData
        {
            var type = typeof(T);
            Set(data, type, autoSave);
        }
        public void Set(SaveData data, Type dataType, bool autoSave = true)
        {
            if (data == null)
            {
                Debug.LogError("Saving failed: Can`t save null data!");
                return;
            }

            if (_indexesDictionary.TryGetValue(dataType, out var index))
            {
                _dataList[index] = data;
            }
            else
            {
                _indexesDictionary.Add(dataType, _dataList.Count);
                _dataList.Add(data);
            }

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

        public bool Add<T>() where T : SaveData
        {
            var type = typeof(T);
            return Add(type);
        }
        public bool Add(Type type)
        {
            if (Contains(type)) return false;

            var dataInstance = Activator.CreateInstance(type) as SaveData;
            _indexesDictionary.Add(type, _dataList.Count);
            _dataList.Add(dataInstance);

            return true;
        }

        internal void Initialize(Action saveAction, Action clearAction)
        {
            _saveAction = saveAction;
            _clearAction = clearAction;
        }
    }
}
