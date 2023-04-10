using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletSO BulletSO;
    private IEnumerator _destroyTimer;
    
    private void Awake()
    {
        this._destroyTimer = this._TimeDestroy();
        StartCoroutine(this._destroyTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Controller objectController = null;
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10)
        {
            if (collision.transform.TryGetComponent<Controller>(out objectController))
            {
                objectController.GetComponent<HealthManager>()?.Damage(this.BulletSO.Damage);
            }
        }

        Destroy(gameObject);
    }

    private IEnumerator _TimeDestroy()
    {
        yield return new WaitForSeconds(1);
        this._BulletDestroy();
    }

    private void _BulletDestroy()
    {
        StopCoroutine(this._destroyTimer);
        Destroy(gameObject);
    }
}
