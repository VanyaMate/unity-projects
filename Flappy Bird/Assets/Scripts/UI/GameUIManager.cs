using FB.Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace FB.UI
{
    public class GameUIManager : MonoBehaviour
    {
        private UIDocument _document;
        private VisualElement _root;
        private Label _scoreText;
        private Button _settingsButton;
        private Button _reloadButton;

        private void Awake()
        {
            this._document = GetComponent<UIDocument>();
            this._root = this._document.rootVisualElement;

            this._scoreText = this._root.Q<Label>("ScoreText");
            this._settingsButton = this._root.Q<Button>("SettingsButton");
            this._reloadButton = this._root.Q<Button>("ReloadButton");

            this._reloadButton.clicked += () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                this._reloadButton.Blur();
            };
        }

        private void Start()
        {
            Debug.Log(ScoreManager.Instance);
            ScoreManager.Instance.OnScoreChange.AddListener((float score) => this.UpdateScore(score));
        }

        private void UpdateScore(float score)
        {
            this._scoreText.text = score.ToString();
        }
    }
}
