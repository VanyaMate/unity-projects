using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InvSys.Controller
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private float _changePositionSpeed;
        [SerializeField] private float _cameraDistance;

        private void Update()
        {
            if (this._target)
            {
                this.ChangePosition(this._target.transform.position);
            }
        }

        private void ChangePosition (Vector3 position)
        {
            Vector3 targetCameraPosition = new Vector3(
                position.x + 0,
                position.y + 7,
                position.z - this._cameraDistance
            );

            this.transform.DOMove(targetCameraPosition, this._changePositionSpeed);
        }

        private void Rotate (Quaternion rotation)
        {
            
        }

        private void ChangeFov ()
        {

        }

        private void Scroll ()
        {

        }
    }
}
