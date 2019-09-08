using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bronz.Ui
{
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
            UiCommonPopupMenu.Instance.InitYesNoDialog("Are you sure you want to go to main menu?",
                OnYesButtonClicked, OnNoButtonClicked, "Yes", "No");
        }

        private void OnYesButtonClicked()
        {
            SceneLoader.LoadScene(SceneNames.StartMenuScene);
        }

        private void OnNoButtonClicked()
        {
            //
        }
    }
}