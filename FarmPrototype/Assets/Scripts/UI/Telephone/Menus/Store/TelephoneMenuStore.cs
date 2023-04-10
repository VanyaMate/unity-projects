using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VM.Inventory;

namespace VM.UI.Telephone
{
    public class TelephoneMenuStore : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private Transform _contentContainer;
        [SerializeField] private TelephoneMenuStoreItem _itemPrefab;

        private List<TelephoneMenuStoreItem> _items = new List<TelephoneMenuStoreItem>();

        private void Start()
        {
            this._GetListOfGoodsTypes().ForEach((type) =>
            {
                this._dropdown.options.Add(new TMP_Dropdown.OptionData()
                {
                    text = type
                });
            });

            this._dropdown.onValueChanged.AddListener(this._UpdateItemsList);
            this._UpdateItemsList(0);
        }

        private void _UpdateItemsList (int type)
        {
            this._items.ForEach((x) => x.Destroy());
            this._items = new List<TelephoneMenuStoreItem>();

            if (type == 0)
            {
                InventoryListOfTypes.Instance.items.ForEach(this._InstantiateItem);
            }
            else
            {
                this._GetListOfTypesByTypeName(this._dropdown.captionText.text).ForEach(this._InstantiateItem);
            }
        }

        private void _InstantiateItem (SO_InventoryItem item)
        {
            if (item.AvailInStore)
            {
                TelephoneMenuStoreItem storeItem = Instantiate(
                    this._itemPrefab,
                    this._contentContainer.transform
                );

                storeItem.Set(item);
                this._items.Add(storeItem);
            }
        }

        private List<SO_InventoryItem> _GetListOfTypesByTypeName (string typeName)
        {
            List<SO_InventoryItem> items = new List<SO_InventoryItem>();

            InventoryListOfTypes.Instance.items.ForEach((item) =>
            {
                if (item.CommonType == typeName && item.AvailInStore)
                {
                    items.Add(item);
                }
            });

            return items;
        }

        private List<string> _GetListOfGoodsTypes()
        {
            HashSet<string> hash = new HashSet<string>();

            InventoryListOfTypes.Instance.items.ForEach((item) =>
            {
                if (item.AvailInStore)
                {
                    hash.Add(item.CommonType);
                }
            });

            return new List<string>(hash);
        }
    }
}