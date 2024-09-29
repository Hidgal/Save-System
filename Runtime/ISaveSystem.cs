namespace SaveSystem
{
    public interface ISaveSystem
    {
        /// <summary>
        /// Global saved data. Doesn`t depend on the active profile
        /// </summary>
        ISave Global { get; }

        /// <summary>
        /// Data of currently selected profile
        /// </summary>
        ISave Profile { get; }

        /// <summary>
        /// Name of currently selected profile
        /// </summary>
        string ProfileName { get; }


        /// <summary>
        /// Returns data for profile with nameed as <paramref name="profileName"/>. 
        /// Returns new data if there`s no data in save files.
        /// </summary>
        ISave GetProfile(string profileName);

        /// <summary>
        /// Creates new profile names as <paramref name="profileName"/>
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        ISave CreateProfile(string profileName);

        /// <summary>
        /// Switch active profile to <paramref name="newProfileName"/>. If there`s no such profile, creates new.
        /// </summary>
        /// <param name="newProfileName"></param>
        void SwitchProfile(string newProfileName);

        /// <summary>
        /// Clear all data of active profile.
        /// </summary>
        void ClearProfile();

        /// <summary>
        /// Clear all data for <paramref name="profileName"/> if it exists.
        /// </summary>
        /// <param name="profileName"></param>
        void ClearProfile(string profileName);



        /// <summary>
        /// Write GlobalData and all profiles to save files.
        /// </summary>
        void SaveAll();

        /// <summary>
        /// Write all profile datas data to save files.
        /// </summary>
        void SaveAllProfiles();

        /// <summary>
        /// Write GlobalData to save file.
        /// </summary>
        void SaveGlobalData();



        /// <summary>
        /// Clear GlobalData and profile datas.
        /// </summary>
        void ClearAll();

        /// <summary>
        /// Clear GlobalData.
        /// </summary>
        void ClearGlobalData();

        /// <summary>
        /// Clear all profile datas
        /// </summary>
        void ClearAllProfiles();
    }
}

