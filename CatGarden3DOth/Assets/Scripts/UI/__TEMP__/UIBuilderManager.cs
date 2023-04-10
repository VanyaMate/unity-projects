using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace CG.UI
{
    public class UIBuilderGroupItem
    {
        public string ButtonName;
        public Sprite ButtonIcon;
        public UnityAction ButtonEvent;
    }

    public class UIBuilderGroupData
    {
        public string ButtonName;
        public Sprite ButtonIcon;
        public List<UIBuilderGroupItem> GroupItems;
    }

    public class UIBuilderManager : MonoBehaviour
    {
        public static UIBuilderManager Instance;

        [SerializeField] private UIDocument _uiDoc;

        [Header("Intstuments Groups")]
        [SerializeField] private VisualTreeAsset _listGroups;
        [SerializeField] private VisualTreeAsset _listOfGroup;
        [SerializeField] private VisualTreeAsset _groupItem;

        private VisualElement _root;

        private GroupBox _openGroupsButtons;
        private GroupBox _groupsContainer;

        private Dictionary<Button, GroupBox> _groupsList = new Dictionary<Button, GroupBox>();


        private void Awake()
        {
            Instance = this;

            this._root = this._uiDoc.rootVisualElement;
            this._openGroupsButtons = this._root.Q<GroupBox>("openGroupsButtons");
            this._groupsContainer = this._root.Q<GroupBox>("groupsLists");
        }

        public void RenderInstrumentsGroup(UIBuilderGroupData groupData)
        {
            GroupBox listOfGroup = this._listOfGroup.CloneTree().Q<GroupBox>("listOfGroup");
            Button groupUpButton = this._groupItem.CloneTree().Q<Button>("listGroupsItem");

            groupUpButton.text = groupData.ButtonName;
            groupUpButton.style.backgroundImage = new StyleBackground(groupData.ButtonIcon);
            groupUpButton.clicked += () =>
            {
                bool toHide = !listOfGroup.ClassListContains("hide");

                if (toHide)
                {
                    listOfGroup.AddToClassList("hide");
                } else
                {
                    foreach (KeyValuePair<Button, GroupBox> groups in this._groupsList)
                    {
                        groups.Value.AddToClassList("hide");
                    }

                    listOfGroup.RemoveFromClassList("hide");
                }
            };

            this._groupsList.Add(groupUpButton, listOfGroup);

            groupData.GroupItems.ForEach((UIBuilderGroupItem i) =>
            {
                Button groupButtonItem = this._groupItem.CloneTree().Q<Button>("listGroupsItem");
                groupButtonItem.text = i.ButtonName;
                groupButtonItem.style.backgroundImage = new StyleBackground(i.ButtonIcon);
                groupButtonItem.clicked += () => i.ButtonEvent();
                listOfGroup.Add(groupButtonItem);
            });

            this._openGroupsButtons.Add(groupUpButton);
            this._groupsContainer.Add(listOfGroup);
        }
    }
}
