using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Button backToGameButton;
    [SerializeField] private Button backToMainMenuButton;

    private void Start()
    {
        backToGameButton.onClick.AddListener(BackToGameButtonClicked);
        backToMainMenuButton.onClick.AddListener(BackToMainMenuButtonClicked);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            mainPanel.SetActive(true);
        }
    }

    private void BackToGameButtonClicked()
    {
        mainPanel.SetActive(false);
    }

    private void BackToMainMenuButtonClicked()
    {
        SceneLoader.LoadScene(SceneNames.StartMenuScene);
    }
}