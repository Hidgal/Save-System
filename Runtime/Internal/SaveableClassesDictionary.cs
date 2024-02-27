using System;
using SaveSystem.Utils;
using UnityEngine;

namespace SaveSystem.Internal
{
    [Serializable]
    internal class SaveableClassesDictionary : SerializableDictionary<string, SaveableData, string, string>
    {
        public override string DeserializeKey(string serializedKey) => serializedKey;

        public override SaveableData DeserializeValue(string currentType, string serializedValue)
        {
            if (serializedValue == null || serializedValue.Equals(string.Empty) || serializedValue.Length == 0)
                return default;

            var type = Type.GetType(currentType);

            if (type == null) 
                return default;

            var result = JsonUtility.FromJson(serializedValue, type) as SaveableData;
            
            if (result != null)
                return result;

            return default;
        }

        public override string SerializeKey(string key) => key;
        public override string SerializeValue(SaveableData value)
        {
            if (value == null)
                return string.Empty;

            return JsonUtility.ToJson(value);
        }

        public string GetClassKey<T>() where T : SaveableData
        {
            return typeof(T).AssemblyQualifiedName;
        }
    }
}

