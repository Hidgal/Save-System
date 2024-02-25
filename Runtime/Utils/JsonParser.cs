using System;
using System.Globalization;
using System.Text;
using SaveSystem.Misc;
using UnityEngine;

namespace SaveSystem.Utils
{
    public class JsonParser
    {
        private const string DATA_HEADER = "SaveData\n";

        private readonly SaveSystemSettings _settings;

        private bool _useEncryption => _settings.UseEncryption;

        public JsonParser(SaveSystemSettings settings)
        {
            _settings = settings;
        }

        public byte[] ToJson<T>(T data, bool prettyPrint = true)
        {
            var result = new byte[0];
            if (data == null) return result;

            string dataString = DATA_HEADER;
            dataString += JsonUtility.ToJson(data, prettyPrint);

            if (_useEncryption)
            {
                result = EncryptionSystem.Encrypt(_settings.EncryptionKey, _settings.EncryptionIv, dataString);
            }
            else
            {
                result = EncryptionSystem.GetBytes(dataString);
            }

            return result;
        }

        public T FromJson<T>(byte[] data)
        {
            string dataString = EncryptionSystem.GetString(data);

            if (!dataString.Contains(DATA_HEADER))
            {
                dataString = EncryptionSystem.Decrypt(_settings.EncryptionKey, _settings.EncryptionIv, data);
            }
            
            dataString = dataString.Replace(DATA_HEADER, string.Empty);

            T result = default;

            try
            {
                result = JsonUtility.FromJson<T>(dataString);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"JSON parsing failed:\n{ex.Message}\nStackTrace:\n{ex.StackTrace}");
            }

            return result;
        }
    }
}
