using System;
using UnityEngine;

namespace SaveSystem.Dictionary
{
    [System.Serializable]
    public class DictionaryData<K, V>
    {
        [SerializeField]
        private K _key;
        [SerializeField]
        private V _value;

        public K Key => _key;
        public V Value => _value;

#if UNITY_EDITOR
        public object BoxedValue => _value;
        public Type ValueType => typeof(V);
#endif

        public DictionaryData(K key, V value)
        {
            _key = key;
            _value = value;
        }
    }
}
