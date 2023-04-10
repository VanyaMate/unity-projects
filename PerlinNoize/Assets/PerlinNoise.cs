using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerlinNoise : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _seed;
    [SerializeField] private float _scale;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;

    private Renderer _renderer;

    private void Awake()
    {
        this._renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        this._seed = Random.Range(0, 2);
    }

    private void Update()
    {
        this._renderer.material.mainTexture = this.GetNoiseTexture();
    }

    private Texture2D GetNoiseTexture()
    {
        Texture2D texture2D = new Texture2D(this._width, this._height);

        for (int x = 0; x < this._width; x++)
        {
            for (int y = 0; y < this._height; y++)
            {
                texture2D.SetPixel(x, y, this.GetNoiseColorByPixels(x, y));
            }
        }

        texture2D.Apply();
        return texture2D;
    }

    private Color GetNoiseColorByPixels(int x, int y)
    {
        float xCoord = (float)x / this._width * this._scale + this._seed + this._offsetX;
        float yCoord = (float)y / this._height * this._scale + this._seed + this._offsetY;
        
        float weight = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(weight, weight, weight);
    }
}
