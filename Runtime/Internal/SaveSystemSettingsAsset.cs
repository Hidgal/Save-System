using SaveSystem.Misc;
using UnityEngine;

namespace SaveSystem.Internal
{
    [CreateAssetMenu(menuName = "Save System/Settings Asset", fileName = "Save System Settings Asset")]
    public class SaveSystemSettingsAsset : ScriptableObject
    {
        [SerializeField]
        private SaveSystemSettings _settings;

        public SaveSystemSettings Settings => _settings;

//TODO: replace this block to editor assembly
#if UNITY_EDITOR
        private void Reset()
        {
            if (_settings == null)
                _settings = new();

            if (_settings.EncryptionIv == null || _settings.EncryptionIv.Length == 0
                || _settings.EncryptionKey == null || _settings.EncryptionKey.Length == 0)
            {
                _settings.GenerateKeys();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }

        [ContextMenu("Generate Keys For Project")]
        private void GenerateEncryptKeysForProject()
        {
            _settings.GenerateKeys(false);
            UnityEditor.EditorUtility.SetDirty(this);
        } 
#endif
    }
}
