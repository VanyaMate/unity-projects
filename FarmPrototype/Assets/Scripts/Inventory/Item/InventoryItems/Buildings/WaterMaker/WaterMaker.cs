using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.TerrainTools;

namespace VM.Inventory
{
    public class WaterMaker : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(this.SetWaterAround());
        }

        private IEnumerator SetWaterAround ()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                TerrainManager.Instance.redactor.ChangeColorsFromTo(transform.position, 5f, 2, 1, .2f, 0);
            }
        }
    }
}