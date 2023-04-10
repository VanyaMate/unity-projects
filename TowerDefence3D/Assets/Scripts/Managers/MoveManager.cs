using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Managers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveManager : MonoBehaviour
    {
        private Rigidbody _rb;
        private NavMeshAgent _navMeshAgent;

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
            this._rb = GetComponent<Rigidbody>();
            this._navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Vector3 direction)
        {
            this._rb.velocity = direction;
        }
        
        public void MoveToPoint(Vector3 point, float speed)
        {
            this._navMeshAgent.speed = speed;
            this._navMeshAgent.SetDestination(point);
        }

        public void DieAction()
        {
            this._rb.freezeRotation = false;
            this._navMeshAgent.isStopped = true;
        }
    }
}
