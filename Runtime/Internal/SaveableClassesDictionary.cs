using System;
using SaveSystem.Utils;
using UnityEngine;

namespace SaveSystem.Internal
{
    [Serializable]
    internal class SaveableClassesDictionary : SerializedDictionary<string, SaveableClass, string, string>
    {
        public override string DeserializeKey(string serializedKey) => serializedKey;

        public override SaveableClass DeserializeValue(string currentType, string serializedValue)
        {
            var type = Type.GetType(currentType);
            return JsonUtility.FromJson(serializedValue, type) as SaveableClass;
        }

        public override string SerializeKey(string key) => key;
        public override string SerializeValue(SaveableClass value)
        {
            return JsonUtility.ToJson(value);
        }

        public string GetClassKey<T>() where T : SaveableClass
        {
            return typeof(T).AssemblyQualifiedName;
        }
    }
}

