using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WG.Animation;

namespace WG.Point
{
    public class PointEndPoint : MonoBehaviour
    {
        [Header("Inside components")]
        [SerializeField] private Transform _model;
        [SerializeField] private Canvas _canvas;

        [Header("Settings")]
        [SerializeField] private float _modelSize;
        [SerializeField] private float _modelSizeSpeedChange;

        public void Show ()
        {
            this._model.DOScale(Vector3.one * this._modelSize, .2f);         
        }

        public void Hide ()
        {
            this._model.DOScale(Vector3.zero, .2f);
        }

        public void SetPosition(Vector3 position)
        {
            transform.DOMove(position, .1f);
        }
    }
}
