using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS15.Interfaces
{
    public interface ITakable
    {
        public void Take();
        public void Put(Vector3 position, Quaternion quaternion);
    }
}