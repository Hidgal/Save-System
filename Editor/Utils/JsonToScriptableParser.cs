using System.IO;
using SaveSystem.Internal.Utils;
using SaveSystem.Internal;
using UnityEditor;
using SaveSystem.Internal.Data;

namespace SaveSystem.Editor.Utils
{
    public static class JsonToScriptableParser
    {
        public static void ImportDataFromJson(SaveContainerScriptable dataAsset)
        {
            if (!dataAsset) return;

            var pathToFile = EditorUtility.OpenFilePanelWithFilters("Select JSON with save", "", new string[] { "JSON files", "json" });
            if (File.Exists(pathToFile))
            {
                var fileData = File.ReadAllBytes(pathToFile);
                var settings = SaveSystemAssetUtils.GetSettings();

                var parser = new SaveSystemJsonParser(settings);
                var result = parser.FromJson<SaveContainer>(fileData);

                if (result != null)
                {
                    dataAsset.Data = result;
                    AssetDatabase.SaveAssetIfDirty(dataAsset);

                    UnityEngine.Debug.Log($"Successfully loaded data from {Path.GetFileName(pathToFile)}");
                }
            }
        }

        public static void ExportDataToJson(SaveContainerScriptable dataAsset)
        {
            if (!dataAsset) return;

            var extensionName = SaveSystemConstants.FILE_EXTENSION.Replace(".", "");
            var pathToFile = EditorUtility.SaveFilePanel("Choose folder to save file", "", dataAsset.name, extensionName);
            if (pathToFile.Equals(string.Empty)) return;

            var settings = SaveSystemAssetUtils.GetSettings();
            var parser = new SaveSystemJsonParser(settings);
            var result = parser.ToJson(dataAsset.Data);
            File.WriteAllBytes(pathToFile, result);
        }
    }
}
