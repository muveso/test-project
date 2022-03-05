using System;
using ModestTree;
using PlayerSetup;
using UnityEngine;
using Util;
using Zenject;

namespace Main
{
    public enum GameStates
    {
        WaitingToStart,
        Playing,
        GameOver
    }

    public class GameController : IInitializable, ITickable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly Player _player;

        public GameController(Player player, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _player = player;
            _player.GameController = this;
        }

        public float HighestPoint
        {
            get
            {
                var h = PlayerPrefs.GetFloat("HighestPoint", 0);
                if (TotalPoints > h) h = TotalPoints;
                return h;
            }
            set => PlayerPrefs.GetFloat("HighestPoint", value);
        }
        
        public float TotalPoints { get; set; }
        
        public GameStates State { get; private set; } = GameStates.WaitingToStart;

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerCrashedSignal>(OnPlayerCrashed);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerCrashedSignal>(OnPlayerCrashed);
        }

        public void Tick()
        {
            switch (State)
            {
                case GameStates.WaitingToStart:
                {
                    UpdateStarting();
                    break;
                }
                case GameStates.Playing:
                {
                    UpdatePlaying();
                    break;
                }
                case GameStates.GameOver:
                {
                    UpdateGameOver();
                    break;
                }
                default:
                {
                    Assert.That(false);
                    break;
                }
            }
        }

        private void UpdateGameOver()
        {
            Assert.That(State == GameStates.GameOver);

            if (Input.GetMouseButtonDown(0)) StartGame();
        }

        private void OnPlayerCrashed()
        {
            Assert.That(State == GameStates.Playing);
            State = GameStates.GameOver;
        }

        private void UpdatePlaying()
        {
            Assert.That(State == GameStates.Playing);
        }

        private void UpdateStarting()
        {
            Assert.That(State == GameStates.WaitingToStart);
            if (Input.GetMouseButtonDown(0)) StartGame();
        }

        private void StartGame()
        {
            Assert.That(State == GameStates.WaitingToStart || State == GameStates.GameOver);

            _player.ChangeState(PlayerStates.Moving);
            State = GameStates.Playing;
        }
    }
}