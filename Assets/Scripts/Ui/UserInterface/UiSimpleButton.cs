using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bronz.Ui
{
    public class UiSimpleButton : MonoBehaviour
    {
        private TextMeshProUGUI buttonText;
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void ChangeButtonText(string textToChange)
        {
            buttonText.text = textToChange;
        }

        public void RemoveListner()
        {
            button.onClick.RemoveAllListeners();
        }

        public void AddListener(UnityAction unityAction)
        {
            button.onClick.AddListener(unityAction);
        }
    }
}