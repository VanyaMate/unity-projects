using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Bots
{
    [Serializable]
    class MoveActionSettings
    {
        public float Speed;
        public MoveManager MoveManager;
        public float TargetDistance;
        public Transform Target;
    }

    [Serializable]
    class DamageSettings
    {
        public float Damage;
        public float HitRate;
        public float CurrentHitRateTimer;
    }

    [Serializable]
    class HealthSettings
    {
        public bool Died;
        public float MaxHealth;
        public float CurrentHealth;
        public HealthManager HealthManager;
    }

    [Serializable]
    class SoundSettings
    {
        public float IdleSoundTimer;
        public AudioClip IdleSound;
    }

    public class BotController : Controller
    {
        [SerializeField] private MoveActionSettings _moveActionSettings = new MoveActionSettings();
        [SerializeField] private DamageSettings _damageSettings = new DamageSettings();
        [SerializeField] private HealthSettings _healthSettings = new HealthSettings();
        [SerializeField] private SoundSettings _soundSettings = new SoundSettings();

        [SerializeField] private Collider _botCollider;

        private AudioSource _audio;
        
        private IEnumerator _finderPlayer;
        private IEnumerator _idleSound;
        
        private void Awake()
        {
            this._audio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            this._healthSettings.HealthManager.MaxHealth = this._healthSettings.MaxHealth;
            this._healthSettings.HealthManager.CurrentHealth = this._healthSettings.CurrentHealth;
            this._moveActionSettings.MoveManager.Speed = this._moveActionSettings.Speed;
            
            StartCoroutine(this._finderPlayer = this.FindPlayerCollidersAround());
            StartCoroutine(this._idleSound = this.IdleSound());
        }

        private void OnDestroy()
        {
            StopCoroutine(this._finderPlayer);
        }

        private void Update()
        {
            if (this._healthSettings.Died == false)
            {
                if (this._moveActionSettings.Target != null)
                {
                    this.MoveToTarget();
                }
            }
        }

        private void OnDisable()
        {
            StopCoroutine(this.FindPlayerCollidersAround());
        }

        public void DieAction()
        {
            gameObject.layer = LayerMask.GetMask("Default");
            Destroy(this._botCollider);
            this._healthSettings.Died = true;
            this._moveActionSettings.MoveManager.DieAction();
        }

        private void MoveToTarget()
        {
            Transform target = this._moveActionSettings.Target.transform;
            this._moveActionSettings.MoveManager.MoveToPoint(target.position, this._moveActionSettings.Speed);
/*            this._moveActionSettings.MoveManager.MoveTo(
                new Vector3(
                    target.position.x - transform.position.x,
                    0,
                    target.position.z - transform.position.z
                ).normalized * this._moveActionSettings.Speed
            );*/
        }

        private IEnumerator IdleSound()
        {
            while (this._healthSettings.Died == false)
            {
                yield return new WaitForSeconds(this._soundSettings.IdleSoundTimer - Random.Range(-3000, 2000)/1000);

                if (this._botCollider && this._botCollider.gameObject.layer == 10)
                {
                    this._audio.PlayOneShot(this._soundSettings.IdleSound, Random.Range(50, 150) / 100);
                }
            }
        }

        private IEnumerator FindPlayerCollidersAround()
        {
            while (true)
            {
                yield return new WaitForSeconds(this._damageSettings.HitRate);
                
                Transform nearestCollider = null;
                float nearestDistance = -1;
                foreach (Transform playerBuilding in BuildingManager.Instance.BuildingHashList)
                {
                    float distance = Vector3.Distance(transform.position, playerBuilding.position);

                    if (distance < nearestDistance || nearestDistance < 0)
                    {
                        nearestDistance = distance;
                        nearestCollider = playerBuilding;
                    }
                }

                if (nearestCollider != null)
                {
                    this._moveActionSettings.Target = nearestCollider;
                    if (nearestDistance < 1)
                    {
                        HealthManager targetHealthManager = null;
                        BuildingController buildingController = null;
                        if (nearestCollider.TryGetComponent<HealthManager>(out targetHealthManager))
                        {
                            targetHealthManager.Damage(this._damageSettings.Damage);
                        }
                        else if (nearestCollider.TryGetComponent<BuildingController>(out buildingController))
                        {
                            buildingController.HealthManager.Damage(this._damageSettings.Damage);
                        }
                    }
                }
                else
                {
                    this._moveActionSettings.Target = null;
                }
            }
        }
    }
}
