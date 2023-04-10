using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Managers.Path;
/*
 RobotLogic

    ----------------------------------
    
    -- Если в инвентаре робота есть товары на продажу 
        -- Если магазин пустой 
            -> доехать до магазина, выложить товары
        -- Если магазин полный
            -> доехать до пустого склада, выложить товары
    
    -- Если в инвентаре робота есть больше 10 семян
        -- Если есть незасеянное поле
            -> доехать до поля, высадить семена
    !! Если в инветране робота меньше 10 семян
        -- Если есть склад с семенами
            -> доехать до склада, забрать 100 семян
    
    
    
    
*/
namespace VM.Managers.Robots
{
    public class SeedlingsItemsData 
    {
        public PathNode node;
        public InventoryManager storage;
        public SO_InventorySeedsItem seed;
        public List<int> ids;
    }

    [Serializable]
    public class RobotAutotaskManager
    {
        [SerializeField] private List<SO_InventorySeedsItem> _seeds;

        public void OnUpdate()
        {
            List<RobotUnit> freeRobots = RobotManager.instance.GetFreeRobots();

            freeRobots.ForEach((robot) =>
            {
                Debug.Log("free robot: " + robot.name);
                float seedsAmount = robot.inventory.GetAmountItems("Еда");
                Debug.Log("seedsAmount: " + seedsAmount);
                if (seedsAmount > 0)
                {
                    bool fullOutputStore = this._OutputSeeds(
                        data: this._FindNodeWithEmptyStore((int)seedsAmount),
                        unit: robot,
                        amount: seedsAmount
                    );

                    Debug.Log("fullOutputStore: " + fullOutputStore);
                    if (fullOutputStore) return;

                    bool fullOutputStorage = this._OutputSeeds(
                        data: this._FindNodeWithEmptyStorage((int)seedsAmount),
                        unit: robot,
                        amount: seedsAmount
                    );

                    Debug.Log("fullOutputStorage: " + fullOutputStorage);
                    if (fullOutputStorage) return;

                    bool outputStore = this._OutputSeeds(
                        data: this._FindNodeWithEmptyStore(0),
                        unit: robot,
                        amount: seedsAmount
                    );

                    Debug.Log("outputStore: " + outputStore);
                    if (outputStore) return;

                    bool outputStorage = this._OutputSeeds(
                        data: this._FindNodeWithEmptyStorage(0),
                        unit: robot,
                        amount: seedsAmount
                    );

                    Debug.Log("outputStorage: " + outputStorage);
                    if (outputStorage) return;
                }

                PathNode nodeWithReadySeedlings = this._FindNodeWithReadySeedlings();
                Debug.Log("nodeWithSeedlings: " + nodeWithReadySeedlings);

                if (nodeWithReadySeedlings && seedsAmount == 0)
                {
                    nodeWithReadySeedlings.reserved = true;

                    robot.AddTask(
                        new RobotMoveTask(
                            unit: robot,
                            to: nodeWithReadySeedlings
                        )
                    );

                    robot.AddTask(
                        new RobotTakeSeedlingTask(
                            unit: robot,
                            node: nodeWithReadySeedlings
                        )
                    );

                    return;
                }

                float seedlingsAmount = robot.inventory.GetAmountItems("Семена");
                Debug.Log("seedlingsAmount: " + seedlingsAmount);

                if (seedlingsAmount > 10)
                {
                    PathNode placeForSeedlings = this._FindNodeWithEmptySeedlings();
                    Debug.Log("placeForSeedlings: " + placeForSeedlings);

                    if (placeForSeedlings)
                    {
                        placeForSeedlings.reserved = true;

                        robot.AddTask(
                            new RobotMoveTask(
                                unit: robot,
                                to: placeForSeedlings
                            )
                        );

                        robot.AddTask(
                            new RobotSeedlingTask(
                                unit: robot,
                                node: placeForSeedlings
                            )
                        );

                        return;
                    }
                }
                else
                {
                    float robotEmptySlots = robot.inventory.GetIndexEmptySlots().Count;
                    Debug.Log("robotEmptySlots: " + robotEmptySlots);
                    if (robotEmptySlots > 0)
                    {
                        SeedlingsItemsData nodeContainerWithSeedlings = this._FindSeedlingItemsInStores();
                        Debug.Log("nodeWithSeedlings: " + nodeContainerWithSeedlings.node);

                        if (nodeContainerWithSeedlings.node != null)
                        {
                            robot.AddTask(
                                new RobotMoveTask(
                                    unit: robot,
                                    to: nodeContainerWithSeedlings.node
                                )
                            );

                            robot.AddTask(
                                new RobotTakeTask(
                                    unit: robot,
                                    storage: nodeContainerWithSeedlings.storage,
                                    item: nodeContainerWithSeedlings.seed,
                                    amount: robot.robotType.takeSeedlingAmount
                                )
                            );

                            return;
                        }
                    }
                }

                if (robot.inventory.GetIndexEmptySlots().Count > 0)
                {
                    SeedlingsItemsData nodeStoreWithReadySeeds = this._FindNodeStoreWithReadySeeds();

                    if (nodeStoreWithReadySeeds.node != null)
                    {
                        robot.AddTask(
                            new RobotMoveTask(
                                unit: robot,
                                to: nodeStoreWithReadySeeds.node
                            )
                        );

                        nodeStoreWithReadySeeds.ids.ForEach((id) =>
                        {
                            robot.AddTask(
                                new RobotTakeTask(
                                    unit: robot,
                                    storage: nodeStoreWithReadySeeds.storage,
                                    item: nodeStoreWithReadySeeds.storage.Inventory[id].Type,
                                    amount: nodeStoreWithReadySeeds.storage.Inventory[id].Amount
                                )
                            );
                        });

                        return;
                    }

                    PathNode nodeWithReadySomeSeedlings = this._FindNodeWithReadySeedlings(0);
                    if (nodeWithReadySomeSeedlings)
                    {
                        nodeWithReadySomeSeedlings.reserved = true;

                        robot.AddTask(
                            new RobotMoveTask(
                                unit: robot,
                                to: nodeWithReadySomeSeedlings
                            )
                        );

                        robot.AddTask(
                            new RobotTakeSeedlingTask(
                                unit: robot,
                                node: nodeWithReadySomeSeedlings
                            )
                        );

                        return;
                    }
                }
            });
        }

        private bool _OutputSeeds (SeedlingsItemsData data, RobotUnit unit, float amount)
        {
            if (data.node != null)
            {
                data.storage.reservedSlots += amount;

                unit.AddTask(
                    new RobotMoveTask(
                        unit: unit,
                        to: data.node
                    )
                );

                unit.AddTask(
                    new RobotPutTask(
                        unit: unit,
                        storage: data.storage,
                        amount: amount
                    )
                );

                return true;
            }

            return false;
        }

        private SeedlingsItemsData _FindSeedlingItemsInStores ()
        {
            SeedlingsItemsData data = new SeedlingsItemsData();

            PathManager.instance.pathNodes.Find((pathNode) =>
            {
                if (pathNode.pathNodeType == PathType.Storage)
                {
                    return pathNode.storageConnects.Find((storage) =>
                    {
                        InventoryManager manager = ((InventoryItemStorage)storage.Manager).inventory;
                        SO_InventorySeedsItem seed = this._seeds.Find((seed) => manager.GetIndexSameItems(seed).Count > 0);

                        if (seed != null)
                        {
                            data.node = pathNode;
                            data.storage = manager;
                            data.seed = seed;

                            return true;
                        }

                        return false;
                    });
                }

                return false;
            });

            return data;
        }

        private PathNode _FindNodeWithEmptySeedlings ()
        {
            return PathManager.instance.pathNodes.Find((pathNode) =>
            {
                if (pathNode.pathNodeType == PathType.Seedling && pathNode.reserved == false)
                {
                    return pathNode.unusabledSeedlingsPlace.Count > 10;
                }

                return false;
            });
        }

        private PathNode _FindNodeWithReadySeedlings (float amount = 10)
        {
            return PathManager.instance.pathNodes.Find((pathNode) =>
            {
                if (pathNode.pathNodeType == PathType.Seedling && pathNode.reserved == false)
                {
                    return pathNode.seedlingsItems.Count > 0 && pathNode.seedlingsItems.FindAll((seedling) => seedling.ready).Count > amount;
                }
                else
                {
                    return false;
                }
            });
        }

        private SeedlingsItemsData _FindNodeStoreWithReadySeeds()
        {
            SeedlingsItemsData data = new SeedlingsItemsData();

            PathManager.instance.pathNodes.Find((pathNode) =>
            {
                if (pathNode.pathNodeType == PathType.Storage)
                {
                    return pathNode.storageConnects.Find((storage) =>
                    {
                        InventoryManager manager = ((InventoryItemStorage)storage.Manager).inventory;
                        List<int> itemsIds = manager.GetIndexSameType("Еда");

                        if (itemsIds.Count > 0)
                        {
                            data.storage = manager;
                            data.node = pathNode;
                            data.ids = itemsIds;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });
                }
                else
                {
                    return false;
                }
            });

            return data;
        }

        private SeedlingsItemsData _FindNodeWithEmptyStorage (int amount)
        {
            SeedlingsItemsData data = new SeedlingsItemsData();

            PathManager.instance.pathNodes.Find((pathNode) =>
            {
                if (pathNode.pathNodeType == PathType.Storage)
                {
                    return pathNode.storageConnects.Find((storage) => {
                        InventoryManager manager = ((InventoryItemStorage)storage.Manager).inventory;
                        Debug.Log("NodeWithEmptyStorage: " + manager.GetIndexEmptySlots(withoutReserved: false).Count);
                        Debug.Log("NodeWithEmptyStorage_Reserved: " + manager.reservedSlots);
                        if (manager.GetIndexEmptySlots(withoutReserved: false).Count > amount)
                        {
                            data.storage = manager;
                            data.node = pathNode;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });
                }
                else
                {
                    return false;
                }
            });

            return data;
        }

        private SeedlingsItemsData _FindNodeWithEmptyStore (int amount)
        {
            SeedlingsItemsData data = new SeedlingsItemsData();

            PathManager.instance.pathNodes.Find((pathNode) =>
            {
                if (pathNode.pathNodeType == PathType.Store)
                {
                    return pathNode.storageConnects.Find((storage) => {
                        InventoryManager manager = ((InventoryItemStorage)storage.Manager).inventory;
                        Debug.Log("NodeWithEmptyStore: " + manager.GetIndexEmptySlots(withoutReserved: false).Count);
                        Debug.Log("NodeWithEmptyStore_Reserved: " + manager.reservedSlots);
                        if (manager.GetIndexEmptySlots(withoutReserved: false).Count > amount)
                        {
                            data.storage = manager;
                            data.node = pathNode;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });
                }
                else
                {
                    return false;
                }
            });

            return data;
        }
    }
}