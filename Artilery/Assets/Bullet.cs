using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Map")
        {
            Collider[] objects = Physics.OverlapSphere(transform.position, 10f);

            foreach (Collider obj in objects)
            {
                if (obj.tag == "Actor")
                {
                    Debug.Log("Destroy -> " + obj.name);
                }
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
