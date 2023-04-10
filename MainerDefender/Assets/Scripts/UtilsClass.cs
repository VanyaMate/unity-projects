using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{
    public static Vector3 GetMouseWorldPosition(Camera camera, Vector3 lastPosition)
    {
        RaycastHit hit;
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out hit))
        {
            return hit.point;
        }

        return lastPosition;
    }
}
