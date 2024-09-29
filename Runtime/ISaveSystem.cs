namespace SaveSystem
{
    public interface ISaveSystem
    {
        /// <summary>
        /// Global saved data. Doesn`t depend on the active profile
        /// </summary>
        ISave Global { get; }

        /// <summary>
        /// Name of currently selected profile
        /// </summary>
        string ActiveProfileName { get; }

        /// <summary>
        /// Data of currently selected profile
        /// </summary>
        ISave Profile { get; }

        /// <summary>
        /// Data of currently selected profile
        /// </summary>
        ISave GetCurentProfileData();



        /// <summary>
        /// Returns data for profile with nameed as <paramref name="profileName"/>. 
        /// Returns new data if there`s no data in save files.
        /// </summary>
        ISave GetProfileData(string profileName);

        /// <summary>
        /// Creates new profile names as <paramref name="profileName"/>
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        ISave CreateProfile(string profileName);

        /// <summary>
        /// Switch active profile to <paramref name="profileName"/>. If there`s no such profile, creates new.
        /// </summary>
        /// <param name="profileName"></param>
        void SetProfile(string profileName);

        /// <summary>
        /// Clear all data of active profile.
        /// </summary>
        void ClearActiveProfile();

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

