using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _enemyList = new List<Transform>();
    [SerializeField] private Transform _enemyPrefab;
    [SerializeField] private Terrain _map;
    [SerializeField] private float _timeToSpawn;
    [SerializeField] private int _enemyCountInGroupStart;
    [SerializeField] private int _enemyCountInGroupFinish;

    [SerializeField] private int _padding;

    [SerializeField] private float _timeGame;

    private float _terrainWidth;
    private float _terrainLength;

    private float _halfPadding;
    private float _halfWidth;
    private float _halfLength;

    private void Start()
    {
        Vector3 terrainSize = this._map.terrainData.size;
        this._terrainWidth = terrainSize.x;
        this._terrainLength = terrainSize.z;
        
        this._halfPadding = this._padding / 2;
        this._halfWidth = this._terrainWidth / 2;
        this._halfLength = this._terrainLength / 2;

        StartCoroutine(this.SpawnEnemy());
    }

    private void Update()
    {
        this._timeGame += Time.deltaTime;
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            float nextSpawnEnemyAfter = this._timeToSpawn / (1 * (this._timeGame + this._timeToSpawn) / 60);
            Debug.Log("nextSpawnEnemyAfter" + nextSpawnEnemyAfter);
            yield return new WaitForSeconds(nextSpawnEnemyAfter);

            int groupAmount = Random.Range(this._enemyCountInGroupStart, this._enemyCountInGroupFinish);
            int xSide = Random.Range(0, 2);
            int zSide = Random.Range(0, 2);

            Vector3 spawnPosition;

            if (xSide == 0 && zSide == 0)
            {
                // top
                spawnPosition = new Vector3(
                    (int)Random.Range(this._halfPadding, this._terrainWidth - this._halfPadding) - this._halfWidth,
                    0,
                    this._halfPadding + this._halfLength
                );
            } 
            else if (xSide == 0 && zSide == 1)
            {
                // right
                spawnPosition = new Vector3(
                    this._halfWidth - this._halfPadding,
                    0,
                    (int)Random.Range(this._halfPadding, this._terrainLength - this._halfPadding) - this._halfLength
                );
            } 
            else if (xSide == 1 && zSide == 0)
            {
                // left
                spawnPosition = new Vector3(
                    this._halfPadding - this._halfWidth,
                    0,
                    (int)Random.Range(this._halfPadding, this._terrainLength - this._halfPadding) - this._halfLength
                );
            }
            else
            {
                // bottom
                spawnPosition = new Vector3(
                    (int)Random.Range(this._halfPadding, this._terrainWidth - this._halfPadding) - this._halfWidth,
                    0,
                    this._halfPadding - this._halfLength
                );
            }
            

            for (int i = 0; i < groupAmount; i++)
            {
                int xDelta = Random.Range(-10, 10);
                int zDelta = Random.Range(-10, 10);

                Transform enemy = Instantiate(this._enemyPrefab, this._map.transform);
                enemy.position = new Vector3(
                    spawnPosition.x + xDelta,
                    0,
                    spawnPosition.z + zDelta
                );
            }
        }
    }
}
