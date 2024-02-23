namespace SaveSystem.Internal
{
    public interface ISaveSystem
    {
        SaveData GlobalData { get; }
        string CurrentProfileName { get; }
        SaveData CurrentProfileData { get; }

        SaveData GetCurentProfileData();
        SaveData GetProfileData(string key);
        void SwitchToProfile(string key);
        void ClearCurrentProfile();
        void ClearProfile(string key);

        void SaveAll();
        void SaveAllProfiles();
        void SaveGlobalData();

        void ClearAll();
        void ClearGlobalData();
        void ClearAllProfiles();
    }
}

