using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CG.Cat
{
    [Serializable]
    class CatNeedsFood
    {
        [Header("Counters")]
        public float Quality;
        public float Satiety;

        [Header("Dec Amount")]
        public float HungerAmountBySecond;

        [Header("Status")]
        public bool Hung;
        public bool MoveTo;

        public bool Busy => this.Hung || this.MoveTo;
    }

    [Serializable]
    class CatNeedsEmotional
    {
        [Header("Counters")]
        public float Kindness;
        public float Stress;

        [Header("Dec Amount")]
        public float KindnessAmountBySecond;

        [Header("Inc Amount")]
        public float StressAmountBySecond;

        [Header("Status")]
        public bool Played;
        public bool Kindessed;

        public bool Busy => this.Played || this.Kindessed;
    }

    [Serializable]
    class CatFatigue
    {
        [Header("Counters")]
        public float Sleep;
        public float Play;

        [Header("Dec Amount")]
        public float SleepAmountBySecond;
        public float PlayAmountBySecond;

        [Header("Inc Amount")]
        public float UnSleepAmountBySecond;
        public float UnPlayAmountBySecond;

        [Header("Status")]
        public bool Sleepped;
        public bool Played;
        public bool Walked;

        [Header("Data")]
        public Vector3 WalkTo;

        public bool Busy => this.Sleepped || this.Played;
    }

    public class CatNeeds : MonoBehaviour
    {
        [SerializeField] private CatNeedsFood _food;
        [SerializeField] private CatNeedsEmotional _emotional;
        [SerializeField] private CatFatigue _fatigue;

        [SerializeField] private CatController _catController;

        private IEnumerator _fungerTimer;
        private IEnumerator _emotionalTimer;
        private IEnumerator _fatigueTimer;
        
        private void Awake()
        {
            this._catController = GetComponent<CatController>();
        }

        private void OnEnable()
        {
            StartCoroutine(this._fungerTimer = this.FoodTimer());
            StartCoroutine(this._emotionalTimer = this.EmotionalTimer());
            StartCoroutine(this._fatigueTimer = this.FatigueTimer());
        }

        private void OnDisable()
        {
            StopCoroutine(this._fungerTimer);
            StopCoroutine(this._emotionalTimer);
            StopCoroutine(this._fatigueTimer);
        }

        private IEnumerator FoodTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                this._food.Satiety -= this._food.HungerAmountBySecond / 2;

                if (!this._emotional.Busy)
                {
                    if (this._food.Satiety < 50 || this._food.Hung == true)
                    {
                        Vector3 nearestFoodPoint = this._catController.CatMemory.GetNearestFoodPoint().transform.position;

                        if (Vector3.Distance(transform.position, nearestFoodPoint) < .5f)
                        {
                            this._food.MoveTo = false;
                            this._food.Hung = true;
                            this._food.Satiety += 10f;

                            if (this._food.Satiety >= 100)
                            {
                                this._food.Satiety = 100;
                                this._food.Hung = false;
                            }
                        }
                        else
                        {
                            this._food.MoveTo = true;
                            this._food.Hung = false;
                            this._catController.CatMoveManager.MoveToPoint(this._catController.CatMemory.GetNearestFoodPoint().transform.position, this._food.Satiety < 10);
                        }
                    }
                }

                this._catController.CatUI.SetFood(this._food.Satiety, 100);
            }
        }

        private IEnumerator EmotionalTimer()
        {
            while(true)
            {
                yield return new WaitForSeconds(.5f);

                this._emotional.Kindness -= this._emotional.KindnessAmountBySecond;
                this._emotional.Stress += this._emotional.StressAmountBySecond;

                if (this._emotional.Kindness < 90)
                {
                    // this._catController.CatMoveManager.MoveToPoint(this._catController.CatMemory.GetPlayerPosition(), true);
                }

                this._catController.CatUI.SetEmotional(this._emotional.Kindness, 100);
            }
        }

        private IEnumerator FatigueTimer()
        {
            while(true)
            {
                yield return new WaitForSeconds(.5f);

                if (!this._emotional.Busy && !this._food.Busy)
                {
                    if (!this._fatigue.Busy && !this._fatigue.Walked)
                    {
                        float x = UnityEngine.Random.Range(-10, 10);
                        float z = UnityEngine.Random.Range(-10, 10);

                        this._catController.CatMoveManager.MoveToPoint(this._fatigue.WalkTo = transform.position + new Vector3(x, 0, z));
                        this._fatigue.Walked = true;
                    }
                    else if (!this._catController.CatMoveManager.Moved)
                    {
                        this._fatigue.Walked = false;
                    }
                }

                this._catController.CatUI.SetFatigue(this._fatigue.Sleep, 100);
            }
        }

        public void UpEmotional (float kindness, float stress)
        {
            this._emotional.Kindness += kindness;
            this._emotional.Stress -= stress;
        }
    }
}
