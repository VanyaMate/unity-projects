using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VM.Managers;

namespace VM.TerrainTools
{
    public class TerrainRedactor : MonoBehaviour
    {
        public static TerrainRedactor Instance;

        [SerializeField] private Terrain _terrain;
        [SerializeField] private TerrainCollider _collider;

        private float _width;
        private float _length;
        private Vector3 _terrainPosition;

        public Terrain Terrain => _terrain;

        private void Awake()
        {
            Instance = this;

            this._width = this._terrain.terrainData.size.x;
            this._length = this._terrain.terrainData.size.z;
            this._terrainPosition = this._terrain.transform.position;

            this._ResetTerrainHeights();
            this._ResetTerrainDetails();
            this._ResetTerrainColors();
        }

        public void SetHeights(Vector3 position, float radius, float opacity)
        {
            return;
            int _radius = (int)Mathf.Ceil(radius * this._terrain.terrainData.heightmapResolution / this._width + 1);
            Vector3 points = (position - this._terrainPosition) * this._terrain.terrainData.heightmapResolution / this._width;
            int pointX = (int)Mathf.Round(points.x - _radius / 2);
            int pointZ = (int)Mathf.Round(points.z - _radius / 2);

            float[,] heights = this._terrain.terrainData.GetHeights(pointX, pointZ, _radius, _radius);
            int center = _radius / 2;
            Vector2 centerPosition = new Vector2(center, center);

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
                    float distance = Vector2.Distance(new Vector2(i, j), centerPosition);
                    heights[i, j] = opacity - opacity * (distance > center ? 1 : distance / center);
                }
            }

            this._terrain.terrainData.SetHeights(pointX, pointZ, heights);
            this._collider.terrainData.SetHeights(pointX, pointZ, heights);
            this._FixRigidbodyKinematic(position, _radius);
        }

        public void ChangeHeights(Vector3 position, float radius, float amount)
        {
            return;
            int _radius = (int)Mathf.Ceil(radius * this._terrain.terrainData.heightmapResolution / this._width + 1);
            Vector3 points = (position - this._terrainPosition) * this._terrain.terrainData.heightmapResolution / this._width;
            int pointX = (int)Mathf.Round(points.x - _radius / 2);
            int pointZ = (int)Mathf.Round(points.z - _radius / 2);

            float[,] heights = this._terrain.terrainData.GetHeights(pointX, pointZ, _radius, _radius);
            int center = _radius / 2;

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
                    float distance = Vector2.Distance(new Vector2(i, j), new Vector2(center, center));
                    heights[i, j] += amount - amount * (distance > center ? 1 : distance / center);
                }
            }

            this._terrain.terrainData.SetHeights(pointX, pointZ, heights);
            this._collider.terrainData.SetHeights(pointX, pointZ, heights);
            this._FixRigidbodyKinematic(position, _radius);
        }

        public void ChangeRandomHeights (Vector3 position, float radius, float range)
        {
            return;
            float heightmapPointWidth = this._terrain.terrainData.heightmapResolution / this._width;
            int _radius = Mathf.CeilToInt(radius * heightmapPointWidth + 1);
            Vector3 points = (position - this._terrainPosition) * heightmapPointWidth;
            int pointX = Mathf.RoundToInt(points.x - _radius / 2);
            int pointZ = Mathf.RoundToInt(points.z - _radius / 2);

            float[,] heights = this._terrain.terrainData.GetHeights(pointX, pointZ, _radius, _radius);
            int center = _radius / 2;
            Vector2 centerPosition = new Vector2(center, center);

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
                    float _range = Random.Range(-range, range);
                    float distance = Vector2.Distance(new Vector2(i, j), centerPosition);
                    heights[i, j] += _range - _range * (distance > center ? 1 : distance / center);
                }
            }

            this._terrain.terrainData.SetHeights(pointX, pointZ, heights);
            //this._collider.terrainData.SetHeights(pointX, pointZ, heights);
            //this._FixRigidbodyKinematic(position, _radius);
        }

        public void AlignmentHeights (Vector3 position, float radius)
        {
            return;
            int _radius = (int)Mathf.Ceil(radius * this._terrain.terrainData.heightmapResolution / this._width);
            Vector3 points = (position - this._terrainPosition) * this._terrain.terrainData.heightmapResolution / this._width;
            int pointX = (int)Mathf.Round(points.x - _radius / 2);
            int pointZ = (int)Mathf.Round(points.z - _radius / 2);

            float[,] heights = this._terrain.terrainData.GetHeights(pointX, pointZ, _radius, _radius);
            float heightsAmount = 0;
            float heightsSummary = 0;
            int center = _radius / 2;

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
                    float distance = Vector2.Distance(new Vector2(i, j), new Vector2(center, center));

                    if (distance < center)
                    {
                        heightsAmount += 1;
                        heightsSummary += heights[i, j];
                    }
                }
            }

            float avgHeight = heightsSummary / heightsAmount;

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
                    float distance = Vector2.Distance(new Vector2(i, j), new Vector2(center, center));
                    heights[i, j] = distance < center ? avgHeight : heights[i, j];
                }
            }

            this._terrain.terrainData.SetHeights(pointX, pointZ, heights);
            this._collider.terrainData.SetHeights(pointX, pointZ, heights);
            this._FixRigidbodyKinematic(position, _radius);
        }

        public void SetDetails(Vector3 position, float radius, int layer, int opacity)
        {
            int _radius = (int)Mathf.Ceil(radius * this._terrain.terrainData.detailResolution / this._width);
            Vector3 detailsPoints = (position - this._terrainPosition) * this._terrain.terrainData.detailResolution / this._width;
            int dpX = (int)(detailsPoints.x - _radius / 2);
            int dpZ = (int)(detailsPoints.z - _radius / 2);

            int[,] details = this._terrain.terrainData.GetDetailLayer(dpX, dpZ, _radius, _radius, layer);
            int center = _radius / 2;

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
                    float distance = Vector2.Distance(new Vector2(i, j), new Vector2(center, center));
                    details[i, j] = distance > center ? 0 : opacity;
                }
            }

            this._terrain.terrainData.SetDetailLayer(dpX, dpZ, layer, details);
        }

        public void RandomRemoveDetails(Vector3 position, float radius, int layer, float chanse)
        {
            int _radius = (int)Mathf.Ceil(radius * this._terrain.terrainData.detailResolution / this._width);
            Vector3 detailsPoints = (position - this._terrainPosition) * this._terrain.terrainData.detailResolution / this._width;
            int dpX = (int)(detailsPoints.x - _radius / 2);
            int dpZ = (int)(detailsPoints.z - _radius / 2);

            int[,] details = this._terrain.terrainData.GetDetailLayer(dpX, dpZ, _radius, _radius, layer);
            int center = _radius / 2;

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
                    if (details[i, j] != 0)
                    {
                        float range = Random.Range(0f, 1f);
                        float distance = Vector2.Distance(new Vector2(i, j), new Vector2(center, center));
                        details[i, j] = distance > center ? details[i, j] : range > chanse ? 1 : 0;
                    }
                }
            }

            this._terrain.terrainData.SetDetailLayer(dpX, dpZ, layer, details);
        }

        public void SetColor(Vector3 position, float radius, int layer, float opacity)
        {
            int _radius = Mathf.CeilToInt(radius * this._terrain.terrainData.alphamapResolution / this._width + 1);
            Vector3 colorPoint = (position - this._terrainPosition) * this._terrain.terrainData.alphamapResolution / this._width;
            int colorX = Mathf.RoundToInt(colorPoint.x - _radius / 2);
            int colorZ = Mathf.RoundToInt(colorPoint.z - _radius / 2);

            float[,,] colors = this._terrain.terrainData.GetAlphamaps(colorX, colorZ, _radius, _radius);
            int center = _radius / 2;
            Vector2 centerPosition = new Vector2(center, center);

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
                    float distance = Vector2.Distance(new Vector2(i, j), centerPosition);

                    for (int l = 0; l < colors.GetLength(2); l++)
                    {
                        colors[i, j, l] = distance > center ? colors[i, j, l] : (1 - opacity) / (colors.GetLength(2) - 1);
                    }

                    colors[i, j, layer] = distance > center ? colors[i, j, layer] : opacity;
                }
            }

            this._terrain.terrainData.SetAlphamaps(colorX, colorZ, colors);
        }

        public void ChangeColorsFromTo (Vector3 position, float radius, int layerDec, int layerInc, float value, float minDec)
        {
            int _radius = Mathf.CeilToInt(radius * this._terrain.terrainData.alphamapResolution / this._width + 1);
            Vector3 colorPoint = (position - this._terrainPosition) * this._terrain.terrainData.alphamapResolution / this._width;
            int colorX = Mathf.RoundToInt(colorPoint.x - _radius / 2);
            int colorZ = Mathf.RoundToInt(colorPoint.z - _radius / 2);

            float[,,] colors = this._terrain.terrainData.GetAlphamaps(colorX, colorZ, _radius, _radius);
            int center = _radius / 2;
            Vector2 centerPosition = new Vector2(center, center);

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
                    float distance = Vector2.Distance(new Vector2(i, j), centerPosition);

                    if (distance < center)
                    {
                        float dec = colors[i, j, layerDec];
                        float inc = colors[i, j, layerInc];

                        if ((dec - value) > minDec)
                        {
                            colors[i, j, layerDec] -= value;
                            colors[i, j, layerInc] += value;
                        }
                    }
                }
            }

            this._terrain.terrainData.SetAlphamaps(colorX, colorZ, colors);
        }

        public void GlobalChangeColors (int layerDec, int layerInc, float value, float minDec)
        {
            int _radius = this._terrain.terrainData.alphamapResolution;
            float[,,] colors = this._terrain.terrainData.GetAlphamaps(0, 0, _radius, _radius);
            //int center = _radius / 2;

            for (int i = 0; i < _radius; i++)
            {
                for (int j = 0; j < _radius; j++)
                {
/*                    float distance = Vector2.Distance(new Vector2(i, j), new Vector2(center, center));

                    if (distance < center)
                    {*/
                        float dec = colors[i, j, layerDec];

                        if ((dec - value) > minDec)
                        {
                            colors[i, j, layerDec] -= value;
                            colors[i, j, layerInc] += value;
                        }
                    //}
                }
            }

            this._terrain.terrainData.SetAlphamaps(0, 0, colors);
        }

        public float[,] GetHeighsFrom (Vector3 positionFrom, Vector3 positionTo)
        {
            Vector3 pointsFrom = (positionFrom - this._terrainPosition) * this._terrain.terrainData.heightmapResolution / this._width;
            Vector3 pointsTo = (positionTo - this._terrainPosition) * this._terrain.terrainData.heightmapResolution / this._width;
            int pointFromX = (int)Mathf.Floor(pointsFrom.x);
            int pointFromZ = (int)Mathf.Floor(pointsFrom.z);
            int pointToX = (int)Mathf.Floor(pointsTo.x);
            int pointToZ = (int)Mathf.Floor(pointsTo.z);

            int pointStartX;
            int pointStartZ;
            int widthX;
            int lengthZ;

            if (pointFromX > pointToX)
            {
                pointStartX = pointToX;
                widthX = pointFromX - pointToX;
            }
            else
            {
                pointStartX = pointFromX;
                widthX = pointToX - pointFromX;
            }
            
            if (pointFromZ > pointToZ)
            {
                pointStartZ = pointToZ;
                lengthZ = pointFromZ - pointToZ;
            }
            else
            {
                pointStartZ = pointFromZ;
                lengthZ = pointToZ - pointFromZ;
            }

            return this._terrain.terrainData.GetHeights(pointStartX, pointStartZ, widthX, lengthZ);
        }

        public float[,,] GetColorsFrom (Vector3 position, float radius)
        {
            int _radius = (int)Mathf.Ceil(radius * this._terrain.terrainData.alphamapResolution / this._width);
            Vector3 colorPoint = (position - this._terrainPosition) * this._terrain.terrainData.alphamapResolution / this._width;
            int colorX = (int)(colorPoint.x - _radius / 2);
            int colorZ = (int)(colorPoint.z - _radius / 2);

            colorX = colorX <= 0 ? 1 : colorX;
            colorZ = colorX <= 0 ? 1 : colorZ;

            return this._terrain.terrainData.GetAlphamaps(colorX, colorZ, _radius, _radius);
        }

        private void _FixRigidbodyKinematic (Vector3 position, float radius)
        {
            Collider[] colliders = Physics.OverlapSphere(position, radius);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    if (!rb.isKinematic)
                    {
                        rb.isKinematic = true;
                        rb.isKinematic = false;
                    }
                }
            }
        }

        private void _ResetTerrainHeights()
        {
            float[,] heights = this._terrain.terrainData.GetHeights(
                0, 
                0, 
                this._terrain.terrainData.heightmapResolution, 
                this._terrain.terrainData.heightmapResolution
            );

            for (int i = 0; i < heights.GetLength(0); i++)
            {
                for (int j = 0; j < heights.GetLength(1); j++)
                {
                    heights[i, j] = .5f;
                }
            }

            this._terrain.terrainData.SetHeights(0, 0, heights);
        }

        private void _ResetTerrainDetails()
        {
            int[,] details = this._terrain.terrainData.GetDetailLayer(
                0,
                0,
                this._terrain.terrainData.detailResolution,
                this._terrain.terrainData.detailResolution,
                0
            );

            for (int i = 0; i < details.GetLength(0); i++)
            {
                for (int j = 0; j < details.GetLength(1); j++)
                {
                    details[i, j] = Random.Range(0, 1000) > 200 ? 1 : 0;
                }
            }

            this._terrain.terrainData.SetDetailLayer(0, 0, 0, details);
        }

        private void _ResetTerrainColors()
        {
            float[,,] alpha = this._terrain.terrainData.GetAlphamaps(
                0, 
                0, 
                this._terrain.terrainData.alphamapResolution,
                this._terrain.terrainData.alphamapResolution
            );

            for (int i = 0; i < alpha.GetLength(0); i++)
            {
                for (int j = 0; j < alpha.GetLength(1); j++)
                {
                    for (int l = 0; l < alpha.GetLength(2); l++)
                    {
                        alpha[i, j, l] = l == 0 ? 1 : 0;
                    }
                }
            }

            this._terrain.terrainData.SetAlphamaps(0, 0, alpha);
            this._terrain.Flush();
        }
    }
}