using System;
using System.Text;
using SaveSystem.Utils;
using UnityEngine;

namespace SaveSystem.Internal.DataList
{
    [Serializable]
    public class DataContainer
    {
        [SerializeReference]
        private SaveableData _data;

        public SaveableData Data => _data;
        public string Key => _data.GetType().GetSeriallizationKey();

        public DataContainer(SaveableData data)
        {
            _data = data;
        }

        public string GetSerializedData()
        {
            return new StringBuilder()
                .AppendLine(Key)
                .AppendLine(JsonUtility.ToJson(_data))
                .ToString();
        }
    }
}

