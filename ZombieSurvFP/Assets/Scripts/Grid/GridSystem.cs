using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ZS.Grid
{
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem instance;

        [SerializeField] private Vector2Int _gridAmount;
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private GridPoint[][] _grid;

        private void Awake()
        {
            instance = this;

            this._grid = new GridPoint[this._gridAmount.x][];
            Canvas debugCanvas = this._CreateWorldDebugGridCanvas(
                amount: this._gridAmount,
                size: this._gridSize
            );

            for (int i = 0; i < this._gridAmount.x; i++)
            {
                this._grid[i] = new GridPoint[this._gridAmount.y];

                for (int j = 0; j < this._gridAmount.y; j++)
                {
                    this._grid[i][j] = new GridPoint() { x = i, y = j, status = GridStatus.empty };
                    TextMeshPro debugText = this._CreateWorldTMPText(
                        canvas: debugCanvas,
                        position: new Vector3(i * this._gridSize.x, 0, j * this._gridSize.y),
                        text: $"{i}:{j}"
                    );
                }
            }
        }

        private Canvas _CreateWorldDebugGridCanvas (Vector2Int amount, Vector2Int size)
        {
            GameObject canvasGameObject = new GameObject("Grid Debug Canvas", typeof(Canvas));
            Canvas canvas = canvasGameObject.GetComponent<Canvas>();

            canvasGameObject.transform.position = new Vector3(-amount.x * size.x, 0, -amount.y * size.y) / 2;

            return canvas;
        }

        private TextMeshPro _CreateWorldTMPText (Canvas canvas, Vector3 position, string text)
        {
            GameObject tmpGameObject = new GameObject("Grid Point", typeof(TextMeshPro));
            TextMeshPro tmp = tmpGameObject.GetComponent<TextMeshPro>();

            tmpGameObject.transform.parent = canvas.transform;
            tmpGameObject.transform.localPosition = position;
            ((RectTransform)tmpGameObject.transform).sizeDelta = new Vector2(1, 1);
            tmp.text = text;
            tmp.fontSize = 3;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Top;

            return tmp;
        }
    }   
}