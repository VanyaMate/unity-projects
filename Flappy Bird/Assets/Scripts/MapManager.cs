using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FB.Spawner
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;

        [Header("Background")]
        [SerializeField] private Sprite _backgroundTexture;

        [Header("Move speed")]
        [SerializeField] private float _startMoveSpeed;

        [Header("Spawner")]
        [SerializeField] private Transform _spawner;
        [SerializeField] private float _spawnTime;
        [SerializeField] private float _minPosition;
        [SerializeField] private float _maxPosition;

        [Header("Columns")]
        [SerializeField] private ColumnManager _column;
        [SerializeField] private Sprite _columnTexture;
        [SerializeField] private float _columnWidth;
        [SerializeField] private float _startColumnSpeed;
        [SerializeField] private float _speedAcceleration;
        [SerializeField] private float _minHeightHole;
        [SerializeField] private float _maxHeightHole;

        private List<ColumnManager> _columnList = new List<ColumnManager>();
        private IEnumerator _spawnerCoroutine;

        public List<ColumnManager> ColumnList => _columnList;

        private void Awake()
        {
            Instance = this;
            StartCoroutine(this._spawnerCoroutine = this.Spawner());
        }

        private IEnumerator Spawner ()
        {
            while(true)
            {
                yield return new WaitForSeconds(this._spawnTime);

                ColumnManager column = Instantiate(this._column, this._spawner);

                column.transform.position = this._spawner.position;
                
                column.SetData(
                    new ColumnData()
                    {
                        Height = this.GetRandomHeightHole(),
                        Position = this.GetRandomPosition(),
                        Sprite = this._columnTexture,
                        Speed = this._startColumnSpeed
                    }
                );

                this._columnList.Add(column);
            }
        }

        private float GetRandomHeightHole ()
        {
            return Random.Range(this._minHeightHole, this._maxHeightHole);
        }

        private Vector2 GetRandomPosition ()
        {
            return new Vector2(0, Random.Range(this._minPosition, this._maxPosition));
        }

        public void StopGame()
        {
            StopCoroutine(this._spawnerCoroutine);

            this._columnList.ForEach((ColumnManager x) => x.Moving = false);
        }
    }
}
