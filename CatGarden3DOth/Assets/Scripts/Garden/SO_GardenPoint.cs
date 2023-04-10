using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.InventoryManager;

namespace CG.Garden
{
    [CreateAssetMenu(fileName = "so_gardenPoint_", menuName = "game/garden/point/create", order = 51)]
    public class SO_GardenPoint : SO_InventoryData
    {
        [SerializeField] private float _progressTime;

        public float ProgressTime => _progressTime;
    }
}
