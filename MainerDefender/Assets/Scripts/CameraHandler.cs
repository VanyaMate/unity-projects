using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _zoomAmount;
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _minZoom;

    private CinemachineFramingTransposer _transposer;
    private float _ortographicSize;

    private void Start()
    {
        this._transposer = this._virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFramingTransposer;
        this._ortographicSize = this._transposer.m_CameraDistance;
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, 0, z).normalized;

        transform.position += moveDir * this._moveSpeed * Time.deltaTime;

        float zoom = this._ortographicSize;

        this._ortographicSize += -Input.mouseScrollDelta.y * this._zoomAmount;

        if (this._ortographicSize > this._maxZoom || this._ortographicSize < this._minZoom)
        {
            this._ortographicSize = zoom;
        }
        else
        {
            this._transposer.m_CameraDistance = this._ortographicSize;
        }
    }
}
