using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _speed;

    private float _currentZoom;
    private float _targetZoom;

    private void Awake()
    {
        this._currentZoom = this._targetZoom = this._cinemachineVirtualCamera.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        Vector3 moveVector = Vector3.zero;

        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");

        transform.position += moveVector.normalized * this._speed * Time.deltaTime;

        this._targetZoom -= Input.mouseScrollDelta.y;

        if (this._targetZoom > this._maxZoom)
        {
            this._targetZoom = this._maxZoom;
        }
        else if (this._targetZoom < this._minZoom)
        {
            this._targetZoom = this._minZoom;
        }

        this._currentZoom = Mathf.Lerp(this._currentZoom, this._targetZoom, this._zoomSpeed * Time.deltaTime);

        this._cinemachineVirtualCamera.m_Lens.OrthographicSize = this._currentZoom;
    }
}
