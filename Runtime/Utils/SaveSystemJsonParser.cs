using System;
using SaveSystem.Misc;
using UnityEngine;

namespace SaveSystem.Utils
{
    public class SaveSystemJsonParser
    {
        private const string DATA_HEADER = "SaveData\n";

        private readonly SaveSystemSettings _settings;

        private bool _useEncryption => _settings.UseEncryption;

        public SaveSystemJsonParser(SaveSystemSettings settings)
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
                result = SaveSystemEncryption.Encrypt(_settings.EncryptionKey, _settings.EncryptionIv, dataString);
            }
            else
            {
                result = SaveSystemEncryption.GetBytes(dataString);
            }

            return result;
        }

        public T FromJson<T>(byte[] data)
        {
            string dataString = SaveSystemEncryption.GetString(data);

            if (!dataString.Contains(DATA_HEADER))
            {
                dataString = SaveSystemEncryption.Decrypt(_settings.EncryptionKey, _settings.EncryptionIv, data);
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
