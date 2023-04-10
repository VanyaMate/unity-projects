using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZS
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Range(.5f, 3)] private float _speed = 1f;

        private CharacterController _cc;


        private void Awake()
        {
            this._cc = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Vector3 keyboardInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        }
    }
}