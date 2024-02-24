using System.IO;
using SaveSystem.Internal;
using SaveSystem.Scriptables;
using UnityEditor;

namespace SaveSystem.Utils.Editor
{
    public static class JsonToScriptableParser
    {
        public static void ImportDataFromJson(SaveDataScriptable dataAsset)
        {
            if (!dataAsset) return;

            var pathToFile = EditorUtility.OpenFilePanelWithFilters("Select JSON with save", "", new string[] { "JSON files", "json" });
            if (File.Exists(pathToFile))
            {
                var fileString = File.ReadAllText(pathToFile);
                var settings = SaveSystemAssetUtils.GetSettings();

                var parser = new JsonParser(settings);
                var result = parser.FromJson<SaveData>(fileString);
                
                if(result != null)
                {
                    dataAsset.SaveData(result);
                    UnityEngine.Debug.Log($"Successfully parsed data from {Path.GetFileName(pathToFile)}");
                }
            }
        }

        public static void ExportDataToJson(SaveDataScriptable dataAsset)
        {
            if (!dataAsset) return;

            var extensionName = SaveSystemConstants.FILE_EXTENSION.Replace(".", "");
            var pathToFile = EditorUtility.SaveFilePanel("Choose folder to save file", "", dataAsset.name, extensionName);
            if (pathToFile.Equals(string.Empty)) return;

            var settings = SaveSystemAssetUtils.GetSettings();
            var parser = new JsonParser(settings);
            var result = parser.ToJson(dataAsset.Data);
            File.WriteAllText(pathToFile, result);
        }
    }
}
