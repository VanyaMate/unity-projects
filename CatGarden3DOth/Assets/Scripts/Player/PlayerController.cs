using CG.Camera;
using CG.Cat;
using CG.Garden;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.Player
{
    [RequireComponent(typeof(PlayerMoveManager))]
    [RequireComponent(typeof(PlayerGardenInteract))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTarget;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CameraController.Instance.SetLookAt(this._cameraTarget);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Transform hit = Common.Utils.GetMouseWorldHit().transform;
                Debug.Log(hit.name);

                if (hit.name == "CatSelect")
                {
                    CameraController.Instance.SetLookAt(hit.parent.GetComponent<CatController>().CameraTarget);
                }
                else if (hit.name == "GardenSelect")
                {
                    // CameraController.Instance.SetLookAt(hit.parent.GetComponent<GardenPoint>().transform);
                }
            }
        }
    }
}
