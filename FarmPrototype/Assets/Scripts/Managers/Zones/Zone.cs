using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Managers.Zones
{
    public abstract class Zone : MonoBehaviour
    {
        [Header("Position")]
        [SerializeField] private Vector3 _start;
        [SerializeField] private Vector3 _finish;

        [Header("ShowModel")]
        [SerializeField] private Transform _model;
        [SerializeField] private Transform _canvas;

        public abstract void InitZone();
        public abstract void UpdateZone();
        public abstract void RemoveZone();
        public abstract void AddPoint();
        public abstract void RemovePoint();
    }
}