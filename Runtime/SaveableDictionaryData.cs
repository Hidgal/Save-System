using System.Collections.Generic;
using SaveSystem.Utils;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// A special case of SaveableData.
    /// Saveable shell for SerializableDictionary type
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [System.Serializable]
    public class SaveableDictionaryData<TKey, TValue> : SaveableData
    {
        public TValue this[TKey key] => _dictionary[key];

        public IReadOnlyDictionary<TKey, TValue> Dictionary => _dictionary;

        [SerializeField]
        private SerializableDictionary<TKey, TValue> _dictionary;

        public SaveableDictionaryData()
        {
            _dictionary = new();
        }

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        /// <summary>
        /// Adds key if doesn`t contain it already. If autoSave is <see langword="true"/> then saves changes to file.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="autoSave"></param>
        /// <returns><see langword="true"/> if data was added</returns>
        public bool Add(TKey key, TValue value, bool autoSave = false)
        {
            if (key == null) return false;
            
            if(_dictionary.TryAdd(key, value))
            {
                if (autoSave)
                    Save();

                return true;
            }

            return true;
        }

        /// <summary>
        /// Removes data for provided key if contains it already. If autoSave is <see langword="true"/> then saves changes to file.
        /// </summary>
        /// <returns><see langword="true"/> if data was removed</returns>
        public bool Remove(TKey key, bool autoSave = false)
        {
            if (key == null) return false;

            var result = _dictionary.Remove(key);

            if (result && autoSave)
                Save();

            return result;
        }
        /// <summary>
        /// Removes data for provided key if contains it already. If autoSave is <see langword="true"/> then saves changes to file.
        /// </summary>
        /// <returns><see langword="true"/> if data was removed</returns>
        public bool Remove(TKey key, out TValue value, bool autoSave = false)
        {
            value = default;
            if (key == null) return false;

            var result = _dictionary.Remove(key, out value);

            if (result && autoSave)
                Save();

            return result;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;
            if (key == null) return default;

            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Clears dictionary. If autoSave is true then saves changes to file.
        /// </summary>
        /// <param name="autoSave"></param>
        public void Clear(bool autoSave = false)
        {
            _dictionary.Clear();

            if (autoSave)
                Save();
        }
    }
}
