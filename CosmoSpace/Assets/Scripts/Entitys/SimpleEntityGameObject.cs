using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    //[ExecuteInEditMode]
    public class SimpleEntityGameObject : MonoBehaviour
    {
        [SerializeField] private SimpleEntity _currentEntityData;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;

        public SimpleEntity currentEntityData => _currentEntityData;
        public MeshRenderer meshRenderer => _meshRenderer;
        public MeshFilter meshFilter => _meshFilter;

        private void Awake()
        {
            this._meshRenderer = GetComponent<MeshRenderer>();
            this._meshFilter = GetComponent<MeshFilter>();

            this._currentEntityData.Init();
            // this.UpgradeTo(this._currentEntityData);
        }

        public void UpgradeTo (SimpleEntity entityData)
        {
            if (entityData.type.prefab != null)
            {
                this._meshFilter.mesh = entityData.type.prefab.meshFilter.sharedMesh;
            }

            this._meshRenderer.material = entityData.GetMaterial();
            this._meshRenderer.UpdateGIMaterials();
            this._meshRenderer.sharedMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        private void OnValidate()
        {
            //this.UpgradeTo(this.currentEntityData);
        }
    }
}