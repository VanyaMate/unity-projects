using SS15.DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS15.Interfaces
{
    public interface IToucheble
    {
        public void Touch(FingerPrint fingerPrint);
        public void ClearTouches();
    }
}