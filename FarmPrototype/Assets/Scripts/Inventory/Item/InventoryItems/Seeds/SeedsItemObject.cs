using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.UI.Seedling;

namespace VM.Seeds
{
    public class SeedsItemObject : InventoryItemObject
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private SeedlingInfoPointUI _infoPoint;

        public Canvas canvas => _canvas;
        public SeedlingInfoPointUI infoPoint => _infoPoint;
    }
}