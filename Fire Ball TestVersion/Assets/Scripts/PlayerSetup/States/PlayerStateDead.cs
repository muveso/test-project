using UnityEngine;
using Util;
using Zenject;

namespace PlayerSetup.States
{
    public class PlayerStateDead : PlayerState
    {
        private readonly SignalBus _signalBus;
        private readonly Player _player;

        public PlayerStateDead(Player player, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _player = player;
        }

        public override void Start()
        {
            _signalBus.Fire<PlayerCrashedSignal>();
            _player.Rigid.constraints = RigidbodyConstraints.FreezeAll;
        }

        public override void Dispose()
        {
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
        }

        public class Factory : PlaceholderFactory<PlayerStateDead>
        {
        }
    }
}