using SaveSystem.Misc;
using UnityEngine;

namespace SaveSystem.Internal
{
    internal class JsonParser
    {
        private readonly SaveSystemSettings _settings;

        private bool _useEncryption => _settings.UseEncryption;

        public JsonParser(SaveSystemSettings settings)
        {
            _settings = settings;
        }

        public string ToJson<T>(T data, bool prettyPrint = true)
        {
            return JsonUtility.ToJson(data, prettyPrint);
        }

        public T FromJson<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }
    }
}
