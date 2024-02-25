using System.IO;
using SaveSystem.Utils;
using UnityEngine;

namespace SaveSystem.Misc
{
    [System.Serializable]
    public class SaveSystemSettings
    {
#if UNITY_EDITOR
        public bool UseScriptableSavesInEditor => _useScriptableSavesInEditor;
        /// <summary>
        /// Editor only! Full path to saves folder including path to project
        /// </summary>
        public string ScriptableSavesPath => Path.Combine(Application.dataPath, _scriptableSavesPath);
        /// <summary>
        /// Editor only! Relative path to saves folder. Starting from Assets
        /// </summary>
        public string ScriptableSavesRelativePath => Path.Combine("Assets", _scriptableSavesPath);
#endif

        public string DefaultProfileName => _defaultProfileName;

        /// <summary>
        /// Full path to saves folder including path to data storage
        /// </summary>
        public string JsonSavePath => Path.Combine(Application.persistentDataPath, _jsonSavePath);
        public bool UseEncryption => _useDataEncryption;
        public byte[] EncryptionKey => _encryptionKey;
        public byte[] EncryptionIv => _encryptionIv;

        public bool AutoSaveOnApplicationQuit => _autoSaveOnApplicationQuit;
        public bool AutoSaveOnApplicationLostFocus => _autoSaveOnApplicationLostFocus;


#if UNITY_EDITOR
        [Header("Scriptable Saves Settings")]
        [SerializeField]
        private bool _useScriptableSavesInEditor = true;
        [SerializeField]
        [Tooltip("Relative to the Assets folder")]
        private string _scriptableSavesPath = "Saves";
#endif

        [Space]
        [Header("Json settings")]
        [SerializeField]
        [Tooltip("Relative to the App data folder")]
        private string _jsonSavePath = "Data";
        
        [Header("Encryption Settings")]
        [SerializeField]
        private bool _useDataEncryption = true;
        [SerializeField]
        private byte[] _encryptionKey;
        [SerializeField]
        private byte[] _encryptionIv;

        [Space]
        [Header("Other settings")]
        [SerializeField]
        private bool _autoSaveOnApplicationQuit = true;
        [SerializeField]
        private bool _autoSaveOnApplicationLostFocus = true;

        [Space]
        [SerializeField]
        private string _defaultProfileName = "Default Profile";

        public void GenerateKeys(bool useDefault = true)
        {
            string keyReference = useDefault ? EncryptionSystem.DEFAULT_KEY : Application.productName;
            string ivReference = useDefault ? EncryptionSystem.DEFAULT_IV : Application.companyName;

            EncryptionSystem.GenerateKey(ref _encryptionKey, 32, keyReference);
            EncryptionSystem.GenerateKey(ref _encryptionIv, 16, ivReference);
        }
    }
}

