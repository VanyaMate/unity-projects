using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VM.Managers.Path;

namespace VM.UI.Path
{
    public class PathNodeMenu : MonoBehaviour
    {
        public static PathNodeMenu instance;

        [SerializeField] private TMP_Dropdown _pathType;
        [SerializeField] private Button _closeButton;

        private PathNode _node;

        private void Awake()
        {
            instance = this;
            this._FillOptions();
            this._pathType.onValueChanged.AddListener(this._UpdatePathNode);
            this._closeButton.onClick.AddListener(this.Hide);
            this.Hide();
        }

        public void SetPathNode (PathNode node)
        {
            this._node = node;
        }

        public void Show ()
        {
            if (this._node != null)
            {
                this._pathType.captionText.text = this._node.pathNodeType.ToString();
                gameObject.SetActive(true);
            }
        }

        public void Hide ()
        {
            this._node = null;
            gameObject.SetActive(false);
        }

        private void _FillOptions ()
        {
            this._pathType.ClearOptions();
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach (PathType type in Enum.GetValues(typeof(PathType)))
            {
                options.Add(new TMP_Dropdown.OptionData()
                {
                    text = type.ToString(),
                });
            }

            this._pathType.AddOptions(options);
        }

        private PathType _GetPathTypeByName (string type)
        {
            return Enum.Parse<PathType>(type);
        }

        private void _UpdatePathNode (int _)
        {
            if (this._node == null) return;

            string typeName = this._pathType.captionText.text;
            PathType type = this._GetPathTypeByName(typeName);
            this._node.SetPathNodeType(type);
        }
    }
}