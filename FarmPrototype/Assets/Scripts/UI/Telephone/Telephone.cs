using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VM.UI.Telephone
{
    public class Telephone : MonoBehaviour
    {
        [Header("Store")]
        [SerializeField] private Button _storeButton;
        [SerializeField] private GameObject _storeMenu;

        [Header("Bank")]
        [SerializeField] private Button _bankButton;
        [SerializeField] private GameObject _bankMenu;

        [Header("HH")]
        [SerializeField] private Button _hhButton;
        [SerializeField] private GameObject _hhMenu;

        [Header("SMS")]
        [SerializeField] private Button _smsButton;
        [SerializeField] private GameObject _smsMenu;

        [Header("Camera")]
        [SerializeField] private Button _cameraButton;
        [SerializeField] private GameObject _cameraMenu;

        [Header("Settings")]
        [SerializeField] private Button _settingsButton;
        [SerializeField] private GameObject _settingsMenu;

        [Header("Other")]
        [SerializeField] private GameObject _menu;
        [SerializeField] private Button _homeButton;

        private void Awake()
        {
            this._storeButton.onClick.AddListener(() => this._ActivateApp(this._storeMenu));
            this._bankButton.onClick.AddListener(() => this._ActivateApp(this._bankMenu));
            this._hhButton.onClick.AddListener(() => this._ActivateApp(this._hhMenu));
            this._smsButton.onClick.AddListener(() => this._ActivateApp(this._smsMenu));
            this._cameraButton.onClick.AddListener(() => this._ActivateApp(this._cameraMenu));
            this._settingsButton.onClick.AddListener(() => this._ActivateApp(this._settingsMenu));

            this._homeButton.onClick.AddListener(this._HomeButtonClickHandler);
            this._HomeButtonClickHandler();
        }

        private void _ActivateApp (GameObject app)
        {
            this._menu.SetActive(false);
            app.SetActive(true);
        }

        private void _HomeButtonClickHandler ()
        {
            this._storeMenu.SetActive(false);
            this._bankMenu.SetActive(false);
            this._hhMenu.SetActive(false);
            this._smsMenu.SetActive(false);
            this._cameraMenu.SetActive(false);
            this._settingsMenu.SetActive(false);

            this._menu.SetActive(true);
        }
    }
}