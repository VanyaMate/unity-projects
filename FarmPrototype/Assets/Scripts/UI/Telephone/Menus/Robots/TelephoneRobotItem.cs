using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VM.UI.Telephone
{
    public class TelephoneRobotItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _status;

        public void SetInfo (Sprite icon, string name, string status)
        {
            this._icon.sprite = icon;
            this._name.text = name;
            this._status.text = status;
        }
    }
}