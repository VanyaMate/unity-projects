using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory.Items;
using VM.Managers;
using VM.Managers.Save;
using VM.Save;

namespace VM.TerrainTools
{
    public class TerrainManager : ObjectToSave
    {
        public static TerrainManager Instance;

        [Header("Terrain Tools")]
        [SerializeField] private TerrainRedactor _redactor;

        [Header("Terrain Elements")]
        [SerializeField] private Terrain _terrain;
        [SerializeField] private Transform _terrainChangeArea;

        [Header("Props")]
        [SerializeField] private bool _enabled;

        private Vector3 _startPosition;
        private InventoryItemScythe _currentScythe;
        private InventoryItemShovel _currentShovel;
        private int _shovelMode = 1;

        public TerrainRedactor redactor => _redactor;

        private void Awake()
        {
            Instance = this;
            this._enabled = false;

            StartCoroutine(this._GlobalWaterChange());
        }

        private void Update()
        {
            if (this._enabled && Utils.MouseOverGameObject)
            {
                if (Utils.MouseWorldPosition.transform == this._terrain.transform)
                {
                    int radius = 1;

                    if (this._GetGroundInfoAboutArea(Utils.MouseWorldPosition.point, 1f) == 1)
                    {
                        this._ChangeGhostColor(new Color(0, 1, 0, .3f));
                    }
                    else
                    {
                        this._ChangeGhostColor(new Color(1, 0, 0, .3f));
                    }


                    if (this._currentShovel != null)
                    {
                        if (this._shovelMode == 1)
                        {
                            this.ShovelChanges(radius);
                        }
                        else if (this._shovelMode == 2)
                        {
                            this.ShovelAlignment(radius);
                        }
                        else if (this._shovelMode == 3)
                        {
                            this.ShovelDestroy(radius);
                        }
                    }
                    else if (this._currentScythe != null)
                    {
                        this.ScytheDestroy(radius *= 2);
                    }

                    this._ShowGhost(radius);

                    /*else if (Input.GetMouseButtonDown(1))
                    {
                        this._startPosition = Utils.MouseWorldPosition.point;
                    }
                    else if (Input.GetMouseButtonUp(1))
                    {
                        this.GetHeightsInfoAboutArea(this._startPosition, Utils.MouseWorldPosition.point);
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        this.GetGroundInfoAboutArea(Utils.MouseWorldPosition.point, 1f);
                    }*/
                }
            }
        }

        public void EnableShovel (InventoryItemShovel currentShovel)
        {
            this._currentShovel = currentShovel;
            this._enabled = true;
            this._HideGhost();
        }

        public void DisableShovel ()
        {
            this._enabled = false;
            this._currentShovel = null;
            this._HideGhost();
        }

        public void SetShovelMode (int mode = 1)
        {
            this._shovelMode = mode;
        }

        public void ShovelAlignment (float radius)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this._redactor.AlignmentHeights(Utils.MouseWorldPosition.point, radius);
            }
        }

        public void ShovelChanges (float radius)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (this._currentShovel.filled)
                {
                    this._redactor.ChangeHeights(Utils.MouseWorldPosition.point, radius, .003f);
                    this._redactor.SetDetails(Utils.MouseWorldPosition.point, radius + .6f, 0, 0);
                    this._redactor.SetColor(Utils.MouseWorldPosition.point, radius + .3f, 1, 1);
                    this._currentShovel.filled = false;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (!this._currentShovel.filled)
                {
                    this._redactor.ChangeHeights(Utils.MouseWorldPosition.point, radius, -.003f);
                    this._redactor.SetDetails(Utils.MouseWorldPosition.point, radius + .6f, 0, 0);
                    this._redactor.SetColor(Utils.MouseWorldPosition.point, radius + .3f, 1, 1);
                    this._currentShovel.filled = true;
                }
            }
        }

        public void ShovelDestroy(float radius)
        {
            if (Input.GetMouseButton(0))
            {
                this._redactor.ChangeRandomHeights(Utils.MouseWorldPosition.point, radius, .004f);
                this._redactor.SetDetails(Utils.MouseWorldPosition.point, radius + .6f, 0, 0);
                this._redactor.SetColor(Utils.MouseWorldPosition.point, radius + .3f, 1, 1);
            }
        }

        public void EnableScythe (InventoryItemScythe currentScythe)
        {
            this._currentScythe = currentScythe;
            this._enabled = true;
            this._HideGhost();
        }

        public void DisableScythe ()
        {
            this._enabled = false;
            this._currentScythe = null;
            this._HideGhost();
        }

        public void ScytheDestroy (float radius)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this._redactor.RandomRemoveDetails(Utils.MouseWorldPosition.point, radius, 0, 1f);
            }
        }

        public float GetColorCoefFrom(Vector3 position, float radius, int layer)
        {
            float[,,] colors = this._redactor.GetColorsFrom(position, radius);
            int _radius = colors.GetLength(0);
            int center = _radius / 2;
            float coef = 0;
            float summary = 0;
            float counter = 0;

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    for (int l = 0; l < colors.GetLength(2); l++)
                    {
                        float distance = Vector2.Distance(new Vector2(i, j), new Vector2(center, center));

                        if (distance < center)
                        {
                            if (l == layer)
                            {
                                counter += 1;
                                summary += colors[i, j, l];
                            }
                        }
                    }
                }
            }

            coef = summary / counter;

            return coef;
        }

        public bool PositionAcceptForChanges (Vector3 position, float radius)
        {
            if (
                position.x - radius < this._terrain.transform.position.x || 
                position.x + radius > this._terrain.transform.position.x + this._terrain.terrainData.size.x
            )
            {
                return false;
            }

            if (
                position.z - radius < this._terrain.transform.position.z ||
                position.z + radius > this._terrain.transform.position.z + this._terrain.terrainData.size.z
            )
            {
                return false;
            }

            return true;
        }

        private IEnumerator _GlobalWaterChange ()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                this._redactor.GlobalChangeColors(1, 2, .11f, .5f);
            }
        }

        private int _GetGroundInfoAboutArea (Vector3 position, float radius)
        {
            float[,,] colors = this._redactor.GetColorsFrom(position, radius);
            int color = -1;
            bool equals = true;
            int _radius = colors.GetLength(0);
            int center = _radius / 2;

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                if (equals)
                {
                    for (int j = 0; j < colors.GetLength(1); j++)
                    {
                        float distance = Vector2.Distance(new Vector2(i, j), new Vector2(center, center));

                        if (distance > center) continue;

                        if (equals)
                        {
                            for (int l = 0; l < colors.GetLength(2); l++)
                            {
                                if (color == -1 && colors[i, j, l] == 1)
                                {
                                    color = l;
                                }

                                if (color != -1 && (l != color || colors[i, j, l] != 1))
                                {
                                    if (colors[i, j, l] == 0) continue;

                                    color = -1;
                                    equals = false;
                                    break;
                                }
                            }
                        }
                        else break;
                    }
                } 
                else break;
            }

            return color;
        }

        // TODO:
        private void _GetHeightsInfoAboutArea (Vector3 from, Vector3 to)
        {
            float[,] heights = this._redactor.GetHeighsFrom(from, to);
            float startValue = -2;
            bool equals = true;

            for (int i = 0; i < heights.GetLength(0); i++)
            {
                if (equals)
                {
                    for (int j = 0; j < heights.GetLength(1); j++)
                    {
                        if (startValue == -2)
                        {
                            startValue = heights[i, j];
                        }
                        else
                        {
                            if (startValue != heights[i, j])
                            {
                                equals = false;
                                break;
                            }
                        }
                    }
                }
                else break;
            }

            Debug.Log("Heights: " + equals);
        }

        private void _ShowGhost (float radius)
        {
            this._terrainChangeArea.gameObject.SetActive(true);
            this._terrainChangeArea.transform.localScale = new Vector3(radius + .3f, .5f, radius + .3f);
            this._terrainChangeArea.transform.position = Utils.MouseWorldPosition.point + new Vector3(0, 0, 0);
        }

        private void _HideGhost ()
        {
            this._terrainChangeArea.gameObject.SetActive(false);
        }
            
        private void _ChangeGhostColor (Color color)
        {
            this._terrainChangeArea.GetComponent<MeshRenderer>().material.color = color;
        }

        public override string GetSaveData()
        {
            TerrainSaveData saveData = new TerrainSaveData();
            saveData.details = new Dictionary<int, int[,]>();

            TerrainData terrainData = this._terrain.terrainData;

            saveData.heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
            saveData.colors = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

            for (int i = 0; i < terrainData.detailPrototypes.Length; i++)
            {
                saveData.details.Add(i, terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, i));
            }

            return JsonConvert.SerializeObject(saveData);
        }

        public override void LoadSaveData(string data)
        {
            TerrainSaveData terrainData = JsonConvert.DeserializeObject<TerrainSaveData>(data);
            TerrainData terrain = this._terrain.terrainData;

            float[,,] colors = terrain.GetAlphamaps(0, 0, terrain.alphamapWidth, terrain.alphamapWidth);

            for (int i = 0; i < terrainData.colors.GetLength(0); i++)
            {
                for (int j = 0; j < terrainData.colors.GetLength(1); j++)
                {
                    for (int l = 0; l < terrainData.colors.GetLength(2); l++)
                    {
                        colors[i, j, l] = terrainData.colors[i, j, l];
                    }
                }
            }

            terrain.SetHeights(0, 0, terrainData.heights);
            terrain.SetAlphamaps(0, 0, colors);

            for (int i = 0; i < terrainData.details.Count; i++)
            {
                terrain.SetDetailLayer(0, 0, i, terrainData.details[i]);
            }
        }
    }
}