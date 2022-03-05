using System;
using System.Globalization;
using Main;
using ModestTree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;
using Zenject;

#pragma warning disable 649

namespace Misc
{
    public class GUIHandler : MonoBehaviour, IDisposable, IInitializable
    {
        [SerializeField] private Text scoreOutput, lastScoreOutput, highestScoreOutput;
        [SerializeField] private GameObject holdToStart, gameOverPanel;

        private GameController _gameController;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(GameController gameController, SignalBus signalBus)
        {
            _gameController = gameController;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerCrashedSignal>(OnPlayerCrashed);
        }

        protected virtual void OnPlayerCrashed()
        {
            Debug.LogError("carptik be tuh..");
        }

        private void Update()
        {
            switch (_gameController.State)
            {
                case GameStates.WaitingToStart:
                {
                    StartGUI();
                    break;
                }
                case GameStates.Playing:
                {
                    PlayingGUI();
                    break;
                }
                case GameStates.GameOver:
                {
                    PlayingGUI();
                    GameOverGUI();
                    break;
                }
                default:
                {
                    Assert.That(false);
                    break;
                }
            }
        }

        private void StartGUI()
        {
            _gameController.TotalPoints = 0;
            scoreOutput.gameObject.SetActive(true);
            holdToStart.SetActive(true);
            gameOverPanel.SetActive(false);
            var score = Mathf.RoundToInt(_gameController.TotalPoints);
            score = score < 0 ? 0 : score;
            scoreOutput.text = $"Score: {score.ToString(CultureInfo.InvariantCulture)}";
        }

        private void PlayingGUI()
        {
            holdToStart.SetActive(false);
            var score = Mathf.RoundToInt(_gameController.TotalPoints);
            score = score < 0 ? 0 : score;
            scoreOutput.text = $"Score: {score.ToString(CultureInfo.InvariantCulture)}";
        }

        private void GameOverGUI()
        {
            var score = Mathf.RoundToInt(_gameController.TotalPoints);
            score = score < 0 ? 0 : score;
            
            var highestScore = Mathf.RoundToInt(_gameController.HighestPoint);
            highestScore = highestScore < 0 ? 0 : highestScore;
            
            lastScoreOutput.text = $"Your score: {score.ToString(CultureInfo.InvariantCulture)}";
            highestScoreOutput.text = $"Highest score: {highestScore.ToString(CultureInfo.InvariantCulture)}";
            scoreOutput.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
        }

        public void OnRestartButtonClicked()
        {
            SceneManager.LoadScene(0);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerCrashedSignal>(OnPlayerCrashed);
        }
    }
}