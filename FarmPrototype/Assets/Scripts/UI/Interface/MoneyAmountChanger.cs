using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VM.Player;

namespace VM.UI.Interface
{
    public class MoneyAmountChanger : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void Start()
        {
            PlayerManager.Instance.moneyManager.OnMoneyChange.AddListener(this._SetMoney);
            this._SetMoney(PlayerManager.Instance.moneyManager.money);
        }

        private void _SetMoney (float amount)
        {
            this._text.text = $"${amount}";
        }
    }
}