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
        newGameButton.onClick.AddListener(ContinueGameButtonClicked);
    }

    private void NewGameButtonClicked()
    {
        OnNewGameClicked?.Invoke();
    }

    private void ContinueGameButtonClicked()
    {
        OnContinueGameClicked?.Invoke();
    }
}
