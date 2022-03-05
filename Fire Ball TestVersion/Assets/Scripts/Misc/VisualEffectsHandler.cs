using System;
using PlayerSetup;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Zenject;
using Random = UnityEngine.Random;

namespace Misc
{
    public class VisualEffectsHandler : IInitializable, IDisposable
    {
        private readonly Player _player;
        private readonly Text _text;
        private readonly Animator _animator;
        private readonly SignalBus _signalBus;

        private readonly string[] _victoryTexts = {
            "Very good!",
            "Fascinating!",
            "Well done!",
            "Crazy!"
        };
        
        public VisualEffectsHandler(
            Player player, 
            [Inject(Id = "BonusText")] Text text, 
            [Inject(Id = "BonusText")] Animator animator, 
            SignalBus signalBus)
        {
            _player = player;
            _text = text;
            _animator = animator;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerGetsBonusSignal>(OnPlayerGetsBonus);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerGetsBonusSignal>(OnPlayerGetsBonus);
        }

        private void OnPlayerGetsBonus()
        {
            _text.text = $"+100\n{_victoryTexts[Random.Range(0, _victoryTexts.Length - 1)]}";
            _player.RunCoroutine(_animator.CreateBonusEffect());
        }
    }
}