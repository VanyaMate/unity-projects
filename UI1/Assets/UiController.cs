using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    public Button StartButton;
    public Button JustButton;
    public Label Text;

    [SerializeField] private float _counter;
    [SerializeField] private VisualElement _newElement;

    private void Start()
    {
        this._counter = 0;

        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        this.StartButton = document.Q<Button>("startButton");
        this.Text = document.Q<Label>("defaultLabel");

        this._newElement = new VisualElement();


        this.StartButton.clicked += () => {
            this.Text.text = (++this._counter).ToString();
        };
    }
}
