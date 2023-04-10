using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VM.Managers.Path;
using VM.Managers.Robots;

namespace VM.UI.Telephone
{
    public class TelephoneRobotsMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _robotsButton;
        [SerializeField] private Button _pathsButton;

        [Header("Robots")]
        [SerializeField] private Transform _robotsMenu;
        [SerializeField] private Transform _robotsContainer;
        [SerializeField] private TelephoneRobotItem _itemRobotsPrefab;

        [Header("Paths")]
        [SerializeField] private Transform _pathsMenu;
        [SerializeField] private Toggle _redactorPathsToggle;
        [SerializeField] private Toggle _redactorPathsStorageToggle;
        [SerializeField] private Toggle _showPathsToggle;

        private List<TelephoneRobotItem> _robotItems = new List<TelephoneRobotItem>(); 

        private void Start()
        {
            this._robotsButton.onClick.AddListener(this._RobotsMenuButtonHandler);
            this._pathsButton.onClick.AddListener(this._PathMenuButtonHandler);

            this._redactorPathsToggle.isOn = PathManager.instance.redactedPathNode;
            this._redactorPathsStorageToggle.isOn = PathManager.instance.redactedStorageNode;
            this._showPathsToggle.isOn = PathManager.instance.showLines;

            this._redactorPathsToggle.onValueChanged.AddListener(PathManager.instance.Enable);
            this._redactorPathsStorageToggle.onValueChanged.AddListener(PathManager.instance.EnableStorageRedact);
            this._showPathsToggle.onValueChanged.AddListener(PathManager.instance.EnableLines);
        }

        private void _PathMenuButtonHandler ()
        {
            this.HideAllMenus();
            this._pathsMenu.gameObject.SetActive(true);
        }

        private void _RobotsMenuButtonHandler ()
        {
            this.HideAllMenus();
            this._robotsMenu.gameObject.SetActive(true);

            this._robotItems.ForEach((item) => Destroy(item.gameObject));
            this._robotItems = new List<TelephoneRobotItem>();

            RobotManager.instance.units.ForEach((unit) =>
            {
                TelephoneRobotItem item = Instantiate(this._itemRobotsPrefab, this._robotsContainer);

                item.SetInfo(
                    icon: unit.robotType.Icon,
                    name: unit.robotType.Name,
                    status: unit.worked.ToString()
                );
                this._robotItems.Add(item);
            });
        }

        public void HideAllMenus ()
        {
            this._robotsMenu.gameObject.SetActive(false);
            this._pathsMenu.gameObject.SetActive(false);
        }
    }
}