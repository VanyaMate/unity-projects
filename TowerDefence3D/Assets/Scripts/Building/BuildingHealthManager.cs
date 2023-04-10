using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingHealthManager : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;

    public void Repair(float amount)
    {
        if (this._health + amount > this._maxHealth)
        {
            this._health = this._maxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (this._health - amount <= 0)
        {
            this.Destroy();        
        }
    }

    private void Destroy()
    {
        BuildingManager.Instance.BuildingHashList.Remove(transform);
        Destroy(gameObject);
    }
}
