using LabyTanks.Game;
using LabyTanks.Network;
using UnityEngine;
using Zenject;

namespace LabyTanks.Zenject.Installers.Mono
{
    public class ProjectMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<LifecycleBehaviour>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName(nameof(LifecycleBehaviour))
                .AsSingle()
                .NonLazy();
        }
    }
}
