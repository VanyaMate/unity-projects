using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.UI.Inventory
{
    public class HandsManagerUI : MonoBehaviour
    {
        public static HandsManagerUI instance;

        [SerializeField] private HandsItemListUI _itemsList;

        private void Awake()
        {
            instance = this;
        }

        public void Show (List<HandsItemInfo> list)
        {
            this._itemsList.gameObject.SetActive(true);
            this._itemsList.Add(list);
        }

        public void Hide ()
        {
            this._itemsList.gameObject.SetActive(false);
            this._itemsList.RemoveAll();
        }
    }
}