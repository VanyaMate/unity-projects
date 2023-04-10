using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Managers.Customers;

namespace VM.Managers.Road
{
    [Serializable]
    public class RoadLine
    {
        public Transform start;
        public Transform finish;
    }

    public class RoadManager : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private RoadCar _carPrefab;

        [Header("Roads")]
        [SerializeField] private RoadLine _nearLine;
        [SerializeField] private RoadLine _farLine;

        [Header("Props")]
        [SerializeField] private float _spawnTime;

        private float _spawnTimeSaved;

        private void Awake()
        {
            this._spawnTimeSaved = this._spawnTime;
        }

        private void Start()
        {
            StartCoroutine(this._SpawnRandomCar());
        }

        public void SpawnCar(RoadCar carType, RoadLine line)
        {
            RoadCar car = Instantiate(carType, transform);

            int customersCount = UnityEngine.Random.Range(1, car.carPlaces.Count + 1);

            for (int i = 0; i < customersCount; i++)
            {
                Customer customer = CustomersManager.instance.GetCustomer();
                customer.SetCar(car, true);
                car.AddEntity(customer);
            }

            car.SetRoad(line);
            car.Go();
        }

        private IEnumerator _SpawnRandomCar ()
        {
            while (true)
            {
                yield return new WaitForSeconds(this._spawnTime);

                int line = UnityEngine.Random.Range(0, 2);

                this.SpawnCar(this._carPrefab, line == 0 ? this._nearLine : this._farLine);

                float half = this._spawnTimeSaved - 1f;
                this._spawnTime = UnityEngine.Random.Range(this._spawnTimeSaved - half, this._spawnTimeSaved + half);
            }
        }
    }
}