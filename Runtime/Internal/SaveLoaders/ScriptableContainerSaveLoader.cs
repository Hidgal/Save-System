using System.Collections.Generic;
using System.IO;
using SaveSystem.Internal.Data;
using SaveSystem.Internal.Settings;
using UnityEditor;
using UnityEngine;

namespace SaveSystem.Internal.SaveLoaders
{
    public class ScriptableContainerSaveLoader
    {
        private readonly SaveSystemSettings _settings;
        private readonly Dictionary<string, SaveContainerScriptable> _loaded;
        private readonly HashSet<string> _unavailable;

        public ScriptableContainerSaveLoader(SaveSystemSettings settings)
        {
            _settings = settings;
            _loaded = new();
            _unavailable = new();
        }

        public SaveContainerScriptable Load(string name)
        {
            if (_unavailable.Contains(name)) return null;

            SaveContainerScriptable result;
            var path = Path.Combine(_settings.ScriptableSavesRelativePath, name);
            result = Resources.Load<SaveContainerScriptable>(path);

#if UNITY_EDITOR
            if (!result)
                result = CreateScriptable(name);
#endif

            if (!result)
                _unavailable.Add(name);
            else
                _loaded.Add(name, result); 

            return result;
        }

        public void Save(string name, SaveContainer data)
        {
            if(_loaded.TryGetValue(name, out var asset))
            {
                asset.Data = data;
                return;
            }

#if UNITY_EDITOR
            var scriptable = CreateScriptable(name);
            _loaded.Add(name, scriptable);
            scriptable.Data = data; 
#endif
        }

        public void Clear(string name)
        {
            if(_loaded.TryGetValue(name, out var asset))
            {
                asset.Data = new();
            }
        }

#if UNITY_EDITOR
        private SaveContainerScriptable CreateScriptable(string name)
        {
            CreateSavesFolderIfNotExists();
            var assetPath = Path.Combine(_settings.ScriptableSavesRelativePath, name + ".asset");
            var scriptableInstance = ScriptableObject.CreateInstance<SaveContainerScriptable>();
            AssetDatabase.CreateAsset(scriptableInstance, assetPath);
            AssetDatabase.SaveAssets();

            return AssetDatabase.LoadAssetAtPath<SaveContainerScriptable>(assetPath);
        }
        private void CreateSavesFolderIfNotExists()
        {
            var path = _settings.ScriptableSavesPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }
#endif
    }
}
