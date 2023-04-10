using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : Controller
{
    [SerializeField] private List<MonoBehaviour> _coreComponentsToDestroy = new List<MonoBehaviour>();
    [SerializeField] private List<Transform> _coreElementsToDestroy = new List<Transform>();
    [SerializeField] private HealthManager _healthManager;

    public HealthManager HealthManager => _healthManager;
    
    public void DestroyThis()
    {
        BuildingManager.Instance.BuildingHashList.Remove(transform);
        foreach (MonoBehaviour component in this._coreComponentsToDestroy)
        {
            Destroy(component);
        }
        
        foreach (Transform component in this._coreElementsToDestroy)
        {
            Destroy(component.gameObject);
        }
    }
}
