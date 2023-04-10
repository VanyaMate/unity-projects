using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.UI.Telephone
{
    public class TelephoneManager : MonoBehaviour
    {
        [SerializeField] private Telephone _telephone;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                this._telephone.gameObject.SetActive(!this._telephone.gameObject.activeSelf);
            }
        }
    }
}