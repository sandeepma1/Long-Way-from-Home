using UnityEngine;
using UnityEngine.UI;

public class UiInGameMenu : MonoBehaviour
{
    [SerializeField] private Button backToMainMenuButton;

    private void Start()
    {
        backToMainMenuButton.onClick.AddListener(BackToMainMenuButtonClicked);
    }

    private void OnDestroy()
    {
        backToMainMenuButton.onClick.RemoveListener(BackToMainMenuButtonClicked);
    }

    private void BackToMainMenuButtonClicked()
    {
        SceneLoader.LoadScene(SceneNames.StartMenuScene);
    }
}