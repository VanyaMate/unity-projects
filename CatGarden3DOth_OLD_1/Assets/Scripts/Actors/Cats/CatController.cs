using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.Cat
{
    [RequireComponent(typeof(CatMoveManager))]
    [RequireComponent(typeof(CatNeeds))]
    public class CatController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] public CatMemory CatMemory;
        [SerializeField] public CatMoveManager CatMoveManager;
        [SerializeField] public CatNeeds CatNeeds;
        [SerializeField] public CatUI CatUI;

        [Header("Other")]
        [SerializeField] private Transform _cameraTarget;

        public Transform CameraTarget => _cameraTarget;
    }
}
