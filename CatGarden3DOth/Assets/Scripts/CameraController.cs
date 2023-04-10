using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.Camera
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private CinemachineVirtualCamera _cinemachine;

        [Header("Camera Distance")]
        [SerializeField] private float _maxSteps;
        [SerializeField] private float _step;
        [SerializeField] private float _minLength;
        [SerializeField] private float _minHeight;
        [SerializeField] private float _maxLength;
        [SerializeField] private float _maxHeight;

        private void Awake()
        {
            Instance = this;

            if (this._cameraTarget != null)
            {
                this.SetLookAt(this._cameraTarget);
            }
        }

        private void Update()
        {
            if (this._cameraTarget)
            {
                float length = this._minLength + this._maxLength / this._maxSteps * this._step;
                float height = this._minHeight + this._maxHeight / this._maxSteps * this._step;

                transform.position = Vector3.Lerp(
                    transform.position,
                    new Vector3(
                        this._cameraTarget.position.x + length / 2,
                        this._cameraTarget.position.y + height,
                        this._cameraTarget.position.z + length / 2
                    ),
                    Time.deltaTime * 5
                );

                this._step -= Input.mouseScrollDelta.y;
                
                if (this._step <= 1)
                {
                    this._step = 1;
                }
                else if (this._step >= this._maxSteps)
                {
                    this._step = this._maxSteps;
                }
            }
        }

        public void SetLookAt(Transform target)
        {
            this._cinemachine.LookAt = this._cameraTarget = target;
        }
    }
}
