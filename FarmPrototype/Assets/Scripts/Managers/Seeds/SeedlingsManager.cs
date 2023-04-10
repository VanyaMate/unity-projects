using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Managers.Save;
using VM.Player;
using VM.Save;
using VM.Seeds;
using VM.TerrainTools;
using VM.UI;

namespace VM.Managers
{
    public class SeedlingsManager : ObjectToSave
    {
        public static SeedlingsManager instance;
        public static UnityEvent<List<InventoryItemSeedling>> OnInit = new UnityEvent<List<InventoryItemSeedling>>();

        [SerializeField] private GhostObject _ghost;
        [SerializeField] private Transform _container;
        [Header("Settings")]
        [SerializeField] private float _groundColorCoefRadius = 1f;
        [SerializeField] private float _nearbySeadlingRadius = .05f;
        

        private List<InventoryItemSeedling> _seedlings = new List<InventoryItemSeedling>();
        private InventoryItem _currentSeed;
        private bool _active;

        public List<InventoryItemSeedling> seedlings => _seedlings;
        public Transform container => _container;

        private void Awake()
        {
            instance = this;
            OnInit.Invoke(this._seedlings);
        }

        private void Start()
        {
            StartCoroutine(this._GlobalSeedlingChanges());
        }

        private void Update()
        {
            if (this._active)
            {
                Quaternion tempQuat = this._ghost.quat;

                if (Utils.MouseOverGameObject)
                {
                    float groundColorCoef = TerrainManager.Instance.GetColorCoefFrom(
                        position: Utils.MouseWorldPosition.point, 
                        radius: this._groundColorCoefRadius, 
                        layer: 1
                    );

                    bool isTerrain =
                        Utils.MouseWorldPosition.transform &&
                        Utils.MouseWorldPosition.transform.gameObject && 
                        Utils.MouseWorldPosition.transform.gameObject.layer == 7;
                    bool groundCoefGranded = groundColorCoef > .1f;
                    bool nearbySeadling = Physics.OverlapSphere(Utils.MouseWorldPosition.point, this._nearbySeadlingRadius, LayerMask.GetMask("Seedling")).Length > 0;

                    if (isTerrain && groundCoefGranded && !nearbySeadling)
                    {
                        this._ghost.SetStatus(true);
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (this._currentSeed.Type.CommonType == "Семена")
                            {
                                InventoryItemSeed item = this._currentSeed.Storage.Get<InventoryItemSeed>(
                                    item: this._currentSeed, 
                                    amount: 1
                                );
                                InventoryItemSeedling seedling = item.PlaceOnScene(
                                    position: this._ghost.position, 
                                    rotation: Quaternion.Euler(this._ghost.transform.rotation.eulerAngles + new Vector3(0, Random.Range(0, 360), 0))
                                );
                            }
                            else if (this._currentSeed.Type.CommonType == "Саженец")
                            {
                                InventoryItemSeedling seedling = ((InventoryItemSeed)(this._currentSeed)).PlaceOnScene(
                                    position: this._ghost.position,
                                    rotation: Quaternion.Euler(this._ghost.transform.rotation.eulerAngles + new Vector3(0, Random.Range(0, 360), 0))
                                );

                                this.HideGhost(activateMenu: true);
                            }

                            if (this._currentSeed.Amount <= 0)
                            {
                                if (this._currentSeed.Storage != null)
                                {
                                    this._currentSeed.Storage.Get<InventoryItem>(this._currentSeed);
                                }

                                this.HideGhost(activateMenu: true);
                            }
                        }
                    }
                    else
                    {
                        this._ghost.SetStatus(false);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    this.HideGhost();
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    tempQuat = Quaternion.AngleAxis(tempQuat.eulerAngles.y - 1, Vector3.up);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    tempQuat = Quaternion.AngleAxis(tempQuat.eulerAngles.y + 1, Vector3.up);
                }

                this._ghost.SetPosition(Utils.MouseWorldPosition.point, tempQuat);
            }
        }

        public void ShowGhost (InventoryItemSeed seed)
        {
            MenuController.blockOpenMenu = true;
            this._active = true;
            this._currentSeed = seed;
            this._ghost.gameObject.SetActive(true);
            this._ghost.SetMesh(((SO_InventorySeedsItem)this._currentSeed.Type).seedlingPrefab.mesh);
        }

        public void ShowGhost (InventoryItemSeedling seedling)
        {
            MenuController.blockOpenMenu = true;
            this._active = true;
            this._currentSeed = seedling;
            this._ghost.gameObject.SetActive(true);
            this._ghost.SetMesh(((SO_InventorySeedlingsItem)this._currentSeed.Type).Prefab.mesh);
        }

        public void HideGhost (bool activateMenu = false)
        {
            if (activateMenu)
            {
                MenuController.blockOpenMenu = true;
            }

            this._active = false;
            this._ghost.gameObject.SetActive(false);
        }

        private IEnumerator _GlobalSeedlingChanges ()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                this._seedlings.ForEach((seed) => {
                    seed.growthTime += .5f;

                    float seedSize = seed.progress > 100 ? 1 : seed.progress / 100; // 1 : seed.progress <= 10 ? .05f : Mathf.Floor(seed.progress / 10) / 10;

                    seed.seedsItemObject.transform.localScale = new Vector3(seedSize, seedSize, seedSize);
                });
            }
        }

        public void FullReset()
        {
            this._seedlings.ForEach((item) =>
            {
                item.RemoveSeedlingFromScene(deleteFromCommonManager: false);
            });

            this._seedlings.RemoveAll(x => true);
            this._seedlings = new List<InventoryItemSeedling>();
        }

        public override string GetSaveData()
        {
            List<SeedlingSaveData> seedlingsSaveData = new List<SeedlingSaveData>();

            this._seedlings.ForEach((seedling) =>
            {
                Debug.Log("SM: " + seedling.Type.Name);
                SeedlingSaveData save = new SeedlingSaveData()
                {
                    position = new SerVector(seedling.OnScene.transform.position),
                    rotation = new SerQuaternion(seedling.OnScene.transform.rotation),
                    amount = seedling.Amount,
                    growthTime = seedling.growthTime,
                    itemId = seedling.Type.Id
                };

                seedlingsSaveData.Add(save);
            });

            return JsonConvert.SerializeObject(seedlingsSaveData);
        }

        public override void LoadSaveData(string data)
        {
            List<SeedlingSaveData> seedlingSaveDatas = JsonConvert.DeserializeObject<List<SeedlingSaveData>>(data);

            seedlingSaveDatas.ForEach((item) =>
            {
                SO_InventorySeedlingsItem itemType = (SO_InventorySeedlingsItem)InventoryListOfTypes.Instance.GetItemById(item.itemId);

                InventoryItemSeedling seedling = new InventoryItemSeedling(itemType, item.amount);
                seedling.growthTime = item.growthTime;
                seedling.PlaceOnScene(
                    position: new UnityVector3(item.position).vector,
                    rotation: new UnityQuaternion(item.rotation).quaternion
                );
            });
        }
    }
}