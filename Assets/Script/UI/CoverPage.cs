using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CoverPage : MonoBehaviour
{
    private VisualElement root;
    private void Start() {
        root = GetComponent<UIDocument>().rootVisualElement;
        Button startButton = root.Q<Button>("StartButton");
        startButton.clicked += ()=> {
            root.style.display = DisplayStyle.None;
            GameManager.Instance.StartGame();
        };
    }

    


}
