using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VM.UI.Inventory
{
    public class HandsItemInfo
    {
        public string text;
        public UnityAction action;
    }

    public class HandsItemListUI : MonoBehaviour
    {
        [SerializeField] private HandsItemButtonUI _template;

        private List<HandsItemButtonUI> _buttons = new List<HandsItemButtonUI>();

        public void Add (List<HandsItemInfo> items)
        {
            this.RemoveAll();
            items.ForEach((item) =>
            {
                HandsItemButtonUI button = Instantiate(this._template, transform);
                button.Set(item);

                this._buttons.Add(button);
            });
        }

        public void RemoveAll ()
        {
            this._buttons.ForEach(x => Destroy(x.gameObject));
            this._buttons = new List<HandsItemButtonUI>();
        }
    }
}