using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Managers.Customers;
using VM.Managers.EntityTools;
using VM.Managers.Store;

namespace VM.Managers.Road
{
    public class RoadCarStatus
    {
        public static int Stop = 0;
        public static int Go = 1;
    }

    [Serializable]
    public class RoadCarPlace
    {
        public Transform insidePlace;
        public Transform outsidePlace;
        public EntityUnit enitity;
    }

    public class RoadCar : MonoBehaviour
    {
        [Header("Path")]
        [SerializeField] private RoadLine _roadLine;

        [Header("Places")]
        [SerializeField] private List<RoadCarPlace> _carPlaces = new List<RoadCarPlace>();

        [Header("Props")]
        [SerializeField] private float _speed = 5;
        [SerializeField] private int _status = 0;
        [SerializeField] private bool _purchased = false;

        [Header("Entities")]
        [SerializeField] private List<EntityUnit> _carEntities = new List<EntityUnit>();

        public List<RoadCarPlace> carPlaces => _carPlaces;

        private void Awake()
        {

        }

        private void Update()
        {
            if (this._status == RoadCarStatus.Go)
            {
                Vector3 direction = this._roadLine.finish.position - this._roadLine.start.position;
                Vector3 translateDistance = direction.normalized * Time.deltaTime * this._speed;

                float distanceToFinish = Vector3.Distance(
                    transform.position, 
                    this._roadLine.finish.position
                );

                if (distanceToFinish < 2f)
                {
                    Destroy(gameObject);
                }
                else
                {
                    transform.Translate(translateDistance, Space.World);
                }

                // TEMP
                if (!this._purchased)
                {
                    RaycastHit[] hitsL = Physics.RaycastAll(transform.position + Vector3.up / 2, -transform.right, 25f);
                    RaycastHit[] hitsR = Physics.RaycastAll(transform.position + Vector3.up / 2, transform.right, 25f);

                    for (int i = 0; i < hitsL.Length; i++)
                    {
                        if (hitsL[i].transform.gameObject.layer == 10)
                        {
                            if (hitsL[i].transform.TryGetComponent<StoreManagerObject>(out StoreManagerObject store))
                            {
                                this._purchased = true;

                                for (int j = 0; j < this._carEntities.Count; j++)
                                {
                                    Customer customer = (Customer)(this._carEntities[j]);
                                    List<CustomerWant> wants = customer.wants;
                                    wants.ForEach((want) =>
                                    {
                                        store.PurchaseItems(want.itemType, want.amount);
                                    });
                                }

                                break;
                            }
                        }
                    }

                    for (int i = 0; i < hitsR.Length; i++)
                    {
                        if (hitsR[i].transform.gameObject.layer == 10)
                        {
                            if (hitsR[i].transform.TryGetComponent<StoreManagerObject>(out StoreManagerObject store))
                            {
                                this._purchased = true;

                                for (int j = 0; j < this._carEntities.Count; j++)
                                {
                                    Customer customer = (Customer)(this._carEntities[j]);
                                    List<CustomerWant> wants = customer.wants;
                                    wants.ForEach((want) =>
                                    {
                                        store.PurchaseItems(want.itemType, want.amount);
                                    });
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        public void Go ()
        {
            if (this._CheckEntitiesInCar())
            {
                this._status = RoadCarStatus.Go;
            }
        }

        public void Stop ()
        {
            this._status = RoadCarStatus.Stop;
        }

        public void SetRoad (RoadLine line)
        {
            this._roadLine = line;
            transform.localPosition = line.start.localPosition;
            transform.forward = line.finish.position - line.start.position;
        }

        public void SetEntities (List<EntityUnit> entities)
        {
            this._carEntities = entities;
            this._carEntities.ForEach((ent) =>
            {
                this.SeetInside(ent);
            });
        }

        public void AddEntity (EntityUnit entity, bool seetInside = true)
        {
            this._carEntities.Add(entity);

            if (seetInside)
            {
                this.SeetInside(entity);
            }
        }

        public bool SeetInside (EntityUnit entity)
        {
            for (int i = 0; i < this._carPlaces.Count; i++)
            {
                if (this._carPlaces[i].enitity == null)
                {
                    this._carPlaces[i].enitity = entity;
                    entity.insideCar = true;
                    entity.transform.parent = this._carPlaces[i].insidePlace;
                    entity.transform.position = this._carPlaces[i].insidePlace.position;
                    return true;
                }
            }

            return false;
        }

        public void GetOut (EntityUnit entity)
        {
            for (int i = 0; i < this._carPlaces.Count; i++)
            {
                if (this._carPlaces[i].enitity == entity)
                { 
                    this._carPlaces[i].enitity = null;
                    entity.insideCar = false;
                    entity.transform.parent = this._carPlaces[i].outsidePlace;
                    entity.transform.localPosition = Vector3.zero;
                }
            }
        }

        private bool _CheckEntitiesInCar ()
        {
            return this._carEntities.TrueForAll((x) => ((Customer)x).insideCar);
        }
    }
}