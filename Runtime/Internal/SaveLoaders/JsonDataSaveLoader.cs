using System;
using System.IO;
using SaveSystem.Internal.Data;
using SaveSystem.Internal.Settings;
using SaveSystem.Internal.Utils;
using UnityEngine;

namespace SaveSystem.Internal.SaveLoaders
{
    public class JsonDataSaveLoader
    {
        private readonly SaveSystemSettings _settings;
        private readonly SaveSystemJsonParser _parser;

        public JsonDataSaveLoader(SaveSystemSettings settings)
        {
            _settings = settings;
            _parser = new(settings);
        }

        public SaveContainer LoadData(string key)
        {
            var pathToFile = Path.Combine(_settings.JsonSavePath, key + SaveSystemConstants.FILE_EXTENSION);

            SaveContainer data;

            CreateFolderIfNotExists(_settings.JsonSavePath);

            if (!File.Exists(pathToFile))
            {
                data = new();
                File.WriteAllBytes(pathToFile, _parser.ToJson(data));
            }
            else
            {
                var json = File.ReadAllBytes(pathToFile);

                try
                {
                    data = _parser.FromJson<SaveContainer>(json);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Loading of {key} failed: can`t parse data from json.\n{ex.Message}\nStacktrace:\n{ex.StackTrace}");
                    data = default;
                }
            }

            return data;
        }

        public void SaveData(string key, SaveContainer data)
        {
            CreateFolderIfNotExists(_settings.JsonSavePath);

            var pathToFile = Path.Combine(_settings.JsonSavePath, key + SaveSystemConstants.FILE_EXTENSION);
            var parsedData = _parser.ToJson(data);

            File.WriteAllBytes(pathToFile, parsedData);
        }

        public void ClearData(string key)
        {
            if (!Directory.Exists(_settings.JsonSavePath)) return;

            var pathToFile = Path.Combine(_settings.JsonSavePath, key + SaveSystemConstants.FILE_EXTENSION);
            if (!File.Exists(pathToFile)) return;

            File.Delete(pathToFile);
        }

        private void CreateFolderIfNotExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}

