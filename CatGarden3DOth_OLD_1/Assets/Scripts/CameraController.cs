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
        [SerializeField] private float _length;
        [SerializeField] private float _height;

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
                transform.position = Vector3.Lerp(
                    transform.position,
                    new Vector3(
                        this._cameraTarget.position.x + this._length / 2,
                        this._cameraTarget.position.y + this._height,
                        this._cameraTarget.position.z + this._length / 2
                    ),
                    Time.deltaTime * 5
                );
            }
        }

        public void SetLookAt(Transform target)
        {
            this._cinemachine.LookAt = this._cameraTarget = target;
        }
    }
}
