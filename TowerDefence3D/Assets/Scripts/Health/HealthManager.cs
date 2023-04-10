using System;
using System.Collections;
using System.Collections.Generic;
using Bots;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Transform _core;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    [SerializeField] private HealthUnitUI _healthBar;
    [SerializeField] private bool _showHealthBar;

    [SerializeField] private BuildingController _buildingController;

    public float MaxHealth
    {
        get { return this._maxHealth; }
        set
        {
            this._maxHealth = value;
            this.UpdateData();
        }
    }

    public float CurrentHealth
    {
        get { return this._currentHealth; }
        set
        {
            this._currentHealth = value;
            this.UpdateData();
        }
    }

    private void Start()
    {
        if (!this._showHealthBar)
        {
            this._healthBar?.gameObject.SetActive(false);
        }
        
        this.UpdateData();
    }

    private void OnDisable()
    {
        StopCoroutine(this.Die());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.Damage(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.Heal(3);
        }
    }

    public void UpdateData()
    {
        if (!this._showHealthBar)
        {
            this._healthBar?.UpdateProgress(100 / this._maxHealth * this._currentHealth);
        }
    }

    public void Damage(float amount)
    {
        if (this._currentHealth != 0)
        {
            this._currentHealth -= amount;

            if (this._currentHealth <= 0)
            {
                StartCoroutine(this.Die());
                this._currentHealth = 0;
                return;
            }

            this.UpdateData();
        }
    }

    public void Heal(float amount)
    {
        this._currentHealth += amount;

        if (this._currentHealth > this._maxHealth)
        {
            this._currentHealth = this._maxHealth;
        }
        
        this.UpdateData();
    }

    public IEnumerator Die()
    {
        this._healthBar?.gameObject.SetActive(false);
        this.BotDie();
        this.BuildingDestroy();
        
        yield return new WaitForSeconds(2);

        BuildingManager.Instance.BuildingHashList.Remove(this._core.transform);
        Destroy(this._core.gameObject);
    }

    private void BotDie()
    {
        BotController botController = null;
        if (TryGetComponent<BotController>(out botController))
        {
            botController.DieAction();
        }
    }

    private void BuildingDestroy()
    {
        if (this._buildingController != null)
        {
            this._buildingController.DestroyThis();
        }
    }
}
