using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.UI;
using UnityEngine.UIElements;

namespace CG.Garden
{
    public class GardenManager : MonoBehaviour
    {
        public static GardenManager Instance;

        [SerializeField] private GardenPoint _defaultGardenPrefab;
        [SerializeField] private List<SO_GardenPoint> _gardenTypes;
        [SerializeField] private Transform _ghostGarden;

        [Header("Icons Groups")]
        [SerializeField] private Sprite _instrumentsIcon;
        [SerializeField] private Sprite _seedlingIcon;
        [Header("Icons Instruments")]
        [SerializeField] private Sprite _instrumentsMakeGarden;
        [SerializeField] private Sprite _instrumentsWater;

        private Dictionary<Vector3, GardenPoint> _gardenPoints = new Dictionary<Vector3, GardenPoint>();
        private SO_GardenPoint _currentSeedlingType;
        private bool _makeGarden = false;
        private bool _waterGarden = false;

        public SO_GardenPoint CurrentSeedlingType => _currentSeedlingType;
        public bool MakeGarden => _makeGarden;
        public bool WaterGarden => _waterGarden;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this._currentSeedlingType = null;
                this._makeGarden = false;
                this._waterGarden = false;
            }
        }

        private void Start()
        {
            this.AddInstrumentsUI();
            this.AddSeedlingUI();
        }

        private void AddInstrumentsUI ()
        {
            UIBuilderManager.Instance.RenderInstrumentsGroup(
                new UIBuilderGroupData()
                {
                    ButtonName = "Instrum",
                    GroupItems = new List<UIBuilderGroupItem>()
                    {
                        new UIBuilderGroupItem()
                        {
                            ButtonName = "Make",
                            ButtonEvent = () =>
                            {
                                this._makeGarden = true;
                                this._waterGarden = false;
                                this._currentSeedlingType = null;
                            },
                            ButtonIcon = this._instrumentsMakeGarden
                        },
                        new UIBuilderGroupItem()
                        {
                            ButtonName = "Water",
                            ButtonEvent = () =>
                            {
                                this._makeGarden = false;
                                this._waterGarden = true;
                                this._currentSeedlingType = null;
                            },
                            ButtonIcon = this._instrumentsWater
                        }
                    },
                    ButtonIcon = this._instrumentsIcon
                }
            );
        }

        private void AddSeedlingUI ()
        {
            // AddSeedling
            List<UIBuilderGroupItem> gardenFarmItems = new List<UIBuilderGroupItem>();

            this._gardenTypes.ForEach((SO_GardenPoint i) =>
            {
                gardenFarmItems.Add(new UIBuilderGroupItem()
                {
                    ButtonName = i.Name,
                    ButtonEvent = () => {
                        Debug.Log(i.Name); 
                        this._makeGarden = false;
                        this._waterGarden = false;
                        this._currentSeedlingType = i;
                    },
                    ButtonIcon = i.Icon
                });
            });

            UIBuilderManager.Instance.RenderInstrumentsGroup(
                new UIBuilderGroupData()
                {
                    ButtonName = "Seedling",
                    GroupItems = gardenFarmItems,
                    ButtonIcon = this._seedlingIcon
                }
            );
        }

        public void ShowGardenGhost(Vector3 place)
        {
            this._ghostGarden.position = place;
            this._ghostGarden.gameObject.SetActive(true);
        }

        public void HideGardenGhost()
        {
            this._ghostGarden.gameObject.SetActive(false);
        }

        public bool AddGarden(Vector3 place, bool force = false)
        {
            GardenPoint oldGardenPoint;
            bool placeFilled = this._gardenPoints.TryGetValue(place, out oldGardenPoint);

            if (placeFilled && !force)
            {
                return false;
            }
            else
            {
                GardenPoint gardenPoint = Instantiate(this._defaultGardenPrefab, transform);
                gardenPoint.transform.position = place;

                if (oldGardenPoint != null)
                {
                    Destroy(oldGardenPoint.gameObject);
                }

                this._gardenPoints.Add(place, gardenPoint);
                return true;
            }
        }

        public GardenPoint GetGardenPoint (Vector3 position)
        {
            return this._gardenPoints[position];
        }

        public Vector3 GetGardenSetPoint (Vector3 fromPosition)
        {
            return new Vector3(Mathf.Round(fromPosition.x), 0, Mathf.Round(fromPosition.z));
        }
    }
}
