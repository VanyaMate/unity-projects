using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VM.UI.Inventory
{
    public class HandsItemButtonUI : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;

        public void Set (HandsItemInfo info)
        {
            this._text.text = info.text;
            this._button.onClick.AddListener(info.action);
        }
    }
}