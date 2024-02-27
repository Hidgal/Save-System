using System.Collections.Generic;
using System;
using UnityEngine;

namespace SaveSystem.Utils
{
    [Serializable]
    public class SerializableDictionary<K, V> : SerializableDictionary<K, V, K, V>
    {
        /// <summary>
        /// Conversion to serialize a key
        /// </summary>
        /// <param name="key">The key to serialize</param>
        /// <returns>The Key that has been serialized</returns>
        public override K SerializeKey(K key) => key;

        /// <summary>
        /// Conversion to serialize a value
        /// </summary>
        /// <param name="val">The value</param>
        /// <returns>The value</returns>
        public override V SerializeValue(V val) => val;

        /// <summary>
        /// Conversion to serialize a key
        /// </summary>
        /// <param name="key">The key to serialize</param>
        /// <returns>The Key that has been serialized</returns>
        public override K DeserializeKey(K key) => key;

        /// <summary>
        /// Conversion to serialize a value
        /// </summary>
        /// <param name="val">The value</param>
        /// <returns>The value</returns>
        public override V DeserializeValue(K key, V val) => val;
    }

    /// <summary>
    /// Dictionary that can serialize keys and values as other types
    /// </summary>
    /// <typeparam name="K">The key type</typeparam>
    /// <typeparam name="V">The value type</typeparam>
    /// <typeparam name="SK">The type which the key will be serialized for</typeparam>
    /// <typeparam name="SV">The type which the value will be serialized for</typeparam>
    [Serializable]
    public abstract class SerializableDictionary<K, V, SK, SV> : Dictionary<K, V>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<DictionaryData<SK, SV>> _datas = new();

        /// <summary>
        /// From <see cref="K"/> to <see cref="SK"/>
        /// </summary>
        /// <param name="key">They key in <see cref="K"/></param>
        /// <returns>The key in <see cref="SK"/></returns>
        public abstract SK SerializeKey(K key);

        /// <summary>
        /// From <see cref="V"/> to <see cref="SV"/>
        /// </summary>
        /// <param name="value">The value in <see cref="V"/></param>
        /// <returns>The value in <see cref="SV"/></returns>
        public abstract SV SerializeValue(V value);


        /// <summary>
        /// From <see cref="SK"/> to <see cref="K"/>
        /// </summary>
        /// <param name="serializedKey">They key in <see cref="SK"/></param>
        /// <returns>The key in <see cref="K"/></returns>
        public abstract K DeserializeKey(SK serializedKey);

        /// <summary>
        /// From <see cref="SV"/> to <see cref="V"/>
        /// </summary>
        /// <param name="serializedValue">The value in <see cref="SV"/></param>
        /// <returns>The value in <see cref="V"/></returns>
        public abstract V DeserializeValue(K deserializedKey, SV serializedValue);

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
            K deserializedKey;
            V deserializedValue;

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
