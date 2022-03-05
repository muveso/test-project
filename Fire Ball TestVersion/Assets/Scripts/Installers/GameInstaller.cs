using System;
using LevelSetup;
using Main;
using Misc;
using PlayerSetup;
using PlayerSetup.States;
using UnityEngine;
using Util;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [Inject] private Settings _settings;
        
        public override void InstallBindings()
        {
            InstallLevel();
            InstallPlayer();
            InstallMisc();
            InstallSignals();

            InstallExecutionOrder();
        }

        private void InstallLevel()
        {
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
            
            Container.BindFactory<Level, Level.Factory>()
                .FromComponentInNewPrefab(_settings.levelPrefab)
                .WithGameObjectName("Level")
                .UnderTransformGroup("Level Pool Holder");
        }

        private void InstallPlayer()
        {
            Container.Bind<PlayerStateFactory>().AsSingle();

            Container.BindFactory<PlayerStateWaitingToStart, PlayerStateWaitingToStart.Factory>()
                .WhenInjectedInto<PlayerStateFactory>();

            Container.BindFactory<PlayerStateMoving, PlayerStateMoving.Factory>()
                .WhenInjectedInto<PlayerStateFactory>();

            Container.BindFactory<PlayerStateDead, PlayerStateDead.Factory>()
                .WhenInjectedInto<PlayerStateFactory>();
        }

        private void InstallMisc()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.BindInterfacesTo<AudioHandler>().AsSingle();
            Container.BindInterfacesTo<VisualEffectsHandler>().AsSingle();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<PlayerCrashedSignal>();
            Container.DeclareSignal<PlayerGetsBonusSignal>();
        }


        private void InstallExecutionOrder()
        {
            Container.BindExecutionOrder<LevelManager>(-20);
            Container.BindExecutionOrder<GameController>(-10);
        }
        
        [Serializable]
        public class Settings
        {
            public GameObject levelPrefab;
        }
    }
}