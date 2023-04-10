using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InvSys.Controller
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _nma;
        [SerializeField] private float _speed;

        private void Awake()
        {
            if (TryGetComponent<NavMeshAgent>(out this._nma))
            {
                this._nma.speed = this._speed;
            }
        }

        public void MoveToPoint (Vector3 point)
        {
            if (this._nma)
            {
                this._nma.SetDestination(point);
            }
        }

        public void MoveTo (Vector3 vector)
        {
            if (this._nma)
            {
                this._nma.velocity = vector * this._speed;
            }
        }

        public void SetSpeed (float speed)
        {
            this._nma.speed = this._speed = speed;
        }
    }
}