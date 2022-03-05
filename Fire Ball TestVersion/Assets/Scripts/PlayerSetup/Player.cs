using System.Collections;
using Main;
using PlayerSetup.States;
using UnityEngine;
using Zenject;

#pragma warning disable 649
#pragma warning disable 618

namespace PlayerSetup
{
    public class Player : MonoBehaviour
    {
        [SerializeField, HideInInspector] private SpringJoint rope;
        [SerializeField, HideInInspector] private LineRenderer line;
        [SerializeField, HideInInspector] private Rigidbody rigid;

        private PlayerStateFactory _stateFactory;
        public GameController GameController;
        private PlayerState _state;

        [Inject]
        public void Construct(PlayerStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public SpringJoint Rope
        {
            get => rope;
            set => rope = value;
        }

        public LineRenderer Line
        {
            get => line;
            set => line = value;
        }

        public Rigidbody Rigid
        {
            get => rigid;
            set => rigid = value;
        }

        public void Start()
        {
            ChangeState(PlayerStates.WaitingToStart);
        }

        public void Update()
        {
            _state.Update();
        }

        public void FixedUpdate()
        {
            _state.FixedUpdate();
        }

        public void AddPoints()
        {
            AddPoints(Rigid.velocity.z * Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if (Rope != null)
            {
                Line.enabled = true;
                Line.positionCount = 2;
                Line.SetPosition(0, transform.position);
                Line.SetPosition(1, Rope.connectedAnchor);
            }
            else
            {
                Line.enabled = false;
            }
        }

        public void RunCoroutine(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }

        public void AddPoints(float point)
        {
            GameController.TotalPoints += point;
        }

        public void Remove(Object obj)
        {
            DestroyImmediate(obj);
        }

        private void OnCollisionEnter(Collision other)
        {
            _state.OnCollisionEnter(other);
        }

        public void OnTriggerEnter(Collider other)
        {
            _state.OnTriggerEnter(other);
        }

        public void ChangeState(PlayerStates state)
        {
            if (_state != null)
            {
                _state.Dispose();
                _state = null;
            }

            _state = _stateFactory.CreateState(state);
            _state.Start();
        }
    }
}