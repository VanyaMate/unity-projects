using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNodeUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _amount;

    public void SetIcon(Sprite icon)
    {
        this._icon.sprite = icon;
    }
    
    public void UpdateAmount(int amount)
    {
        this._amount.text = amount.ToString();
    }
}
