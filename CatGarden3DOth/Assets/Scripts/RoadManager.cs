using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.Road
{
    [Serializable]
    class RoadPoint
    {
        public Transform StartPoint;
        public Transform EndPoint;
    }

    public class RoadManager : MonoBehaviour
    {
        [Header("Spawner")]
        [SerializeField] private float _carSpawnTime;
        [SerializeField] private Transform _carPrefab;

        [Header("Line")]
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _lineWidth;
        [SerializeField] private float _lineSpace;

        [Header("Start Points")]
        [SerializeField] private List<RoadPoint> _points = new List<RoadPoint>();

        private IEnumerator _carSpawnTimer;

        private void Awake()
        {
            // White line render
            Vector3[] points = { this._startPoint.position, this._endPoint.position }; 
            
            for (int i = 0; i < points.Length; i++)
            {
                this._lineRenderer.SetPosition(i, points[i] + new Vector3(0, 0.01f, 0));
            }

            // Execute car spawner
            StartCoroutine(this._carSpawnTimer = this.CarSpawner());
        }

        private IEnumerator CarSpawner ()
        {
            while(true)
            {
                yield return new WaitForSeconds(this._carSpawnTime);

                // Debug.Log("Spawn Car");
            }
        }
    }
}
