using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private GameObject _spriteGameObject;

    private bool _isActive;

    private void Awake()
    {
        this._isActive = false;
        this._spriteGameObject.SetActive(false);
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingChanged += _BuildingManager_OnActiveBuildingChanged;
    }

    private void _BuildingManager_OnActiveBuildingChanged(object sender, System.EventArgs e)
    {
        if (BuildingManager.Instance.ActiveBuildingType != null)
        {
            this.Show(BuildingManager.Instance.ActiveBuildingType.Prefab.Find("sprite").GetComponent<SpriteRenderer>().sprite);
        }
        else
        {
            this.Hide();
        }
    }

    private void Update()
    {
        if (this._isActive)
        {
            transform.position = UtilsClass.GetMouseWorldPosition();
        }
    }

    public void Show (Sprite sprite)
    {
        this._isActive = true;
        this._spriteGameObject.SetActive(true);
        this._spriteGameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void Hide ()
    {
        this._isActive = false;
        this._spriteGameObject.SetActive(false);
    }
}
