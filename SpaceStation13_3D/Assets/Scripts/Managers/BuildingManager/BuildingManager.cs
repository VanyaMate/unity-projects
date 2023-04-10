using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Transform _markPoint;
    [SerializeField] private Transform _wallPrefab;

    private void Update()
    {
        Vector3 worldPosition = MousePositionUtil.WorldPosition.point;
        int x = Mathf.FloorToInt(worldPosition.x);
        int z = Mathf.FloorToInt(worldPosition.z);

        if (worldPosition == Vector3.zero)
        {
            this._markPoint.gameObject.SetActive(false);
        }
        else
        {
            this._markPoint.gameObject.SetActive(true);
            this._markPoint.position = new Vector3(x, 0, z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(this._wallPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
        }
    }
}
