using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.Cat
{
    public class CatMemory : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Vector3 _lastPlayerPosition;
        [SerializeField] private Transform _playerInTrigger;
        [SerializeField] private bool _playerIsVisibled;

        [Header("Food")]
        private HashSet<FoodPoint> _foodPoints = new HashSet<FoodPoint>();

        private void OnTriggerEnter(Collider other)
        {
            // Save player in trigger
            if (other.gameObject.name == "Player")
            {
                Debug.Log("Is Player");
                this._playerInTrigger = other.transform;
            } else

            // Save food positions
            if (other.gameObject.name == "FoodPoint")
            {
                Debug.Log("Is FoodPoint");
                this._foodPoints.Add(other.transform.GetComponent<FoodPoint>());
            }
        }

        private void OnTriggerStay(Collider other)
        {
            // Check visible if player is trigger
            if (this._playerInTrigger != null)
            {
                Debug.DrawRay(
                    transform.position,
                    (this._playerInTrigger.position - transform.position).normalized * 10,
                    Color.red,
                    1f
                );

                RaycastHit hit;

                this._playerIsVisibled = Physics.Raycast(
                    transform.position,
                    (this._playerInTrigger.position - transform.position).normalized, 
                    out hit,
                    10f
                );

                if (hit.transform.gameObject.name == "Player")
                {
                    this._playerIsVisibled = true;
                    this._lastPlayerPosition = this._playerInTrigger.position;
                }
                else
                {
                    this._playerIsVisibled = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "Player")
            {
                // Save last player position
                this._lastPlayerPosition = this._playerInTrigger.position;
                // Delete player in trigger
                this._playerInTrigger = default;
            }
        }

        public FoodPoint GetNearestFoodPoint ()
        {
            FoodPoint nearestFoodPoint = null;
            float minDistance = 10000;

            foreach (FoodPoint foodPoint in this._foodPoints)
            {
                float distance = Vector3.Distance(foodPoint.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestFoodPoint = foodPoint;
                }
            }

            return nearestFoodPoint;
        }

        public Vector3 GetPlayerPosition ()
        {
            return this._lastPlayerPosition;
        }
    }
}