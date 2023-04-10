using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using VM.Inventory.Items;
using VM.Managers;
using VM.Seeds;

namespace VM.UI.Seedling
{
    public class SeedlingInfoUI : MonoBehaviour
    {
        [SerializeField] private float _showInfoDistance;
        [SerializeField] private SeedlingInfoPointUI _prefab;

        private SeedlingsManager _seedlingsManager;
        private Camera _camera;
        private Dictionary<InventoryItemSeedling, SeedlingInfoPointUI> _points = 
            new Dictionary<InventoryItemSeedling, SeedlingInfoPointUI>();

        private void Start()
        {
            this._seedlingsManager = SeedlingsManager.instance;
            this._camera = Camera.main;
        }

        private void Update()
        {
            /*            this._seedlingsManager.seedlings.ForEach((seedling) =>
                        {
                            seedling.seedsItemObject.canvas.transform.rotation =
                                Quaternion.LookRotation(
                                    this._camera.transform.position - seedling.OnScene.transform.position
                                );
                        });*/

            int seedlingsCount = this._seedlingsManager.seedlings.Count;
            NativeArray<int> seedlingIndexes = new NativeArray<int>(seedlingsCount, Allocator.TempJob);

            for (int i = 0; i < seedlingsCount; i++)
            {
                seedlingIndexes[i] = i;
            }

            RotateCanvasJob rotateJob = new RotateCanvasJob
            {
                cameraPosition = this._camera.transform.position,
                seedlings = seedlingIndexes
            };

            TransformAccessArray transforms = new TransformAccessArray(
                this._seedlingsManager.seedlings.ConvertAll<Transform>(x => x.seedsItemObject.canvas.transform).ToArray(), 
                4
            );

            JobHandle job = rotateJob.Schedule(transforms);
            job.Complete();
            seedlingIndexes.Dispose();
            transforms.Dispose();
        }

        private void _tempSeedlingShowInfo ()
        {
            /*Vector3 screenPosition = this._camera.WorldToScreenPoint(seedling.OnScene.transform.position);

                Color seedlingStatus;

                if (seedling.damaged)
                {
                    seedlingStatus = SeedlingInfoPointStatus.error;
                }
                else if (seedling.ready)
                {
                    seedlingStatus = SeedlingInfoPointStatus.good;
                }
                else
                {
                    seedlingStatus = SeedlingInfoPointStatus.neutral;
                }

                if (this._points.TryGetValue(seedling, out SeedlingInfoPointUI ui))
                {
                    ui.UpdateInfo(seedlingStatus, seedling.progress);
                    ui.SetPosition(screenPosition);
                }
                else
                {
                    SeedlingInfoPointUI info = Instantiate(
                        this._prefab,
                        transform
                    );

                    info.SetInfo(
                        icon: seedling.Type.Icon, 
                        status: seedlingStatus, 
                        progress: seedling.progress
                    );

                    info.SetPosition(screenPosition);

                    this._points.Add(seedling, info);
                }*/

/*            List<InventoryItemSeedling> toDelete = new List<InventoryItemSeedling>();
            foreach (KeyValuePair<InventoryItemSeedling, SeedlingInfoPointUI> pair in this._points)
            {
                if (!pair.Key.placed)
                {
                    toDelete.Add(pair.Key);
                }
            }
            toDelete.ForEach((item) => {
                Destroy(this._points[item].gameObject);
                this._points.Remove(item);
            });*/
        }
    }

    public struct RotateCanvasJob : IJobParallelForTransform
    {
        public Vector3 cameraPosition;
        public NativeArray<int> seedlings;

        public void Execute (int index, TransformAccess transform)
        {
            transform.rotation = Quaternion.LookRotation(
                cameraPosition - transform.position
            );
        }
    }
}