using System;
using UnityEngine;

namespace PlayerSetup.States
{
    public abstract class PlayerState : IDisposable
    {
        public abstract void Update();
        public abstract void FixedUpdate();

        public virtual void Start()
        {
        }

        public virtual void Dispose()
        {
        }

        public virtual void OnTriggerEnter(Collider other)
        {
        }

        public virtual void OnCollisionEnter(Collision other)
        {
        }
    }
}