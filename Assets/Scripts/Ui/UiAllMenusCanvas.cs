﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bronz.Ui
{
    public class UiAllMenusCanvas : MonoBehaviour
    {
        public static Action<RectTransform> OnMoveInventoryPanel;
        [SerializeField] private RectTransform inventoryPanel;
        [SerializeField] private RectTransform mainPanel;
        [SerializeField] private RectTransform inventoryPanelInCrafting;
        [SerializeField] private RectTransform inventoryPanelInInventory;
        [SerializeField] private Button inventoryTabButton;
        [SerializeField] private Button craftingTabButton;
        [SerializeField] private Button skillsTabButton;

        private bool isThisMenuVisible = false;

        private void Start()
        {
            UiPlayerControlCanvas.OnInventoryButtonClicked += OnInventoryButtonClicked;
            inventoryTabButton.onClick.AddListener(OnInventoryTabButtonClicked);
            craftingTabButton.onClick.AddListener(OnCraftingTabButtonClicked);
            skillsTabButton.onClick.AddListener(OnSkillsTabButtonClicked);
            mainPanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            UiPlayerControlCanvas.OnInventoryButtonClicked -= OnInventoryButtonClicked;
            inventoryTabButton.onClick.RemoveListener(OnInventoryTabButtonClicked);
            craftingTabButton.onClick.RemoveListener(OnCraftingTabButtonClicked);
            skillsTabButton.onClick.RemoveListener(OnSkillsTabButtonClicked);
        }

        private void OnInventoryButtonClicked()
        {
            isThisMenuVisible = !isThisMenuVisible;
            mainPanel.gameObject.SetActive(isThisMenuVisible);
            UiPlayerControlCanvas.OnToggleControlsView(!isThisMenuVisible);
            if (isThisMenuVisible)
            {
                inventoryPanel.SetAsLastSibling();
                OnMoveInventoryPanel?.Invoke(inventoryPanelInInventory);
            }
            else
            {
                OnMoveInventoryPanel?.Invoke(UiHotBarCanvas.hotBarPanelHolder);
            }
        }

        private void OnInventoryTabButtonClicked()
        {
            OnMoveInventoryPanel?.Invoke(inventoryPanelInInventory);
        }

        private void OnCraftingTabButtonClicked()
        {
            OnMoveInventoryPanel?.Invoke(inventoryPanelInCrafting);
        }

        private void OnSkillsTabButtonClicked()
        {

        }
    }
}