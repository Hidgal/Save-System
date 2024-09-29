using System;
using System.Collections.Generic;
using SaveSystem.Utils;
using UnityEngine;

namespace SaveSystem.Internal.DataList
{
    [Serializable]
    public class SaveSystemDataList : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<DataContainer> _dataList;

        private Dictionary<string, int> _indexesDictionary;

        public SaveSystemDataList()
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
                _indexesDictionary.Add(_dataList[i].Key, i);
            }
        }

        public bool Contains<T>() where T : SaveableData
        {
            return Contains(GetKey<T>());
        }
        public bool Contains(string key)
        {
            return _indexesDictionary.ContainsKey(key);
        }

        public SaveableData GetData<T>() where T : SaveableData
        {
            string key = GetKey<T>();
            
            if (Contains(key))
                return _dataList[_indexesDictionary[key]].Data as T;

            return null;
        }

        public bool Add<T>() where T : SaveableData
        {
            var dataType = typeof(T);
            string key = dataType.GetSeriallizationKey();
            if (Contains(key)) return false;

            var dataInstance = (T)Activator.CreateInstance(dataType);
            _indexesDictionary.Add(key, _dataList.Count);
            _dataList.Add(new(dataInstance));

            return true;
        }

        public void Set<T>(T data) where T : SaveableData
        {
            string key = GetKey<T>();
            if (Contains(key))
            {
                _dataList[_indexesDictionary[key]] = new(data);
            }
            else
            {
                Add<T>();
            }
        }

        public string GetKey<T>() where T : SaveableData
        {
            return typeof(T).GetSeriallizationKey();
        }
    }
}

