#if UNITY_EDITOR
using System.IO;
using System.Linq;
using SaveSystem.Internal;
using SaveSystem.Misc;
using SaveSystem.Zenject;
using UnityEditor;

namespace SaveSystem.Utils
{
    public static class EditorSavesCleaner
    {
        [MenuItem("Tools/" + SaveSystemConstants.MENU_ITEM_NAME + "/Clear All Saves")]
        public static void ClearAllSaves()
        {
            var settings = GetSettings();
            if (settings == null) return;

            bool hasDeletedData = ClearDirectory(settings.ScriptableSavesPath);
            
            if (hasDeletedData)
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            hasDeletedData |= ClearDirectory(settings.JsonSavePath);

            if (hasDeletedData)
                UnityEngine.Debug.Log("All saves cleared successfully.");
            else
                UnityEngine.Debug.Log("There`s no save data to clear.");
        }

        /// <returns><see langword="true"/> if there`s deleted files</returns>
        private static bool ClearDirectory(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                var directoryInfo = new DirectoryInfo(folderPath);
                var files = directoryInfo.GetFiles();

                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        file.Delete();
                    }

                    return true;
                }
            }

            return false;
        }

        private static SaveSystemSettings GetSettings()
        {
            //TODO: add zenject define
            var installerAssetGuid = AssetDatabase.FindAssets($"t:{nameof(SaveSystemInstaller)}").FirstOrDefault();
            if (installerAssetGuid != null && installerAssetGuid.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(installerAssetGuid);
                var installer = AssetDatabase.LoadAssetAtPath<SaveSystemInstaller>(path);
                if (installer)
                {
                    return installer.Settings;
                }
            }

            return null;
        }
    }
}
#endif
