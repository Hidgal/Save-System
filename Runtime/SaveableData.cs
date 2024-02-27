using System;

namespace SaveSystem
{
    [Serializable]
    public class SaveableData
    {
        internal Action SaveAction;

        public void Save()
        {
            SaveAction?.Invoke();
        }
    }
}