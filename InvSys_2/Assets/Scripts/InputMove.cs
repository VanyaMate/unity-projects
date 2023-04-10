using InvSys.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InvSys.InputController
{
    public class InputMove : MonoBehaviour
    {
        [SerializeField] private MoveController _moveController;

        private void OnEnable()
        {
            this._moveController = GetComponent<MoveController>();
        }

        private void Update()
        {
            if (this._moveController)
            {
                Vector3 vectorMove = Vector3.zero;

                if (Input.GetKey(KeyCode.W))
                {
                    vectorMove += Vector3.forward;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    vectorMove += Vector3.left;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    vectorMove += Vector3.back;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    vectorMove += Vector3.right;
                }

                this._moveController.MoveTo(vectorMove.normalized);
            }
        }
    }
}