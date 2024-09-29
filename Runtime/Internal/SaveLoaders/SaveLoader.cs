using SaveSystem.Internal.Data;
using SaveSystem.Internal.Settings;

namespace SaveSystem.Internal.SaveLoaders
{
    public class SaveLoader
    {
        private readonly SaveSystemSettings _settings;
        private readonly ScriptableContainerSaveLoader _scriptableLoader;
        private readonly JsonDataSaveLoader _jsonLoader;

        public SaveLoader(SaveSystemSettings settings)
        {
            _settings = settings;
            _scriptableLoader = new(_settings);
            _jsonLoader = new(_settings);
        }

        public SaveContainer LoadData(string key)
        {
            var data = _jsonLoader.LoadData(key);
            
            var scriptableContainer = _scriptableLoader.Load(key);
            if (scriptableContainer)
            {
                scriptableContainer.Data = data;
                return scriptableContainer.Data;
            }

            return data;
        }

        public void SaveData(string key, SaveContainer data)
        {
            _jsonLoader.SaveData(key, data);
            _scriptableLoader.Save(key, data);
        }

        public void ClearData(string key)
        {
            _jsonLoader.ClearData(key);
            _scriptableLoader.Clear(key);
        }
    }
}
