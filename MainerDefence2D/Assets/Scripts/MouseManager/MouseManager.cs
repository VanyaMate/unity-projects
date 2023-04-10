using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private Transform _debugMark;

    private Vector3 _lastMousePosition;
    private Camera _mainCamera;

    private void Awake()
    {
        this._mainCamera = Camera.main;
        this._lastMousePosition = Vector3.zero;
    }

    private void Update()
    {
        Debug.Log(this.GetMouseWorldPoint());
    }

    public Vector3 GetMouseWorldPoint ()
    {
        Vector3 position = this._mainCamera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        return position;
    }
}
