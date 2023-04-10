using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FB.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Jump")]
        [SerializeField] private float _jumpForcePower;

        private Rigidbody2D _rb;

        private void Awake()
        {
            this._rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._rb.AddForce(Vector2.up * this._jumpForcePower, ForceMode2D.Impulse);
            }
        }
    }
}
