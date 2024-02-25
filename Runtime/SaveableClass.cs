using System;

namespace SaveSystem
{
    [Serializable]
    public class SaveableClass
    {
        internal Action SaveAction;

        public void Save()
        {
            SaveAction?.Invoke();
        }
    }
}