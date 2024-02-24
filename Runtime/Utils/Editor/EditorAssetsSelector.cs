#if UNITY_EDITOR
using SaveSystem.Internal;
using SaveSystem.Zenject;
using UnityEditor;

namespace SaveSystem.Utils.Editor
{
    public static class EditorAssetsSelector
    {
        [MenuItem(SaveSystemConstants.MENU_ITEM_NAME + "Select Settings", secondaryPriority = 100)]
        public static void SelectSettingsAsset()
        {
            SaveSystemAssetUtils.SelectAssetOfType<SaveSystemInstaller>();
        }

        [MenuItem(SaveSystemConstants.MENU_ITEM_NAME + "Open Saves Folder", secondaryPriority = 10)]
        public static void OpenSavesFolder()
        {
            var settings = SaveSystemAssetUtils.GetSettings();

            if (settings.UseScriptableSavesInEditor)
            {
                SaveSystemAssetUtils.PingFolder(settings.ScriptableSavesRelativePath);
            }
            else
            {
                SaveSystemAssetUtils.OpenFolderInExplorer(settings.JsonSavePath);
            }
        }
    }
} 
#endif
