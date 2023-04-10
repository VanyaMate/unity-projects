using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Player
{
    [Serializable]
    class MoveActionSettings
    {
        public float Speed;
    }

    [Serializable]
    class MouseActionSettings
    {
        [Range(0, 20)]
        public float ActionDistance;
    }

    [Serializable]
    class GunSettings
    {
        public Gun Gun;
    }
    
    [RequireComponent(typeof(MoveManager))]
    public class InputController : MonoBehaviour
    {
        [SerializeField] private MoveActionSettings _moveActionSettings = new MoveActionSettings();
        [SerializeField] private MouseActionSettings _mouseActionSettings = new MouseActionSettings();
        [SerializeField] private GunSettings _gunSettings = new GunSettings();

        private MoveManager _moveManager;
        
        private void Awake()
        {
            this._moveManager = GetComponent<MoveManager>();
        }

        private void Update()
        {
            this._MoveAction();
            this._MouseAction();
        }

        private void _MouseAction()
        {
            RaycastHit mouseWorldPosition = GameUtils.Instance.GetMouseWorldPositionHit();
            Vector3 viewDirection = new Vector3(
                mouseWorldPosition.point.x - transform.position.x, 
                0,
                mouseWorldPosition.point.z - transform.position.z
            ).normalized;
            
            this._gunSettings.Gun.RotateTo(viewDirection);
            
            // left button
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit clickedObject = mouseWorldPosition;
                
                if (
                    Vector3.Distance(
                        transform.position, 
                        clickedObject.point
                    ) <= this._mouseActionSettings.ActionDistance
                )
                {
                    //Debug.Log($"Distance <= { this._mouseActionSettings.ActionDistance } { clickedObject.collider.name }");

                    ResourceNode resourceNode = null;
                    clickedObject.collider.TryGetComponent<ResourceNode>(out resourceNode);
                    
                    if (resourceNode != null)
                    {
                        ResourceManager.Instance.AddResource(resourceNode.Take(5));
                    }
                }
            }
            
            // right button
            if (Input.GetMouseButtonDown(1))
            {
                this._gunSettings.Gun.Shoot();
            }

            if (Input.GetMouseButtonUp(1))
            {
                this._gunSettings.Gun.StopShooting();
            }
        }

        private void _MoveAction()
        {
            Vector3 moveVector = Vector3.zero;

            moveVector += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
            moveVector += new Vector3(0, 0, Input.GetAxisRaw("Vertical"));

            this._moveManager.MoveTo(moveVector.normalized * this._moveActionSettings.Speed);
        }
    }
}