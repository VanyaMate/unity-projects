using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG.Mouse
{
    public class MouseManager : MonoBehaviour
    {
        public static MouseManager Instance;

        private Camera _mainCamera;

        private void Awake()
        {
            Instance = this;

            this._mainCamera = Camera.main;
        }

        public RaycastHit WorldPosition => _GetMouseWorldPosition();

        private RaycastHit _GetMouseWorldPosition ()
        {
            Ray ray = this._mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit;
            }
            else
            {
                return default;
            }
        }
    }
}
