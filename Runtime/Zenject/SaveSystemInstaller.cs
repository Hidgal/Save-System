//#if SAVE_SYSTEM_ZENJECT
using SaveSystem.Internal;
using SaveSystem.Misc;
using UnityEngine;
using Zenject;

namespace SaveSystem.Zenject
{
    [CreateAssetMenu(menuName = "Save System/Save System Installer", fileName = "Save System Installer")]
    public class SaveSystemInstaller : ScriptableObjectInstaller<SaveSystemInstaller>
    {
        public SaveSystemSettings Settings => _settings;

        [SerializeField]
        private SaveSystemSettings _settings;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SaveSystemLogic>().AsSingle().WithArguments(_settings);
        }

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
//#endif

