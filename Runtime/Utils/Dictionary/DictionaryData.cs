using System;
using UnityEngine;

namespace SaveSystem.Utils
{
    [System.Serializable]
    public class DictionaryData<K, V>
    {
#if UNITY_EDITOR
        public object BoxedValue => _value;
        public Type ValueType => typeof(V);
#endif
        public K Key => _key;
        public V Value => _value;

        [SerializeField]
        private K _key;
        [SerializeField]
        private V _value;

        public DictionaryData(K key, V value)
        {
            _key = key;
            _value = value;
        }
    }
}
