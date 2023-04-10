using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour
{
    [SerializeField] private bool _runOnce;
    [SerializeField] private float _orderPositionY;

    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        this._spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        this._spriteRenderer.sortingOrder = -(int)(transform.parent.position.y * 100 + this._orderPositionY);
        
        if (!this._runOnce)
        {
            Destroy(this);
        }
    }
}
