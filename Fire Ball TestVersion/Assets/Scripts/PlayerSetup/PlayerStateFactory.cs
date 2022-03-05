using ModestTree;
using PlayerSetup.States;
using UnityEngine;

namespace PlayerSetup
{
    public enum PlayerStates
    {
        Moving,
        Dead,
        WaitingToStart,
        Count
    }

    public class PlayerStateFactory
    {
        private readonly PlayerStateWaitingToStart.Factory _waitingFactory;
        private readonly PlayerStateMoving.Factory _movingFactory;
        private readonly PlayerStateDead.Factory _deadFactory;

        public PlayerStateFactory(
            PlayerStateDead.Factory deadFactory,
            PlayerStateMoving.Factory movingFactory,
            PlayerStateWaitingToStart.Factory waitingFactory)
        {
            _waitingFactory = waitingFactory;
            _movingFactory = movingFactory;
            _deadFactory = deadFactory;
        }

        public PlayerState CreateState(PlayerStates state)
        {
            return state switch
            {
                PlayerStates.Dead => _deadFactory.Create(),
                PlayerStates.WaitingToStart => _waitingFactory.Create(),
                PlayerStates.Moving => _movingFactory.Create(),
                PlayerStates.Count => null,
                _ => null
            };
        }
    }
}