using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.Managers;

namespace CG.Cat
{
    [Serializable]
    class MoveSettings
    {
        public float WalkMoveSpeed;
        public float RunMoveSpeed;
    }

    [RequireComponent(typeof(MoveManager))]
    public class CatMoveManager : MonoBehaviour
    {
        [SerializeField] private MoveSettings _moveSettings;
        [SerializeField] private MoveManager _moveManager;
        [SerializeField] private Animator _animator;

        private bool _running;

        public bool Moved => this._moveManager.Moved;

        private void Awake()
        {
            this._moveManager = GetComponent<MoveManager>();
            this._moveManager.onMoveStart.AddListener(() =>
            {
                this._animator.SetBool("Walk", true);
                this._animator.SetBool("Run", this._running);
            });

            this._moveManager.onMoveEnd.AddListener(() =>
            {
                this._animator.SetBool("Walk", false);
                this._animator.SetBool("Run", false);
                this._running = false;
            });
        }

        private void Update()
        {
            
        }

        public void MoveToPoint(Vector3 point, bool run = false)
        {
            this._running = run;
            this._moveManager.MoveToPoint(point, run ? this._moveSettings.RunMoveSpeed : this._moveSettings.WalkMoveSpeed);

            this._animator.SetBool("Walk", true);
            this._animator.SetBool("Run", this._running);
        }

        public void SetVelocity(Vector3 direction, bool run = false)
        {
            this._running = run;
            this._moveManager.SetVelocity(direction, run ? this._moveSettings.RunMoveSpeed : this._moveSettings.WalkMoveSpeed);
        }

        public void Stop()
        {
            this._moveManager.SetVelocity(Vector3.zero, 0f);
        }
    }
}
