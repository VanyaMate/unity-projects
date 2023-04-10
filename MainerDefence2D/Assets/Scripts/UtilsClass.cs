using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{
    public static Vector3 GetMouseWorldPosition ()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
}
