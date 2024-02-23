using System.IO;
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

        public string JsonSavePath => Path.Combine(Application.persistentDataPath, _jsonSavePath);
        public int EncryptionKey => _jsonEncryptionKey;

        public bool AutoSaveOnApplicationQuit => _autoSaveOnApplicationQuit;
        public bool AutoSaveOnApplicationLostFocus => _autoSaveOnApplicationLostFocus;


#if UNITY_EDITOR
        [SerializeField]
        private bool _useScriptableSavesInEditor = true;
        [SerializeField]
        [Tooltip("Relative to the Assets folder")]
        private string _scriptableSavesPath = "Saves"; 
#endif

        [Space]
        [SerializeField]
        [Tooltip("Relative to the App data folder")]
        private string _jsonSavePath = "Data";
        [SerializeField]
        private int _jsonEncryptionKey = 8976;

        [Space]
        [SerializeField]
        private bool _autoSaveOnApplicationQuit = true;
        [SerializeField]
        private bool _autoSaveOnApplicationLostFocus = true;
    }
}

