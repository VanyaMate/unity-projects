using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup _tabGroup;

    public Image Background;
    
    private void Awake()
    {
        this.Background = GetComponent<Image>();
    }

    private void Start()
    {
        this._tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this._tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this._tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this._tabGroup.OnTabExit(this);
    }
}
