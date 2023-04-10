using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZS
{
    public class FirstPersonCameraController : MonoBehaviour
    {
        [Header("Camera settings")]
        [SerializeField] private Camera _camera;
        [SerializeField, Range(.25f, 5), Delayed] private float _sensivity = 1;

        private void Awake()
        {
            this._camera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            Vector2 mouseInput = this._GetMouseInput() * this._sensivity;

            Vector3 playerRotation = transform.localRotation.eulerAngles;
            Vector3 cameraRotation = this._camera.transform.localRotation.eulerAngles;

            float horizontalRoration = playerRotation.y + mouseInput.x;
            float verticalRotation = this._GetVerticalRotationRestrictionAngle(cameraRotation.x - mouseInput.y);

            transform.localRotation = Quaternion.Euler(playerRotation.x, horizontalRoration, playerRotation.z);
            this._camera.transform.localRotation = Quaternion.Euler(verticalRotation, cameraRotation.y, cameraRotation.z);
        }

        private Vector2 _GetMouseInput ()
        {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        private float _GetVerticalRotationRestrictionAngle (float verticalRotation)
        {
            if (verticalRotation < 270 && verticalRotation > 210)
            {
                return 270;
            }
            else if (verticalRotation > 90 && verticalRotation <= 210)
            {
                return 90;
            }

            return verticalRotation;
        }
    }
}