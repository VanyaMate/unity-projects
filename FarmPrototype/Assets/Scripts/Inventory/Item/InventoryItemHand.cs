using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    public class InventoryItemHand : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        public MeshFilter meshFilter => _meshFilter;
        public MeshRenderer meshRenderer => _meshRenderer;
    }
}