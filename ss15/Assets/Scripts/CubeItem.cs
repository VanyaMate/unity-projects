using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS15.Interfaces;
using SS15.DataTypes;

public class CubeItem : MonoBehaviour, IMovable, ITakable, IToucheble
{
    public void ClearTouches()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public void Put(Vector3 position, Quaternion quaternion)
    {
        throw new System.NotImplementedException();
    }

    public void Take()
    {
        throw new System.NotImplementedException();
    }

    public void Touch(FingerPrint fingerPrint)
    {
        throw new System.NotImplementedException();
    }
}
