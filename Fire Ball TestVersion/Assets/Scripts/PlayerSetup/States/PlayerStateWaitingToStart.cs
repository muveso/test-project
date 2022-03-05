using System;
using UnityEngine;
using Zenject;

namespace PlayerSetup.States
{
    public class PlayerStateWaitingToStart : PlayerState
    {
        private readonly Settings _settings;
        private readonly Player _player;
        private bool _firstRopeConnected = false;

        public PlayerStateWaitingToStart(Settings settings, Player player)
        {
            _settings = settings;
            _player = player;
        }

        public override void Start()
        {
            _player.Line = _player.GetComponent<LineRenderer>();
            _player.Rigid = _player.GetComponent<Rigidbody>();
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
            if (!_firstRopeConnected)
                ConnectFirstRope();
            else
                MakeIdleAnimation();
        }

        private void ConnectFirstRope()
        {
            if (_firstRopeConnected) return;

            _player.Rigid.useGravity = false;
            _player.Rigid.mass = .1f;

            var t = _player.transform;
            Physics.Raycast(t.position, t.up, out var hit);

            if (hit.collider == null) return;

            var firstRope = _player.gameObject.AddComponent<SpringJoint>();
            firstRope.autoConfigureConnectedAnchor = false;
            firstRope.damper = 30f;
            firstRope.enableCollision = true;
            firstRope.connectedAnchor = hit.point;
            firstRope.spring = 1.5f;

            _player.Rope = firstRope;
            _firstRopeConnected = true;
        }

        private void MakeIdleAnimation()
        {
            var pingPong = Mathf.PingPong(Time.time * _settings.swingSpeed, _settings.swingDistance);
            var distance = pingPong - _settings.swingDistance / 2;
            var t = _player.transform;

            _player.Rigid.velocity = distance * Vector3.Cross(t.right, t.up);
        }

        [Serializable]
        public class Settings
        {
            public float swingSpeed = 10;
            public float swingDistance = 16;
        }

        public class Factory : PlaceholderFactory<PlayerStateWaitingToStart>
        {
        }
    }
}