using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS15.Interfaces
{
    public interface IPowered
    {
        public bool Power(bool state);
        public void PowerUpdate();
        public bool PowerConnect();
    }
}