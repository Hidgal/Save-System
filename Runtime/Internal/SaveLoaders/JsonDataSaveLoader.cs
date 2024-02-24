using System;
using System.IO;
using SaveSystem.Misc;
using UnityEngine;

namespace SaveSystem.Internal.SaveLoaders
{
    internal class JsonDataSaveLoader : DataSaveLoader
    {
        private readonly string _fileExtension;
        private readonly JsonParser _parser;

        public JsonDataSaveLoader(SaveSystemSettings settings) : base(settings)
        {
            _fileExtension = Settings.FileExtension.Contains(".") ? Settings.FileExtension : $".{Settings.FileExtension}";
            _parser = new(settings);
        }

        internal override SaveData LoadData(string key)
        {
            var pathToFile = Path.Combine(Settings.JsonSavePath, key + _fileExtension);

            SaveData data;

            CreateFolderIfNotExists(Settings.JsonSavePath);

            if (!File.Exists(pathToFile))
            {
                data = new();
                File.WriteAllText(pathToFile, _parser.ToJson(data));
            }
            else
            {
                var json = File.ReadAllText(pathToFile);

                try
                {
                    data = _parser.FromJson<SaveData>(json);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Loading of {key} failed: can`t parse data from json.\n{ex.Message}\nStacktrace:\n{ex.StackTrace}");
                    data = default;
                }
            }

            return data;
        }

        internal override void SaveData(string key, SaveData data)
        {
            CreateFolderIfNotExists(Settings.JsonSavePath);

            var pathToFile = Path.Combine(Settings.JsonSavePath, key + _fileExtension);
            string parsedData = _parser.ToJson(data);

            File.WriteAllText(pathToFile, parsedData);
        }

        internal override void ClearData(string key)
        {
            if (!Directory.Exists(Settings.JsonSavePath)) return;

            var pathToFile = Path.Combine(Settings.JsonSavePath, key + _fileExtension);
            if (!File.Exists(pathToFile)) return;

            File.Delete(pathToFile);
        }
    }
}

