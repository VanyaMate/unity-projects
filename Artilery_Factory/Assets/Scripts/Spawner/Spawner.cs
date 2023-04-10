using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnTank;
    [SerializeField] private BotSideSO _side;
    [SerializeField] private float _spawnTime;
    [SerializeField] private bool _useManualTime;

    private float _randomSpawnTime;
    private float _currentSpawnTimer;
    private Vector3 _prevTankPosition;

    private void Awake()
    {
        this._randomSpawnTime = Random.Range(30, 50);
        this._currentSpawnTimer = this._useManualTime ? this._spawnTime : this._randomSpawnTime;
    }

    private void Update()
    {
        this._currentSpawnTimer += Time.deltaTime;

        if (this._currentSpawnTimer > (this._useManualTime ? this._spawnTime : this._randomSpawnTime))
        {
            Transform tank = Instantiate(
                this._spawnTank, 
                this._prevTankPosition == transform.position ? transform.position + new Vector3(.5f, 0, 0) : transform.position, 
                Quaternion.Euler(Vector3.zero)
            );

            this._prevTankPosition = tank.position;
            tank.GetComponent<Bot>().Info = this._side;

            this._currentSpawnTimer = 0f;
        }
    }
}
