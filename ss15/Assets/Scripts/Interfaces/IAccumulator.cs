using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS15.Interfaces
{
    public interface IAccumulator
    {
        public void Charge(float amount);
        public bool GetElectric(float amount);
    }
}