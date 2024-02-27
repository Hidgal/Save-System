using System.Collections.Generic;
using System;
using UnityEngine;

namespace SaveSystem.Utils
{
    /// <summary>
    /// Dictionary that can serialize keys and values
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : SerializableDictionary<TKey, TValue, TKey, TValue>
    {
        /// <summary>
        /// Conversion to serialize a key
        /// </summary>
        /// <param name="key">The key to serialize</param>
        /// <returns>The Key that has been serialized</returns>
        public override TKey SerializeKey(TKey key) => key;

        /// <summary>
        /// Conversion to serialize a value
        /// </summary>
        /// <param name="val">The value</param>
        /// <returns>The value</returns>
        public override TValue SerializeValue(TValue val) => val;

        /// <summary>
        /// Conversion to serialize a key
        /// </summary>
        /// <param name="key">The key to serialize</param>
        /// <returns>The Key that has been serialized</returns>
        public override TKey DeserializeKey(TKey key) => key;

        /// <summary>
        /// Conversion to serialize a value
        /// </summary>
        /// <param name="val">The value</param>
        /// <returns>The value</returns>
        public override TValue DeserializeValue(TKey key, TValue val) => val;
    }

    /// <summary>
    /// Dictionary that can serialize keys and values as other types
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <typeparam name="STKey">Type which the key will be serialized for</typeparam>
    /// <typeparam name="STValue">Type which the value will be serialized for</typeparam>
    [Serializable]
    public abstract class SerializableDictionary<TKey, TValue, STKey, STValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<DictionaryData<STKey, STValue>> _datas = new();

        /// <summary>
        /// From <see cref="TKey"/> to <see cref="STKey"/>
        /// </summary>
        /// <param name="key">They key in <see cref="TKey"/></param>
        /// <returns>The key in <see cref="STKey"/></returns>
        public abstract STKey SerializeKey(TKey key);

        /// <summary>
        /// From <see cref="TValue"/> to <see cref="STValue"/>
        /// </summary>
        /// <param name="value">The value in <see cref="TValue"/></param>
        /// <returns>The value in <see cref="STValue"/></returns>
        public abstract STValue SerializeValue(TValue value);


        /// <summary>
        /// From <see cref="STKey"/> to <see cref="TKey"/>
        /// </summary>
        /// <param name="serializedKey">They key in <see cref="STKey"/></param>
        /// <returns>The key in <see cref="TKey"/></returns>
        public abstract TKey DeserializeKey(STKey serializedKey);

        /// <summary>
        /// From <see cref="STValue"/> to <see cref="TValue"/>
        /// </summary>
        /// <param name="serializedValue">The value in <see cref="STValue"/></param>
        /// <returns>The value in <see cref="TValue"/></returns>
        public abstract TValue DeserializeValue(TKey deserializedKey, STValue serializedValue);

        /// <summary>
        /// OnBeforeSerialize implementation.
        /// </summary>
        public void OnBeforeSerialize()
        {
            _datas.Clear();

            foreach (var kvp in this)
            {
                _datas.Add(new(SerializeKey(kvp.Key), SerializeValue(kvp.Value)));
            }
        }

        /// <summary>
        /// OnAfterDeserialize implementation.
        /// </summary>
        public void OnAfterDeserialize()
        {
            TKey deserializedKey;
            TValue deserializedValue;

            Clear();

            foreach(var data in _datas)
            {
                deserializedKey = DeserializeKey(data.Key);
                deserializedValue = DeserializeValue(deserializedKey, data.Value);

                Add(deserializedKey, deserializedValue);
            }

            _datas.Clear();
        }
    } 
}
