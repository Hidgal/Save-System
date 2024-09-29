using SaveSystem.Internal;
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

