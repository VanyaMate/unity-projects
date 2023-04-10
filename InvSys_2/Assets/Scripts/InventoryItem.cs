using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private InventoryItemInfo _itemInfo;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _amount;

    public InventoryItemInfo ItemInfo => _itemInfo;
    public GameObject Prefab => _prefab;

    public float Amount
    {
        get => _amount;
        set => _amount = value;
    }

    public void HideTo (Transform inventory)
    {
        this.transform.SetParent(inventory);

        GetComponent<SphereCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

        this.transform.Find("ItemModel").gameObject.SetActive(false);
    }

    public void Preview ()
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

        GameObject model = this.transform.Find("ItemModel").gameObject;

        model.SetActive(true);
        model.GetComponent<SphereCollider>().enabled = false;

        Material modelMaterial = model.GetComponent<Renderer>().material;

        modelMaterial.color = new Color(
            modelMaterial.color.r, 
            modelMaterial.color.g, 
            modelMaterial.color.b, 
            .2f
        );
    }

    public void Show ()
    {
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;

        GameObject model = this.transform.Find("ItemModel").gameObject;

        model.SetActive(true);
        model.GetComponent<SphereCollider>().enabled = true;

        Material modelMaterial = model.GetComponent<Renderer>().material;

        modelMaterial.color = new Color(
            modelMaterial.color.r,
            modelMaterial.color.g,
            modelMaterial.color.b,
            1
        );
    }
}
