using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTurrelAim : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private float _turrelDistance;
    [SerializeField] private float _fireRate;

    private bool _idle = true;
    private IEnumerator _enemyFinderTimer;

    private void Start()
    {
        this.WatchAround();
        StartCoroutine(this._enemyFinderTimer = this.EnemyFinder());
    }

    private IEnumerator EnemyFinder()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            Collider[] enemyOnVision = Physics.OverlapSphere(transform.position, this._turrelDistance, LayerMask.GetMask("EnemyFinded"));

            if (enemyOnVision.Length != 0)
            {
                this._idle = false;
                float minDistance = this._turrelDistance + 1;
                Vector3 enemyDirection = Vector3.zero;
                
                foreach (Collider enemy in enemyOnVision)
                {
                    float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
                    if (distanceToEnemy < minDistance)
                    {
                        enemyDirection = new Vector3(
                            enemy.transform.position.x - transform.position.x,
                            0,
                            enemy.transform.position.z - transform.position.z
                        ).normalized;
                    }
                }
                
                this._gun.RotateTo(enemyDirection);
                this._gun.Shoot();
            }
            else
            {
                if (!this._idle)
                {
                    this._idle = true;
                    this._gun.StopShooting();
                    this.WatchAround();
                }
            }
        }
    }

    private void WatchAround()
    {
        Debug.Log("Watch Around");
    }
}
