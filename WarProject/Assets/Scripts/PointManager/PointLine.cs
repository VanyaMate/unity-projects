using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG.Point
{
    public class PointLine : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private PointEndPoint _endPoint;

        private void Awake()
        {
            this._lineRenderer.SetPosition(0, transform.parent.position);
            this.HideLine();
            this._endPoint.Hide();
        }

        public void ShowLine ()
        {
            this._lineRenderer.enabled = true;
            this._endPoint.Show();
        }

        public void HideLine ()
        {
            this._lineRenderer.enabled = false;
            this._endPoint.Hide();
        }

        public void SetLineEndPoint(Vector3 position)
        {
            this._lineRenderer.SetPosition(1, position);
            this._endPoint.SetPosition(position);
        }
    }
}
