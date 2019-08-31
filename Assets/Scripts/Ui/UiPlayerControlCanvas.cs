using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bronz.Ui
{
    public class UiPlayerControlCanvas : MonoBehaviour
    {
        public static Action OnInventoryButtonClicked;
        public static Action OnActionButtonClicked;
        public static Action OnActionButtonDown;
        public static Action OnActionButtonUp;
        public static Action OnMoreButtonClicked;
        public static Action<string> OnActionButtonTextChange;
        public static Action<bool> OnToggleControlsView;
        [SerializeField] private GameObject panelParent;
        [SerializeField] private Button actionButton;
        [SerializeField] private TextMeshProUGUI actionButtonText;
        [SerializeField] private Button moreButton;
        [SerializeField] private Button inventoryButton;
        private EventTrigger trigger;
        private EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        private EventTrigger.Entry pointerUp = new EventTrigger.Entry();

        private void Start()
        {
            OnActionButtonTextChange += OnActionButtonTextChangeEventHandler;
            OnToggleControlsView += TogglePlayerControls;
            trigger = actionButton.GetComponent<EventTrigger>();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((e) => OnActionButtonDown?.Invoke());
            trigger.triggers.Add(pointerDown);

            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((e) => OnActionButtonUp?.Invoke());
            trigger.triggers.Add(pointerUp);

            actionButton.onClick.AddListener(() => OnActionButtonClicked?.Invoke());
            moreButton.onClick.AddListener(() => OnMoreButtonClicked?.Invoke());
            inventoryButton.onClick.AddListener(() => OnInventoryButtonClicked?.Invoke());
        }

        private void OnDestroy()
        {
            OnActionButtonTextChange -= OnActionButtonTextChangeEventHandler;
            OnToggleControlsView -= TogglePlayerControls;
            pointerDown.callback.RemoveListener((e) => OnActionButtonDown?.Invoke());
            pointerUp.callback.RemoveListener((e) => OnActionButtonUp?.Invoke());
            actionButton.onClick.RemoveListener(() => OnActionButtonClicked?.Invoke());
            moreButton.onClick.RemoveListener(() => OnMoreButtonClicked?.Invoke());
            inventoryButton.onClick.RemoveListener(() => OnInventoryButtonClicked?.Invoke());
        }

        public void TogglePlayerControls(bool flag)
        {
            panelParent.SetActive(flag);
        }

        private void OnActionButtonTextChangeEventHandler(string text)
        {
            actionButtonText.text = text;
            //if (text == "")
            //{
            //    actionButton.interactable = false;
            //}
            //else
            //{
            //    actionButton.interactable = true;
            //}
        }
    }
}