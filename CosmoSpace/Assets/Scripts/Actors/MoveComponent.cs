using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;

        private void Awake()
        {
            TryGetComponent<Rigidbody>(out _rb);   
        }

        public void ForceTo (Vector3 direction, Vector3 point = default)
        {
            if (point == null)
            {
                this._rb.AddForce(direction, ForceMode.Impulse);
            }
            else
            {
                this._rb.AddForceAtPosition(direction, point, ForceMode.Impulse);
            }
        }

        public void MoveTo (Vector3 direction)
        {
            transform.Translate(direction, Space.World);
        }

        public void RotateTo (Vector3 direction)
        {
            transform.forward = direction;
        }
    }
}