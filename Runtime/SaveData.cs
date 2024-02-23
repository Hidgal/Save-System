using System;
using SaveSystem.Internal;
using SaveSystem.Utils;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class SaveData
    {
        [SerializeField]
        private SerializedDictionary<string, int> _intValues = new();
        [SerializeField]
        private SerializedDictionary<string, bool> _boolValues = new();
        [SerializeField]
        private SerializedDictionary<string, float> _floatValues = new();
        [SerializeField]
        private SerializedDictionary<string, string> _stringValues = new();
        [SerializeField]
        private SaveableClassesDictionary _classesSave = new();

        private Action _saveAction;
        private Action _clearAction;

        #region Classes Save
        /// <typeparam name="T">Type of saveable class</typeparam>
        /// <returns>True if has data in save</returns>
        public bool HasClass<T>() where T : SaveableClass
        {
            return _classesSave.ContainsKey(_classesSave.GetClassKey<T>());
        }
        /// <summary>
        /// Returns instance of required type. 
        /// If there`s no instance in save - creates new and immediately saves if autoSave is true.
        /// </summary>
        /// <typeparam name="T">Type of saveable class</typeparam>
        /// <returns>Instance of required type</returns>
        public T GetClass<T>(bool autoSave = true) where T : SaveableClass
        {
            var key = _classesSave.GetClassKey<T>();

            if (_classesSave.ContainsKey(key))
            {
                _classesSave[key].SaveAction = _saveAction;
            }
            else
            {
                var instance = CreateClassInstance<T>();
                instance.SaveAction = _saveAction;
                _classesSave.Add(key, instance);

                if(autoSave)
                    Save();
            }

            return _classesSave[key] as T;
        }
        /// <summary>
        /// If autoSave is true - immediately saves data to file
        /// </summary>
        /// <typeparam name="T">Type of saveable class</typeparam>
        /// <param name="autoSave">If true - immediately saves data to file</param>
        public void SaveClass<T>(T classData, bool autoSave = true) where T : SaveableClass
        {
            if (classData == null)
            {
                Debug.LogError("Saving failed: Can`t save null data!");
                return;
            }

            var key = _classesSave.GetClassKey<T>();

            if (_classesSave.ContainsKey(key))
            {
                _classesSave[key] = classData;
            }
            else
            {
                _classesSave.Add(key, classData);
            }

            if (autoSave)
                Save();
        }
        #endregion

        #region Int Save
        /// <returns>True if has value with provided key in save</returns>
        public bool HasIntValue(string key) => _intValues.ContainsKey(key);

        /// <returns>Value by provided key. If has no value - returns default.</returns>
        public int GetIntValue(string key)
        {
            if (_intValues.ContainsKey(key))
            {
                return _intValues[key];
            }

            return default;
        }
        /// <summary>
        /// If autoSave is true - immediately saves data to file
        /// </summary>
        /// <param name="autoSave">If true - immediately saves data to file</param>
        public void SaveIntValue(string key, int value, bool autoSave = true)
        {
            if (_intValues.ContainsKey(key))
            {
                _intValues[key] = value;
            }
            else
            {
                _intValues.Add(key, value);
            }

            if (autoSave)
                Save();
        }
        #endregion

        #region Float Save
        /// <returns>True if has value with provided key in save</returns>
        public bool HasFloatValue(string key) => _floatValues.ContainsKey(key);

        /// <returns>Value by provided key. If has no value - returns default.</returns>
        public float GetFloatValue(string key)
        {
            if (_floatValues.ContainsKey(key))
            {
                return _floatValues[key];
            }

            return default;
        }
        /// <summary>
        /// If autoSave is true - immediately saves data to file
        /// </summary>
        /// <param name="autoSave">If true - immediately saves data to file</param>
        public void SaveFloatValue(string key, float value, bool autoSave = true)
        {
            if (_floatValues.ContainsKey(key))
            {
                _floatValues[key] = value;
            }
            else
            {
                _floatValues.Add(key, value);
            }

            if (autoSave)
                Save();
        }
        #endregion

        #region Bool Save
        /// <returns>True if has value with provided key in save</returns>
        public bool HasBoolValue(string key) => _boolValues.ContainsKey(key);

        /// <returns>Value by provided key. If has no value - returns default.</returns>
        public bool GetBoolValue(string key)
        {
            return default;
        }
        /// <summary>
        /// If autoSave is true - immediately saves data to file
        /// </summary>
        /// <param name="autoSave">If true - immediately saves data to file</param>
        public void SaveBoolValue(string key, bool value, bool autoSave = true)
        {
            if (_boolValues.ContainsKey(key))
            {
                _boolValues[key] = value;
            }
            else
            {
                _boolValues.Add(key, value);
            }

            if (autoSave)
                Save();
        }
        #endregion

        #region String Save
        /// <returns>True if has value with provided key in save</returns>
        public bool HasStringValue(string key) => _stringValues.ContainsKey(key);

        /// <returns>Value by provided key. If has no value - returns empty string.</returns>
        public string GetStringValue(string key)
        {
            if (_stringValues.ContainsKey(key))
            {
                return _stringValues[key];
            }

            return string.Empty;
        }
        /// <summary>
        /// If autoSave is true - immediately saves data to file
        /// </summary>
        /// <param name="autoSave">If true - immediately saves data to file</param>
        public void SaveStringValue(string key, string value, bool autoSave = true)
        {
            if (_stringValues.ContainsKey(key))
            {
                _stringValues[key] = value;
            }
            else
            {
                _stringValues.Add(key, value);
            }

            if (autoSave)
                Save();
        } 
        #endregion

        /// <summary>
        /// Saves all data to file.
        /// </summary>
        public void Save()
        {
            _saveAction?.Invoke();
        }

        /// <summary>
        /// Clear all data from file
        /// </summary>
        public void Clear()
        {
            _clearAction?.Invoke();
        }

        internal void Initialize(Action saveAction, Action clearAction)
        {
            _saveAction = saveAction;
            _clearAction = clearAction;
        }

        private T CreateClassInstance<T>() where T : SaveableClass
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }  
}
