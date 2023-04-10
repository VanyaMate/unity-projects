using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.InventoryManager;

public class PlayerPockets : MonoBehaviour
{
    public static PlayerPockets Instance;

    private InventoryManager _inventory;

    public InventoryManager Inventory => _inventory;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPockets (InventoryManager pockets)
    {
        this._inventory = pockets;
    }
}
