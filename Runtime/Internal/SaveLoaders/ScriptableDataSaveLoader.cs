using System.IO;
using SaveSystem.Misc;
using UnityEditor;
using UnityEngine;

namespace SaveSystem.Internal.SaveLoaders
{
#if UNITY_EDITOR
    internal class ScriptableDataSaveLoader : DataSaveLoader
    {
        private const string ASSET_EXTENSION = ".asset";

        public ScriptableDataSaveLoader(SaveSystemSettings settings) : base(settings)
        {
        }

        internal override SaveData LoadData(string key)
        {
            var dataAsset = GetOrCreateDataAsset(Settings.ScriptableSavesRelativePath, key);

            if (dataAsset)
            {
                if (dataAsset.Data == null)
                {
                    dataAsset.SaveData(new());
                }

                return dataAsset.Data;
            }

            Debug.LogError($"Can`t load data with name {key}: something went wrong!");
            return default;
        }

        internal override void SaveData(string key, SaveData data)
        {
            var dataAsset = GetOrCreateDataAsset(Settings.ScriptableSavesRelativePath, key);
            dataAsset.SaveData(data);
            AssetDatabase.SaveAssetIfDirty(dataAsset);
        }

        internal override void ClearData(string key)
        {
            var folderPath = Settings.ScriptableSavesRelativePath;
            if (AssetDatabase.IsValidFolder(folderPath))
            {
                var assetPath = Path.Combine(folderPath, key + ASSET_EXTENSION);
                var dataAsset = AssetDatabase.LoadAssetAtPath<SaveDataScriptable>(assetPath);

                if (dataAsset)
                {
                    dataAsset.SaveData(new());
                }
            }
        }

        private SaveDataScriptable GetOrCreateDataAsset(string folderPath, string assetName)
        {
            CreateFolderIfNotExists(folderPath);

            var assetPath = Path.Combine(folderPath, assetName + ASSET_EXTENSION);
            var dataAsset = AssetDatabase.LoadAssetAtPath<SaveDataScriptable>(assetPath);
            if (!dataAsset)
            {
                var scriptableInstance = ScriptableObject.CreateInstance<SaveDataScriptable>();
                AssetDatabase.CreateAsset(scriptableInstance, assetPath);
                AssetDatabase.SaveAssets();

                dataAsset = AssetDatabase.LoadAssetAtPath<SaveDataScriptable>(assetPath);
            }

            return dataAsset;
        }

        protected override void CreateFolderIfNotExists(string relativeFolderPath)
        {
            base.CreateFolderIfNotExists(relativeFolderPath);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }  
#endif
}
