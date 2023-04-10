using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Building
{
    public enum BuildingMagnitType
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public class BuildingMagnitBox : MonoBehaviour
    {
        [SerializeField] private BuildingItemObject _parent;
        [SerializeField] private BoxCollider _magnitCollider;
        [SerializeField] private BuildingMagnitType _type;
        [SerializeField] private Vector3 _shift;

        public BuildingItemObject parent => _parent;
        public BuildingMagnitType type => _type;
        public Vector3 shift => _shift;
    }
}
