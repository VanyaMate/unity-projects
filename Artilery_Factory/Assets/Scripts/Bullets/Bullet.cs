using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletSO _bulletSO;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            GameObject explosion = Instantiate(this._bulletSO.ExplosionEffect, transform.position, Quaternion.identity);

            Collider[] tanks = Physics.OverlapSphere(transform.position, this._bulletSO.Damage, LayerMask.GetMask(new string[] { "TankTarget" }));

            foreach (Collider tank in tanks)
            {
                tank.gameObject.layer = 14;
                Destroy(tank.transform.GetComponent<Bot>());

                foreach(Light light in tank.transform.GetComponentsInChildren<Light>())
                {
                    light.color = Color.red;
                }
            }

            Destroy(gameObject);
        }
    }
}
