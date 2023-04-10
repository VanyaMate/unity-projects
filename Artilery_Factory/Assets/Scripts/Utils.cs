using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 GetMouseWorldPosition ()
    {
        RaycastHit hit;
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(raycast, out hit))
        {
            return hit.point;
        }
        else
        {
            return default;
        }
    }
}
