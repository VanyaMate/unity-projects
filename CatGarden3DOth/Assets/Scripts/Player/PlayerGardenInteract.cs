using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.Garden;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace CG.Player
{
    public class PlayerGardenInteract : MonoBehaviour
    {
        private void Update()
        {
            // Если происходит засев
            if (CG.Garden.GardenManager.Instance.CurrentSeedlingType != null)
            {
                this.InteractWith("GardenSelect", (Vector3 position) =>
                {
                    GardenPoint gardenPoint =
                        CG.Garden.GardenManager.Instance.GetGardenPoint(position);

                    gardenPoint.SetGardenSeedling(CG.Garden.GardenManager.Instance.CurrentSeedlingType);
                }, true);
            }
            // Если делается грядка
            else if (CG.Garden.GardenManager.Instance.MakeGarden)
            {
                this.InteractWith("Map", (Vector3 position) =>
                {
                    CG.Garden.GardenManager.Instance.AddGarden(position);
                });
            }

            // Если поливается
            else if (CG.Garden.GardenManager.Instance.WaterGarden)
            {
                this.InteractWith("GardenSelect", (Vector3 position) =>
                {
                    GardenPoint gardenPoint =
                        CG.Garden.GardenManager.Instance.GetGardenPoint(position);

                    gardenPoint.WaterGarden(50);
                }, true);
            }

            // Отменить всё
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CG.Garden.GardenManager.Instance.HideGardenGhost();
            }
        }

        private void InteractWith(string gameObjectName, UnityAction<Vector3> action, bool checkObjectPosition = false)
        {
            RaycastHit rayHit = Common.Utils.GetMouseWorldHit();

            if (rayHit.transform != null && rayHit.transform.gameObject.name == gameObjectName)
            {
                Vector3 gardenPlace = CG.Garden.GardenManager.Instance.GetGardenSetPoint(checkObjectPosition ? rayHit.transform.position : rayHit.point);
                CG.Garden.GardenManager.Instance.ShowGardenGhost(gardenPlace);

                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    action(gardenPlace);
                }
            }
            else
            {
                CG.Garden.GardenManager.Instance.HideGardenGhost();
            }
        }
    }
}