using System.IO;
using SaveSystem.Misc;

namespace SaveSystem.Internal.SaveLoaders
{
    internal abstract class DataSaveLoader
    {
        protected readonly SaveSystemSettings Settings;

        public DataSaveLoader(SaveSystemSettings settings)
        {
            Settings = settings;
        }

        internal abstract SaveData LoadData(string key);
        internal abstract void SaveData(string key, SaveData data);
        internal abstract void ClearData(string key);

        protected virtual void CreateFolderIfNotExists(string folderPath)
{
            if (!Directory.Exists(folderPath))
{
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
