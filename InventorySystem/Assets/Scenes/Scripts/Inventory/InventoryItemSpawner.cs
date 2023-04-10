using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMCode.Inventory
{
    public class InventoryItemSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnTime;
        [SerializeField] private SO_InventoryItem _itemData;

        private void Awake()
        {
            StartCoroutine(this.SpawnItemTimer());
        }

        private IEnumerator SpawnItemTimer ()
        {
            while(true)
            {
                yield return new WaitForSeconds(this._spawnTime);

                InventoryItem item = new InventoryItem(this._itemData);

                item.SpawnTo(transform.position);
            }
        }
    }
}
