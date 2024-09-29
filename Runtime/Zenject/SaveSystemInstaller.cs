#if ZENJECT_INSTALLED
using SaveSystem.Internal;
using SaveSystem.Internal.Settings;
using UnityEngine;
using Zenject;

namespace SaveSystem.Zenject
{
    [CreateAssetMenu(menuName = "Save System/Save System Installer", fileName = "Save System Installer")]
    public class SaveSystemInstaller : ScriptableObjectInstaller<SaveSystemInstaller>
    {
        [SerializeField]
        private SaveSystemSettingsAsset _settingsAsset;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SaveSystemLogic>().AsSingle().WithArguments(_settingsAsset.Settings);
        }
    }
}
#endif