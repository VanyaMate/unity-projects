using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawner;
    [SerializeField] private GunSO _gunType;

    private Vector3 _currentGunDirection;
    private IEnumerator _shootingIterator;
    private AudioSource _audio;

    private void Awake()
    {
        this._audio = GetComponent<AudioSource>();
        this._audio.clip = this._gunType.ShootSound;
    }

    public void Shoot()
    {
        if (this._gunType.Automat)
        {
            this.StopShooting();
            StartCoroutine(this._shootingIterator = this.StartShooting());
        }
        else
        {
            this._Shoot();
        }
    }

    public IEnumerator StartShooting()
    {
        while (true)
        {
            this._Shoot();

            yield return new WaitForSeconds(1 / (float)this._gunType.FireRate);
        }
    }

    private void _Shoot()
    {            
        this._audio.Play();
        Transform bullet = Instantiate(this._gunType.BulletType.Prefab, this._bulletSpawner.position,
            Quaternion.Euler(this._currentGunDirection));
        bullet.GetComponent<Rigidbody>()
            .AddForce(this._currentGunDirection * this._gunType.StartSpeed, ForceMode.Impulse);
        bullet.GetComponent<Bullet>().BulletSO = this._gunType.BulletType;
    }

    public void StopShooting()
    {
        if (this._shootingIterator != null)
        {
            StopCoroutine(this._shootingIterator);
            this._shootingIterator = null;
        }
    }
    
    public void RotateTo(Vector3 direction)
    {
        this._currentGunDirection = direction;
        this._bulletSpawner.position = transform.position + direction;
        this._bulletSpawner.rotation = Quaternion.LookRotation(direction);
    }
}
