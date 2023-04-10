using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFinder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("EnemyFinded");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyFinded"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }
}
