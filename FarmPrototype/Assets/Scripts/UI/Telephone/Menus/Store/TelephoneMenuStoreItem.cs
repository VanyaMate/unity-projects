using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VM.Inventory;
using VM.Player;

namespace VM.UI.Telephone
{
    public class TelephoneMenuStoreItem : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_InputField _count;
        [SerializeField] private Button _purchaseButton;
        
        private SO_InventoryItem _type;

        public void Set(SO_InventoryItem type)
        {
            this._type = type;
            this._image.sprite = this._type.Icon;
            this._title.text = $"{this._type.Name} ({this._type.MaxAmount}רע.)";
            this._price.text = $"${this._type.Cost.ToString()}";
            this._count.text = "1";

            this._purchaseButton.onClick.AddListener(() =>
            {
                Debug.Log("Purchase: " + this._type.Name + ", " + this._count.text);

                int count = int.Parse(this._count.text);
                bool hasMoney = PlayerManager.Instance.moneyManager.Get(count * this._type.Cost, out float lacks);

                if (hasMoney)
                {
                    for (int i = 0; i < count; i++)
                    {
                        InventoryItemObject item = Instantiate(this._type.Prefab, transform);
                        item.SetItemType(this._type, this._type.MaxAmount);
                        if (PlayerManager.Instance.pockets.Manager.Add(item.Manager))
                        {
                            item.Manager.RemoveFromScene();
                        }
                        else
                        {
                            PlayerManager.Instance.moneyManager.money += this._type.Cost;
                        }
                    }
                }
            });
        }

        public void Destroy()
        {
            this._purchaseButton.onClick.RemoveAllListeners();
            Destroy(gameObject);
        }
    }
}