using BayatGames.SaveGameFree;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UiStartMenu : MonoBehaviour
{
    public static Action OnNewGameClicked;
    public static Action OnContinueGameClicked;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;

    private void Start()
    {
        newGameButton.onClick.AddListener(NewGameButtonClicked);
        continueGameButton.onClick.AddListener(ContinueGameButtonClicked);
        if (!SaveGame.Exists(GEM.MapSaveName))
        {
            continueGameButton.interactable = false;
        }
    }

    private void NewGameButtonClicked()
    {
        OnNewGameClicked?.Invoke();
    }

    private void ContinueGameButtonClicked()
    {
        OnContinueGameClicked?.Invoke();
        SceneLoader.LoadScene(SceneNames.MainGameScene);
    }
}
