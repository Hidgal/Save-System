using SaveSystem.Internal.Settings;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SaveSystem.Editor.Utils
{
    public static class SaveSystemAssetUtils
    {
        public static void SelectAssetOfType<T>() where T : UnityEngine.Object
        {
            PingAsset(GetAsset<T>());
        }

        public static void PingFolder(string relativePathToFolder)
        {
            CreateFolderIfNotExists(Path.Combine(Application.dataPath, relativePathToFolder));

            var folderAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(relativePathToFolder);
            PingAsset(folderAsset);
        }

        public static void OpenFolderInExplorer(string pathToFolder)
        {
            CreateFolderIfNotExists(pathToFolder);
            Process.Start(pathToFolder);
        }

        public static SaveSystemSettings GetSettings()
        {
            //TODO: braching based on defines
            var settingsAsset = GetAsset<SaveSystemSettingsAsset>();

            if (settingsAsset)
                return settingsAsset.Settings;

            return default;
        }

        private static T GetAsset<T>() where T : UnityEngine.Object
        {
            var assetGuid = AssetDatabase.FindAssets($"t:{typeof(T)}").FirstOrDefault();
            if (assetGuid != null && assetGuid.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(assetGuid);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return null;
        }

        private static void PingAsset(UnityEngine.Object asset)
        {
            if (!asset) return;

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        private static void CreateFolderIfNotExists(string pathToFolder)
        {
            if (!Directory.Exists(pathToFolder))
            {
                Directory.CreateDirectory(pathToFolder);
            }
        }
    }
} 
