using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bronz.Ui
{
    public class UiPlayerControlCanvas : MonoBehaviour
    {
        public static Action OnActionButtonClicked;
        public static Action OnActionButtonDown;
        public static Action OnActionButtonUp;
        public static Action OnMoreButtonClicked;

        [SerializeField] private Button actionButton;
        [SerializeField] private Button moreButton;
        private EventTrigger trigger;

        private void Start()
        {
            trigger = actionButton.GetComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((e) => OnActionButtonDown?.Invoke());
            trigger.triggers.Add(pointerDown);

            var pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((e) => OnActionButtonUp?.Invoke());
            trigger.triggers.Add(pointerUp);

            actionButton.onClick.AddListener(() => OnActionButtonClicked?.Invoke());
            moreButton.onClick.AddListener(() => OnMoreButtonClicked?.Invoke());
        }
    }
}