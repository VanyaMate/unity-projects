using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CG.Cat
{
    public class CatUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _foodProgress;
        [SerializeField] private RectTransform _emotionalProgress;
        [SerializeField] private RectTransform _fatigueProgress;

        [SerializeField] private UnityEngine.Camera _mainCamera;

        private void Awake()
        {
        }

        private void FixedUpdate()
        {
            // Rotate UI under cat to mainCamera
            transform.rotation = Quaternion.LookRotation(
                this._mainCamera.transform.position - transform.position, 
                Vector3.up
            );
        }

        public void SetFood (float current, float max)
        {
            this._foodProgress.sizeDelta = new Vector2(this.GetPercentProgress(current, max), .16f);
        }

        public void SetEmotional (float current, float max)
        {
            this._emotionalProgress.sizeDelta = new Vector2(this.GetPercentProgress(current, max), .16f);
        }

        public void SetFatigue(float current, float max)
        {
            this._fatigueProgress.sizeDelta = new Vector2(this.GetPercentProgress(current, max), .16f);
        }

        private float GetPercentProgress(float current, float max)
        {
            return .8f / max * current;
        }
    }
}
