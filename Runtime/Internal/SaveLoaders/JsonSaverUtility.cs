using UnityEngine;
using System.IO;
using System.Text;

namespace SaveSystem.Internal
{
    internal static class JsonSaverUtility
    {
        /// <summary>Serialize object as JSON</summary>
        /// <param name="objectToSave">What to save</param>
        /// <param name="directory">Folder to save the file</param>
        /// <param name="fileName">Without ".json"</param>
        public static void SaveToJson(object objectToSave, string directory, string fileName, int encryptionKey = 0)
        {
            CheckOrCreateDirectory(directory);

            string json = JsonUtility.ToJson(objectToSave);
            string path = Path.Combine(directory, fileName) + ".json";

            if (encryptionKey != 0)
            {
                json = EncryptOrDecrypt(json, encryptionKey);
            }

            File.WriteAllText(path, json);
        }

        /// <summary>Deserialize object from disk</summary>
        /// <param name="directory">Folder where file is located</param>
        /// <param name="fileName">Without ".json"</param>
        public static T LoadJson<T>(string directory, string fileName, int encryptionKey = 0)
        {
            string path = Path.Combine(directory, fileName) + ".json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                if (encryptionKey != 0)
                {
                    json = EncryptOrDecrypt(json, encryptionKey);
                }

                T obj = default;

                try
                {
                    obj = JsonUtility.FromJson<T>(json);
                }
                catch (System.ArgumentException) 
                {
                    Debug.LogError("Decryption of json save failed");
                }
                
                return obj;
            }

            return default;
        }

        /// <summary>
        /// Check if directory exists and create one if it doesnt
        /// </summary>
        public static void CheckOrCreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static string EncryptOrDecrypt(string text, int encryptionKey)
        {
            StringBuilder inputStringBuild = new StringBuilder(text);
            StringBuilder outStringBuild = new StringBuilder(text.Length);
            char currentChar;

            for (int i = 0; i < text.Length; i++)
            {
                currentChar = inputStringBuild[i];
                currentChar = (char)(currentChar ^ encryptionKey);
                outStringBuild.Append(currentChar);
            }

            return outStringBuild.ToString();
        }
    }
}
