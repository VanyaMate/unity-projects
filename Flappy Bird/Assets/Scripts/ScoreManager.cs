using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FB.Score
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        [Header("Current Score")]
        [SerializeField] private float _score;

        [Header("Score Counter")]
        [SerializeField] private float _startMultiplyScore;

        public UnityEvent<float> OnScoreChange = new UnityEvent<float>();

        private void Awake()
        {
            Instance = this;
        }

        public void AddPoint(float amount)
        {
            this._score += amount * this._startMultiplyScore;
            this.OnScoreChange?.Invoke(this._score);
        }
    }
}
