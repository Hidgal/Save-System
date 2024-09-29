using UnityEngine;

namespace SaveSystem.Internal.Data
{
    public class SaveSystemSaveData : SaveData
    {
        [SerializeField]
        private string _profileName;

        public string ProfileName 
        { 
            get => _profileName; 
            internal set
            {
                _profileName = value;
                Save();
            }
        }
    }
}
