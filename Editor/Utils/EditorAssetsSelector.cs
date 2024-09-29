using SaveSystem.Internal.Settings;
using UnityEditor;

namespace SaveSystem.Editor.Utils
{
    internal static class EditorAssetsSelector
    {
        [MenuItem(SaveSystemEditorConstants.MENU_ITEM_NAME + "Select Settings", secondaryPriority = 100)]
        public static void SelectSettingsAsset()
        {
            SaveSystemAssetUtils.SelectAssetOfType<SaveSystemSettingsAsset>();
        }

        [MenuItem(SaveSystemEditorConstants.MENU_ITEM_NAME + "Open Saves Folder", secondaryPriority = 10)]
        public static void OpenSavesFolder()
        {
            var settings = SaveSystemAssetUtils.GetSettings();
            SaveSystemAssetUtils.PingFolder(settings.ScriptableSavesRelativePath);
        }
    }
} 
