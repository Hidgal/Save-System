using System.Collections.Generic;
using SaveSystem.Internal.SaveLoaders;
using SaveSystem.Misc;

namespace SaveSystem.Internal
{
    internal class SaveSystemLogic : ISaveSystem
    {
        private readonly Dictionary<string, SaveData> _profileDatas;
        private readonly SaveSystemSettings _settings;
        private readonly DataSaveLoader _saveLoader;

        private string _activeProfileName;
        private SaveData _globalData;
        private SaveData _activeProfileData;

        public SaveData GlobalData
        {
            get => GetGlobalData();
        }

        //TODO: profile switch logic rework
        public string ActiveProfileName => _activeProfileName;
        //{
        //    get
        //    {
        //        if (GlobalData.HasStringValue(SaveSystemConstants.PROFILE_KEY))
        //        {
        //            return GlobalData.GetStringValue(SaveSystemConstants.PROFILE_KEY);
        //        }
        //        else
        //        {
        //            GlobalData.SetStringValue(SaveSystemConstants.PROFILE_KEY, _settings.DefaultProfileName);
        //            return _settings.DefaultProfileName;
        //        }
        //    }

        //    private set
        //    {
        //        GlobalData.SetStringValue(SaveSystemConstants.PROFILE_KEY, value);
        //    }
        //}

        public SaveData ProfileData => GetCurentProfileData();



        public SaveSystemLogic(SaveSystemSettings settings)
        {
            _profileDatas = new();
            _settings = settings;
            CreateDataSaveLoader(settings);

#if UNITY_EDITOR
            _saveLoader = new ScriptableDataSaveLoader(settings);
            return;
#endif

#pragma warning disable CS0162
            _saveLoader = new JsonDataSaveLoader(settings);
#pragma warning restore CS0162
        }

        public void SaveAll()
        {
            SaveGlobalData();
            SaveAllProfiles();
        }
        public void SaveAllProfiles()
        {
            foreach (var keyValue in _profileDatas)
            {
                _saveLoader.SaveData(keyValue.Key, keyValue.Value);
            }
        }
        public void SaveGlobalData()
        {
            _saveLoader.SaveData(SaveSystemConstants.GLOBAL_DATA_KEY, _globalData);
        }

        public void ClearAll()
        {
            ClearGlobalData();
            ClearAllProfiles();
        }
        public void ClearAllProfiles()
        {
            foreach (var keyValue in _profileDatas)
            {
                _saveLoader.ClearData(keyValue.Key);
            }
        }
        public void ClearGlobalData()
        {
            _saveLoader.ClearData(SaveSystemConstants.GLOBAL_DATA_KEY);
        }

        public void SetProfile(string profileName)
        {
            if (ActiveProfileName.Equals(profileName)) return;

            _activeProfileName = profileName;
            _activeProfileData = GetProfileData(profileName);
        }

        public SaveData GetCurentProfileData()
        {
            if(_activeProfileData == null)
            {
                return GetProfileData(ActiveProfileName);
            }

            return _activeProfileData;
        }
        public SaveData GetProfileData(string profileName)
        {
            if (_profileDatas.ContainsKey(profileName))
            {
                return _profileDatas[profileName];
            }

            return CreateProfileInternal(profileName);
        }

        public SaveData CreateProfile(string profileName)
        {
            if (_profileDatas.ContainsKey(profileName))
            {
                UnityEngine.Debug.LogError($"Can`t create profile with name {profileName}: there`s already a profile with the same name!");
                return _profileDatas[profileName];
            }

            return CreateProfileInternal(profileName);
        }
        private SaveData CreateProfileInternal(string profileName)
        {
            var data = _saveLoader.LoadData(profileName);
            _profileDatas.Add(profileName, data);

            InitializeData(_profileDatas[profileName], profileName);

            return _profileDatas[profileName];
        }

        public void ClearActiveProfile() => ClearProfile(ActiveProfileName);
        public void ClearProfile(string profileName)
        {
            if (_profileDatas.ContainsKey(profileName))
            {
                _profileDatas.Remove(profileName);
            }

            _saveLoader.ClearData(profileName);
        }

        private SaveData GetGlobalData()
        {
            if (_globalData == null)
            {
                _globalData = _saveLoader.LoadData(SaveSystemConstants.GLOBAL_DATA_KEY);
                InitializeData(_globalData, SaveSystemConstants.GLOBAL_DATA_KEY);
            }

            return _globalData;
        }

        private void InitializeData(SaveData data, string key)
        {
            data.Initialize(() => _saveLoader.SaveData(key, data), () => _saveLoader.ClearData(key));
        }

        private void CreateDataSaveLoader(SaveSystemSettings settings)
        {

        }
    }
}
