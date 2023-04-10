using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS
{
    public class CompoundEntityGameObject : MonoBehaviour
    {
        [SerializeField] private List<SimpleEntityGameObject> _compoundEntitys = new List<SimpleEntityGameObject>();

        public List<SimpleEntityGameObject> compoundEntitys => _compoundEntitys;

        private void Awake()
        {
            
        }
    }
}