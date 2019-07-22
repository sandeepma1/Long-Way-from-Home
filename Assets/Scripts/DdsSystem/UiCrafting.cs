using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bronz.Ui
{
    public class UiCrafting : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI craftableItemNameText;
        [SerializeField] private TextMeshProUGUI craftableItemDescriptionText;
        [SerializeField] private Transform craftingRequiredParent;
        [SerializeField] private Transform craftingItemsParent;
        [SerializeField] private Button craftButton;
        [SerializeField] private RectTransform selectionBox;
        private UiCraftingItem uiCraftingItemPrefab;
        private UiCraftingItem[] uiCraftingItems;
        private UiCraftingRequiredItem uiCraftingRequiredItemPrefab;
        private UiCraftingRequiredItem[] uiCraftingRequiredItems = new UiCraftingRequiredItem[4];
        private CraftableItem craftableCraftableItem = null;

        private void Start()
        {
            InitCraftingMenu();
        }

        private void OnDestroy()
        {
            craftButton.onClick.RemoveListener(OnCraftButtonClicked);
        }

        private void InitCraftingMenu()
        {
            craftButton.onClick.AddListener(OnCraftButtonClicked);
            craftButton.interactable = false;
            uiCraftingRequiredItemPrefab = PrefabBank.uiCraftingRequiredItem;
            uiCraftingItemPrefab = PrefabBank.uiCraftingItem;
            for (int i = 0; i < 4; i++)
            {
                uiCraftingRequiredItems[i] = Instantiate(uiCraftingRequiredItemPrefab, craftingRequiredParent);
                uiCraftingRequiredItems[i].gameObject.SetActive(false);
            }

            int craftablesCount = CraftableItemsDatabase.CraftableItemsDb.CraftableItems.Length;
            uiCraftingItems = new UiCraftingItem[craftablesCount];
            for (int i = 0; i < craftablesCount; i++)
            {
                UiCraftingItem uiCraftingItem = Instantiate(uiCraftingItemPrefab, craftingItemsParent);
                uiCraftingItem.Init(CraftableItemsDatabase.CraftableItemsDb.CraftableItems[i]);
                uiCraftingItem.OnCraftableItemClicked += OnCraftableItemClicked;
                uiCraftingItems[i] = uiCraftingItem;
            }
        }

        private void OnCraftButtonClicked()
        {
            for (int i = 0; i < craftableCraftableItem.requires.Count; i++)
            {
                UiInventory.RemoveItemFromInventory?.Invoke(craftableCraftableItem.requires[i]);
            }
            UiInventory.AddItemToInventory?.Invoke(craftableCraftableItem.creates);
        }

        private void OnCraftableItemClicked(CraftableItem craftableItem, Transform itemTransform)
        {
            selectionBox.SetParent(itemTransform);
            selectionBox.anchoredPosition = Vector3.zero;
            craftableItemNameText.text = craftableItem.name;
            craftableItemDescriptionText.text = "Description: (implement here) " + craftableItem.name;
            for (int i = 0; i < craftableItem.requires.Count; i++)
            {
                if (craftableItem.requires[i].id.Value < 0)
                {
                    uiCraftingRequiredItems[i].gameObject.SetActive(false);
                    continue;
                }
                uiCraftingRequiredItems[i].gameObject.SetActive(true);
                uiCraftingRequiredItems[i].Init(craftableItem.requires[i]);
            }
            CheckIfCraftable(craftableItem);
        }

        private void CheckIfCraftable(CraftableItem craftableItem)
        {
            bool hasAllItems = true;
            List<Item> gotItems = UiInventory.CheckIfItemsAvailable(craftableItem);
            if (gotItems.Count != craftableItem.requires.Count)
            {
                hasAllItems = false;
            }
            else
            {
                for (int i = 0; i < gotItems.Count; i++)
                {
                    if (gotItems[i] != null)
                    {
                        print("want id " + craftableItem.requires[i].id + " with count " + craftableItem.requires[i].duraCount + " X got id " + gotItems[i].id + " with count " + gotItems[i].duraCount);
                        if (gotItems[i].duraCount < craftableItem.requires[i].duraCount)
                        {
                            hasAllItems = false;
                        }
                    }
                }
            }

            craftButton.interactable = hasAllItems;
            if (hasAllItems)
            {
                craftableCraftableItem = craftableItem;
            }
        }
    }
}