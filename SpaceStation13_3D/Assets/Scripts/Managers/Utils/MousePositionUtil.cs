using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionUtil : MonoBehaviour
{
    public static RaycastHit WorldPosition;

    private Camera _camera;

    private void Awake()
    {
        this._camera = Camera.main;
    }

    private void Update()
    {
        Ray ray = this._camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out WorldPosition);
    }
}
