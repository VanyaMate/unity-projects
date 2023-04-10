using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.InventoryManager;

namespace CG.Garden
{
    public class GardenPoint : MonoBehaviour
    {
        [SerializeField] private SO_GardenPoint _gardenData;

        [SerializeField] private string _name;
        [SerializeField] private float _progressTime;
        [SerializeField] private float _currentProgressTime;
        [SerializeField] private float _progress;
        [Header("Water")]
        [SerializeField] private float _waterMax;
        [SerializeField] private float _waterCurrent;
        [SerializeField] private Color _wateredGroundColor;
        [SerializeField] private Color _withoutWateredGroundColor;
        [SerializeField] private Color _currentGroundColor;
        [SerializeField] private MeshRenderer _mesh;

        private InventoryManager _invManager;
        private Transform _seedlingPrefab;
        private Material _gardenMaterial;

        private IEnumerator _progressTimer;
        private IEnumerator _waterLostTimer;

        private void Awake()
        {
            this._gardenMaterial = this._mesh.material;
        }

        private void Start()
        {
            StartCoroutine(this._waterLostTimer = this.WaterLost());
        }

        public void SetGardenSeedling(SO_GardenPoint gardenData)
        {
            if (this._gardenData != null)
            {
                StopCoroutine(this._progressTimer);
                Destroy(this._seedlingPrefab.gameObject);
            }

            this._gardenData = gardenData;
            this._name = gardenData.Name;
            this._progressTime = gardenData.ProgressTime;
            this._currentProgressTime = 0;
            this._progress = 0;
            this._seedlingPrefab = Instantiate(gardenData.Prefab, transform);
            this._seedlingPrefab.transform.position = transform.position;

            this._invManager = gameObject.AddComponent<InventoryManager>();
            this._invManager.SetInventoryData(this._name, 1);

            StartCoroutine(this._progressTimer = this.UpdateProgress());
        }

        public void WaterGarden(float waterAmount)
        {
            float waterCurrent = this._waterCurrent + waterAmount;

            this._waterCurrent = waterCurrent > this._waterMax ? this._waterMax : waterCurrent;

            this.UpdateGroundColor();
        }

        private IEnumerator WaterLost()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                this._waterCurrent -= .25f;

                if (this._waterCurrent < 0)
                {
                    this._waterCurrent = 0;
                }

                this.UpdateGroundColor();
            }
        }

        private void UpdateGroundColor ()
        {
            float koef = this._waterCurrent / 100f;

            this._gardenMaterial.color = this._currentGroundColor = new Color(
                this._wateredGroundColor.r * koef + this._withoutWateredGroundColor.r * (1 - koef),
                this._wateredGroundColor.g * koef + this._withoutWateredGroundColor.g * (1 - koef),
                this._wateredGroundColor.b * koef + this._withoutWateredGroundColor.b * (1 - koef)
            );
        }

        private IEnumerator UpdateProgress ()
        {
            while(this._progress < 100)
            {
                this._seedlingPrefab.localScale = new Vector3(1, this._progress / 100, 1);

                yield return new WaitForSeconds(.5f);

                this._currentProgressTime += .5f;
                this._progress = 100 / this._progressTime * this._currentProgressTime;

                if (this._progress <= 100)
                {
                    this._invManager.Add(new InventoryItemManager(this._gardenData, 1, this._invManager));
                }
            }
        }
    }
}
