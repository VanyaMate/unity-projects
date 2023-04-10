using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    private GroundSettings _settings;
    private Transform _currentPrefab;

    public void SetGround (GroundSettings settings, Vector2 position)
    {
        transform.position = new Vector3(position.x, 0, position.y);
        this._settings = settings;
        Destroy(this._currentPrefab);
        this._currentPrefab = Instantiate(this._settings.Prefab, transform);
    }
}
