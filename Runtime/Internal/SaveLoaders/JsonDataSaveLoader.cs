using SaveSystem.Misc;

namespace SaveSystem.Internal.SaveLoaders
{
    internal class JsonDataSaveLoader : DataSaveLoader
    {
        public JsonDataSaveLoader(SaveSystemSettings settings) : base(settings)
        {
        }

        internal override SaveData LoadData(string key)
        {
            throw new System.NotImplementedException();
        }

        internal override void SaveData(string key, SaveData data)
        {
            throw new System.NotImplementedException();
        }

        internal override void ClearData(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}

