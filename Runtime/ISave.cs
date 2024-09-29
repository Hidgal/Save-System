using System;

namespace SaveSystem
{
    public interface ISave
    {
        bool Contains(Type dataType);
        bool Contains<T>() where T : SaveData;

        SaveData Get(Type dataType);
        T Get<T>() where T : SaveData;

        void Set(SaveData data, Type dataType, bool autoSave = true);
        void Set<T>(T data, bool autoSave = true) where T : SaveData;

        void Save();
        void Clear();
    }
}