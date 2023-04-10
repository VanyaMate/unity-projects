using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HealthUnitUI : MonoBehaviour
{
    [SerializeField] private Transform _background;
    [SerializeField] private Transform _progress;

    private Camera _mainCamera;
    private RectTransform _progressRectTransform;
    
    private void Awake()
    {
        this._mainCamera = Camera.main;
        this._progressRectTransform = this._progress.GetComponent<RectTransform>();
    }

    private void Update()
    {
        /*this._background.position = Vector3.MoveTowards(this._mainCamera.transform.position, transform.position, 15);
        this._background.rotation =
            Quaternion.LookRotation(
                new Vector3(
                    this._mainCamera.transform.position.x - transform.position.x, 
                    this._mainCamera.transform.position.y - transform.position.y,
                    this._mainCamera.transform.position.z - transform.position.z
                )
            );*/
    }

    public void UpdateProgress(float percent)
    {
        this._progressRectTransform.sizeDelta = new Vector2(
            percent / 100,
            this._progressRectTransform.sizeDelta.y
        );
    }
}
