using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InvSys.Controller
{

    [RequireComponent(typeof(MoveController))]
    public class Actor : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;

        private void Awake()
        {
            if (this._isPlayer)
            {
                this.gameObject.AddComponent<InputController.InputMove>();
            }
        }
    }
}
