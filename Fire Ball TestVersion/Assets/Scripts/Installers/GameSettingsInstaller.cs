using System;
using LevelSetup;
using Misc;
using PlayerSetup.States;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public GameInstaller.Settings gameInstaller;
        public PlayerSettings player;
        public LevelSettings level;
        public AudioHandler.Settings audioHandler;

        public override void InstallBindings()
        {
            Container.BindInstance(player.stateMoving);
            Container.BindInstance(player.stateStarting);
            Container.BindInstance(level.spawner);
            Container.BindInstance(audioHandler);
            Container.BindInstance(gameInstaller);
        }

        [Serializable]
        public class PlayerSettings
        {
            public PlayerStateMoving.Settings stateMoving;
            public PlayerStateWaitingToStart.Settings stateStarting;
        }

        [Serializable]
        public class LevelSettings
        {
            public LevelManager.Settings spawner;
        }
    }
}