using System;
using UnityEngine;
using Util;
using Zenject;

namespace Misc
{
    public class AudioHandler : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly Settings _settings;
        private readonly AudioSource _nominalAudioSource;
        private readonly AudioSource _effectAudioSource;

        public AudioHandler(
            [Inject(Id = "NominalSource")] AudioSource nominalAudioSource, 
            [Inject(Id = "EffectSource")] AudioSource effectAudioSource, 
            Settings settings, 
            SignalBus signalBus)
        {
            _signalBus = signalBus;
            _settings = settings;
            _nominalAudioSource = nominalAudioSource;
            _effectAudioSource = effectAudioSource;
        }

        public void Initialize()
        {
            _nominalAudioSource.loop = true;
            _nominalAudioSource.playOnAwake = true;
            _nominalAudioSource.clip = _settings.loopSound;
            _nominalAudioSource.Play();
            _signalBus.Subscribe<PlayerCrashedSignal>(OnPlayerCrashed);
            _signalBus.Subscribe<PlayerGetsBonusSignal>(OnPlayerGetsBonus);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerCrashedSignal>(OnPlayerCrashed);
            _signalBus.Unsubscribe<PlayerGetsBonusSignal>(OnPlayerGetsBonus);
        }

        private void OnPlayerCrashed()
        {
            _nominalAudioSource.Stop();
            _nominalAudioSource.loop = false;
            _nominalAudioSource.playOnAwake = false;
            _nominalAudioSource.clip = null;
            
            _nominalAudioSource.PlayOneShot(_settings.crashSound);
        }

        private void OnPlayerGetsBonus()
        {
            _effectAudioSource.PlayOneShot(_settings.bonusSound);
        }
        
        [Serializable]
        public class Settings
        {
            public AudioClip loopSound;
            public AudioClip crashSound;
            public AudioClip bonusSound;
        }
    }
}