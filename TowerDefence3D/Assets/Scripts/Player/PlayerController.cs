using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(InputController))]
    public class PlayerController : Controller
    {
        public static PlayerController Instance;

        [SerializeField] private InputController _controller;
        [SerializeField] private Camera _mainCamera;

        private void Awake()
        {
            Instance = this;
            this._controller = GetComponent<InputController>();
            this._mainCamera = Camera.main;
        }

        private void Update()
        {
            this._mainCamera.transform.position = new Vector3(transform.position.x, 15, transform.position.z -15);
        }
    }
}
