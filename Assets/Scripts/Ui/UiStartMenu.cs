using BayatGames.SaveGameFree;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiStartMenu : MonoBehaviour
{
    public static Action OnNewGameClicked;
    public static Action OnContinueGameClicked;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private TextMeshProUGUI loadingText;

    private void Start()
    {
        //This will not display no debug log other than editor
        if (!Application.isEditor)
        {
            GEM.ShowDebug = false;
            GEM.ShowDebugWarning = false;
            GEM.ShowDebugError = false;
        }
        loadingText.gameObject.SetActive(false);
        newGameButton.onClick.AddListener(NewGameButtonClicked);
        continueGameButton.onClick.AddListener(ContinueGameButtonClicked);
        if (!SaveGame.Exists(GEM.MapSaveName))
        {
            continueGameButton.interactable = false;
        }
    }

    private void NewGameButtonClicked()
    {
        SetLoadingText();
        OnNewGameClicked?.Invoke();
    }

    private void ContinueGameButtonClicked()
    {
        SetLoadingText();
        OnContinueGameClicked?.Invoke();
        SceneLoader.LoadScene(SceneNames.MainGameScene);
    }

    private void SetLoadingText()
    {
        newGameButton.gameObject.SetActive(false);
        continueGameButton.gameObject.SetActive(false);
        loadingText.gameObject.SetActive(true);
    }
}
