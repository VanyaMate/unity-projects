using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _headMinY;
    [SerializeField] private float _headMaxY;
    
    [SerializeField] private float _speed;

    private float rotationY = 0;
    
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        float rotationX = transform.localEulerAngles.y + horizontal;
        rotationY += Input.GetAxis("Mouse Y");
        rotationY = Mathf.Clamp(rotationY, this._headMinY, this._headMaxY);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
}
