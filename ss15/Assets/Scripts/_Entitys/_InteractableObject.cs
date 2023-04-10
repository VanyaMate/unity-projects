using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS15 { 
    public class _InteractableObject : MonoBehaviour
    {
        [SerializeField] private _Entity _entity = null;

        public virtual void SetEntity(_Entity entity)
        {
            if (this._entity != null)
            {
                this._entity.Exit();
                this._entity = null;
            }

            this._entity = entity;
            this._entity.Enter();
        }

        public virtual void LeftClickHandler() 
        { 
            if (this._entity != null)
            {
                this._entity.LeftClickHandler();
            }
        }
        public virtual void RightClickHandler() 
        {
            if (this._entity != null)
            {
                this._entity.LeftClickHandler();
            }
        }
    }
}   