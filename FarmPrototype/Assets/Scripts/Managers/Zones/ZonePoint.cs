using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Managers.Zones
{
    public abstract class ZonePoint : MonoBehaviour
    {
        public abstract void Add<T>(T zone, Vector3 position);
        public abstract void Remove();
    }
}