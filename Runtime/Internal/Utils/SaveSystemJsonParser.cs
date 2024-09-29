using System;
using System.Text;
using SaveSystem.Internal.Settings;
using UnityEngine;

namespace SaveSystem.Internal.Utils
{
    public class SaveSystemJsonParser
    {
        private const string DATA_HEADER = "SaveData";

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

            var dataBuilder = new StringBuilder(DATA_HEADER)
                .AppendLine(JsonUtility.ToJson(data, prettyPrint));

            if (_useEncryption)
            {
                result = SaveSystemEncryption.Encrypt(_settings.EncryptionKey, _settings.EncryptionIv, dataBuilder.ToString());
            }
            else
            {
                result = SaveSystemEncryption.GetBytes(dataBuilder.ToString());
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
