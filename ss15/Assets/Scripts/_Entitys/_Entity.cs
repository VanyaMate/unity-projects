using SS15.Entitys;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS15
{
    [Serializable]
    public class _Entity
    {
        [Header("Common props")]
        public _SO_Entity type;
        public Vector3 position;
        public Quaternion rotation;
        public int layer;

        [Header("Current entity changes")]
        public List<_EntityFingerPrint> fingerprints;

        public _Entity ()
        {
            this.fingerprints = new List<_EntityFingerPrint>();
        }


        // Methods
        public void AddFingerPrint (_EntityFingerPrint fingerprint)
        {
            if (!this.fingerprints.Contains(fingerprint))
            {
                this.fingerprints.Add(fingerprint);
            }
        }

        public void ClearFingerPrints ()
        {
            while (this.fingerprints.Count >= 0)
            {
                this.fingerprints.Remove(this.fingerprints[0]);
            }
        }


        // Entity
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    
        // Interact
        public virtual void LeftClickHandler () { }
        public virtual void RightClickHandler () { }
        public virtual void LeftClickUIHandler() { }
        public virtual void RightClickUIHandler() { }
    }
}