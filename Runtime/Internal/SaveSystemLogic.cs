using System.Collections.Generic;
using SaveSystem.Internal.Data;
using SaveSystem.Internal.SaveLoaders;
using SaveSystem.Internal.Settings;
using UnityEngine;

namespace SaveSystem.Internal
{
    public class SaveSystemLogic : ISaveSystem
    {
        private readonly Dictionary<string, SaveContainer> _profileDatas;
        private readonly SaveSystemSettings _settings;
        private readonly SaveLoader _saveLoader;

        private SaveSystemSaveData _mainSave;
        private SaveContainer _globalData;
        private SaveContainer _activeProfile;

        public ISave Global => GetGlobalData();
        public ISave Profile => GetCurentProfile();

        public string ProfileName
        {
            get => GetActiveProfileName();
            private set => MainSave.ProfileName = value;
        }

        internal SaveSystemSaveData MainSave
        {
            get => GetMainSave();
        }


        public SaveSystemLogic(SaveSystemSettings settings)
        {
            _profileDatas = new();
            _settings = settings;
            _saveLoader = new(settings);
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

        public void SwitchProfile(string profileName)
        {
            if (ProfileName.Equals(profileName)) return;

            ProfileName = profileName;
            _activeProfile = GetProfileDataInternal(profileName);
        }

        public ISave GetCurentProfile()
        {
            if (_activeProfile == null)
            {
                return GetProfileDataInternal(ProfileName);
            }

            return _activeProfile;
        }

        public ISave GetProfile(string profileName) => GetProfileDataInternal(profileName);

        public ISave CreateProfile(string profileName)
        {
            if (_profileDatas.ContainsKey(profileName))
            {
                UnityEngine.Debug.LogError($"Can`t create profile with name {profileName}: there`s already a profile with the same name!");
                return _profileDatas[profileName];
            }

            return CreateProfileInternal(profileName);
        }


        public void ClearProfile() => ClearProfile(ProfileName);
        public void ClearProfile(string profileName)
        {
            if (_profileDatas.ContainsKey(profileName))
            {
                _profileDatas.Remove(profileName);
            }

            _saveLoader.ClearData(profileName);
        }


        private ISave GetGlobalData()
        {
            if (_globalData == null)
            {
                _globalData = _saveLoader.LoadData(SaveSystemConstants.GLOBAL_DATA_KEY);
                InitializeData(_globalData, SaveSystemConstants.GLOBAL_DATA_KEY);
            }

            return _globalData;
        }

        private SaveContainer GetProfileDataInternal(string profileName)
        {
            if (_profileDatas.ContainsKey(profileName))
            {
                return _profileDatas[profileName];
            }

            return CreateProfileInternal(profileName);
        }
        private SaveContainer CreateProfileInternal(string profileName)
        {
            var data = _saveLoader.LoadData(profileName);
            _profileDatas.Add(profileName, data);

            InitializeData(_profileDatas[profileName], profileName);

            return _profileDatas[profileName];
        }

        private string GetActiveProfileName()
        {
            if (string.IsNullOrEmpty(MainSave.ProfileName))
                return _settings.DefaultProfileName;

            return MainSave.ProfileName;
        }

        private SaveSystemSaveData GetMainSave()
        {
            if (_mainSave == null)
                _mainSave = Global.Get<SaveSystemSaveData>();

            return _mainSave;
        }

        private void InitializeData(SaveContainer data, string key)
        {
            data.Initialize(() => _saveLoader.SaveData(key, data), () => _saveLoader.ClearData(key));
        }
    }
}
