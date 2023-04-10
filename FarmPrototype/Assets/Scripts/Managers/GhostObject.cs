using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Managers
{
    public class GhostObject : MonoBehaviour
    {
        [Header("Mesh components")]
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        [Header("Material status")]
        [SerializeField] private Material _successStatus;
        [SerializeField] private Material _deniedStatus;

        private GameObject _insModel = null;

        [Header("Props")]
        public Vector3 position;
        public Quaternion quat => transform.rotation;
        public bool status;
        public Transform model => _meshFilter.gameObject.transform;

        private void Awake()
        {
            // this.quat = Quaternion.identity;
        }

        public void Enable (bool status)
        {
            gameObject.SetActive(status);
        }

        public void SetPosition(Vector3 position, Quaternion quaternion)
        {
            transform.position = this.position = position;
            transform.rotation = quaternion;
        }

        public void SetMesh(Mesh mesh = null)
        {
            this._meshFilter.mesh = mesh ? mesh : this._meshFilter.mesh;
        }

        public void InsModel (GameObject model)
        {
            Destroy(this._insModel);

            this._insModel = Instantiate(
                model,
                transform
            );

            this._insModel.transform.localPosition = Vector3.zero;
            this._insModel.layer = 2;

            List<MeshRenderer> renderes = new List<MeshRenderer>(
                this._insModel.transform.GetComponentsInChildren<MeshRenderer>()
            );

            List<Rigidbody> rbList = new List<Rigidbody>(
                this._insModel.transform.GetComponentsInChildren<Rigidbody>()
            );

            List<Collider> colList = new List<Collider>(
                this._insModel.transform.GetComponentsInChildren<Collider>()
            );

            rbList.ForEach((rb) => {
                rb.isKinematic = true;
                rb.useGravity = false;
            });

            colList.ForEach((col) =>
            {
                col.enabled = false;
            });

            renderes.ForEach((x) => x.material = this._successStatus);


            if (this._insModel.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            if (this._insModel.TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = false;
            }
        }

        public void SetStatus(bool status)
        {
            this.status = status;
            this._meshRenderer.material = status ? this._successStatus : this._deniedStatus;
        }
    }
}
