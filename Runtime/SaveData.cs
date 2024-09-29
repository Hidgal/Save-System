using System;

namespace SaveSystem
{
    [Serializable]
    public class SaveData
    {
        internal Action SaveAction;

        public void Save()
        {
            SaveAction?.Invoke();
        }
    }
}