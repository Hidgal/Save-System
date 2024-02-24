#if UNITY_EDITOR
using System.IO;
using SaveSystem.Internal;
using UnityEditor;

namespace SaveSystem.Utils.Editor
{
    internal static class EditorSavesCleaner
    {
        [MenuItem(SaveSystemConstants.MENU_ITEM_NAME + "Clear All Saves")]
        public static void ClearAllSaves()
        {
            var settings = SaveSystemAssetUtils.GetSettings();
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
    }
}
#endif
