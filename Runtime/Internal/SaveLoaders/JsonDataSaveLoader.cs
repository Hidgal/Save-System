using System;
using System.IO;
using SaveSystem.Internal.Data;
using SaveSystem.Internal.Settings;
using SaveSystem.Internal.Utils;
using UnityEngine;

namespace SaveSystem.Internal.SaveLoaders
{
    internal class JsonDataSaveLoader : DataSaveLoader
    {
        private readonly SaveSystemJsonParser _parser;

        public JsonDataSaveLoader(SaveSystemSettings settings) : base(settings)
        {
            _parser = new(settings);
        }

        internal override SaveContainer LoadData(string key)
        {
            var pathToFile = Path.Combine(Settings.JsonSavePath, key + SaveSystemConstants.FILE_EXTENSION);

            SaveContainer data;

            CreateFolderIfNotExists(Settings.JsonSavePath);

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

        internal override void SaveData(string key, SaveContainer data)
        {
            CreateFolderIfNotExists(Settings.JsonSavePath);

            var pathToFile = Path.Combine(Settings.JsonSavePath, key + SaveSystemConstants.FILE_EXTENSION);
            var parsedData = _parser.ToJson(data);

            File.WriteAllBytes(pathToFile, parsedData);
        }

        internal override void ClearData(string key)
        {
            if (!Directory.Exists(Settings.JsonSavePath)) return;

            var pathToFile = Path.Combine(Settings.JsonSavePath, key + SaveSystemConstants.FILE_EXTENSION);
            if (!File.Exists(pathToFile)) return;

            File.Delete(pathToFile);
        }
    }
}

