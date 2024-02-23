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
    }
}

