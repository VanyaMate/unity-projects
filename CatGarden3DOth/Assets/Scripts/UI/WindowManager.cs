using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private TMP_Text _windowName;
    [SerializeField] private Button _closeButton;

    public UnityEvent OnDestroyEvent;

    private void Awake()
    {
        this._closeButton.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    public void SetName (string windowName)
    {
        this._name = windowName;
        this._windowName.text = this._name;
        gameObject.name = this._name;
    }


    public void DragHandler(BaseEventData data)
    {
        ((RectTransform)transform).anchoredPosition += ((PointerEventData)data).delta;
    }

    private void OnDestroy()
    {
        this.OnDestroyEvent?.Invoke();
    }
}
