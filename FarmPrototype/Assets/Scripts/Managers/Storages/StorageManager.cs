using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Player;
using VM.Seeds;

namespace VM.Managers
{
    public class StorageManager : MonoBehaviour
    {
        public static StorageManager instance;

        [SerializeField] private GhostObject _takeAreaGhost;
        private bool _takeAreaGhostActive = false;

        public GhostObject takeAreaGhost => _takeAreaGhost;
        public bool takeAreaGhostArea { get => _takeAreaGhostActive; set => _takeAreaGhostActive = value; }

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (this._takeAreaGhostActive)
            {
                if (Utils.MouseOverGameObject)
                {
                    bool isTerrain =
                        Utils.MouseWorldPosition.transform &&
                        Utils.MouseWorldPosition.transform.gameObject &&
                        Utils.MouseWorldPosition.transform.gameObject.layer == 7;
                    if (isTerrain)
                    {
                        this._takeAreaGhost.Enable(status: true);
                        this._takeAreaGhost.SetPosition(
                            position: Utils.MouseWorldPosition.point,
                            quaternion: Quaternion.identity
                        );

                        if (Input.GetMouseButtonDown(0))
                        {
                            Collider[] items = Physics.OverlapSphere(
                                Utils.MouseWorldPosition.point,
                                .5f,
                                LayerMask.GetMask("InventoryItem", "Seedling")
                            );

                            for (int i = 0; i < items.Length; i++)
                            {
                                if (items[i].TryGetComponent<InventoryItemObject>(out InventoryItemObject item))
                                {
                                    InventoryItem handsItem = PlayerManager.Instance.hands.inventoryObject.Manager.Inventory[0];
                                    SO_InventoryStorageItem storageType = (SO_InventoryStorageItem)handsItem.Type;
                                    List<string> storageForTypes = storageType.storageType.ItemsForContainer.ConvertAll<string>((type) => type.Type);
                                    string findedType = storageForTypes.Find((type) => type == item.Manager.Type.Type);

                                    if (findedType != null)
                                    {
                                        if (item.Manager.Type.Type == "Саженец")
                                        {
                                            InventoryItemSeedling seedling = ((InventoryItemSeedling)item.Manager);

                                            seedling.Take(addToHands: true);
                                            continue;
                                        }

                                        bool addedToInventory = ((InventoryItemStorage)handsItem).inventory.Add(item.Manager);

                                        if (addedToInventory)
                                        {
                                            item.Manager.RemoveFromScene();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this._takeAreaGhost.Enable(status: false);
                    }
                }
            }
        }
    }
}