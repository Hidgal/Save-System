using System.IO;
using SaveSystem.Internal.Data;
using SaveSystem.Internal.Settings;

namespace SaveSystem.Internal.SaveLoaders
{
    internal abstract class DataSaveLoader
    {
        protected readonly SaveSystemSettings Settings;

        public DataSaveLoader(SaveSystemSettings settings)
        {
            Settings = settings;
        }

        internal abstract SaveContainer LoadData(string key);
        internal abstract void SaveData(string key, SaveContainer data);
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
