using System;
using LevelSetup;
using ModestTree;
using UnityEngine;
using Util;
using Zenject;

namespace PlayerSetup.States
{
    public class PlayerStateMoving : PlayerState
    {
        private readonly SignalBus _signalBus;
        private readonly Settings _settings;
        private readonly Player _player;
        private RaycastHit _aim;
        private RaycastHit _hit;

        public PlayerStateMoving(Settings settings, Player player, SignalBus signalBus)
        {
            _player = player;
            _signalBus = signalBus;
            _settings = settings;
        }

        public override void Start()
        {
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var t = _player.transform;

                Physics.Raycast(t.position, t.forward + t.up, out _aim, Mathf.Infinity);

                if (_aim.transform != null)
                    MakeSlingEffect(_aim);
            }
            else if (Input.GetMouseButton(0) && _player.Rope != null)
            {
                ApplyBallForce();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _player.Remove(_player.Rope);
                _player.Rigid.useGravity = true;
                _player.Rigid.drag = .5f;
            }
        }

        private void MakeSlingEffect(RaycastHit aim)
        {
            var position = aim.transform.position;
            var targetPos = new Vector3(position.x, position.y - aim.transform.localScale.y / 2, position.z);
            var pos = _player.transform.position;

            var directionOfRope = targetPos - pos;
            Physics.Raycast(pos, directionOfRope, out _hit, Mathf.Infinity);

            if (_hit.collider == null) return;

            _player.Rigid.useGravity = false;
            _player.Rigid.mass = 0.1f;
            _player.Rigid.drag = 0f;

            ModifyNewRope(_hit);
        }

        private void ModifyNewRope(RaycastHit hit)
        {
            var newRope = _player.gameObject.AddComponent<SpringJoint>();
            newRope.autoConfigureConnectedAnchor = false;
            newRope.spring = 4.5f;
            newRope.damper = 25f;
            newRope.enableCollision = true;
            newRope.connectedAnchor = hit.point;

            _player.Remove(_player.Rope);
            _player.Rope = newRope;
        }

        private void ApplyBallForce()
        {
            var t = _player.transform;
            var dir = Vector3.Cross(-t.right, (t.position - _player.Rope.connectedAnchor).normalized);
            _player.Rigid.AddForce(dir * (_settings.ballSpeed * Time.deltaTime));
        }

        public override void FixedUpdate()
        {
            _player.AddPoints();
        }

        public override void Dispose()
        {
        }

        public override void OnCollisionEnter(Collision other)
        {
            _player.ChangeState(PlayerStates.Dead);
        }

        public override void OnTriggerEnter(Collider other)
        {
            switch (other.name)
            {
                case "2Point":
                    _signalBus.Fire<PlayerGetsBonusSignal>();
                    _player.AddPoints(100);
                    break;
                case "1Point":
                    _signalBus.Fire<PlayerGetsBonusSignal>();
                    _player.AddPoints(50);
                    break;
            }

            foreach (var c in other.transform.parent.GetComponentsInChildren<Collider>()) c.enabled = false;
            _player.RunCoroutine(other.transform.StartAnimation());
        }


        [Serializable]
        public class Settings
        {
            public float ballSpeed = 125f;
        }

        public class Factory : PlaceholderFactory<PlayerStateMoving>
        {
        }
    }
}