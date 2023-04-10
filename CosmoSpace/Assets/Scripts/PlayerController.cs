using CS.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ActorUnit _controllableUnit;
        [SerializeField] private Camera _camera;

        private void Awake()
        {
            this._camera = Camera.main;
        }

        private void Update()
        {
            Vector3 cameraSide = (transform.position - this._camera.transform.position).normalized;
            Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            moveDirection *= Time.deltaTime * this._controllableUnit.move.runSpeed;

            this._controllableUnit.move.moveComponent.MoveTo(moveDirection);
            this._controllableUnit.move.moveComponent.RotateTo(moveDirection);
        }
    }
}