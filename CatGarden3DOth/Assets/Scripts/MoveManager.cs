using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace CG.Managers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveManager : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private bool _moveActive;

        public UnityEvent onMoveStart;
        public UnityEvent onMoveEnd;

        public bool MoveActive => _moveActive;
        public bool Moved => this._navMeshAgent.velocity != Vector3.zero;

        public float Speed
        {
            get
            {
                return this._navMeshAgent.speed;
            }
            set
            {
                this._navMeshAgent.speed = value;
            }
        }

        private void Awake()
        {
            this._moveActive = false;
            this._navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (this._moveActive && !this.Moved)
            {
                this._moveActive = false;
                this.onMoveEnd.Invoke();
            }
            else if (!this._moveActive && this.Moved)
            {
                this._moveActive = true;
                this.onMoveStart.Invoke();
            }
        }

        public bool MoveToPoint(Vector3 point, float speed)
        {
            if (point != null)
            {
                this._navMeshAgent.speed = speed;
                this._navMeshAgent.SetDestination(point);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetVelocity(Vector3 direction, float speed)
        {
            this._navMeshAgent.ResetPath();
            this._navMeshAgent.velocity = direction * speed;
        }
    }
}