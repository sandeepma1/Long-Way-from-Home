using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bronz.Ui
{
    public class UiCrafting : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI craftableItemNameText;
        [SerializeField] private Transform craftingRequiredParent;
        [SerializeField] private Transform craftingItemsParent;
        [SerializeField] private Button craftButton;
        [SerializeField] private RectTransform selectionBox;
        private UiCraftingItem uiCraftingItemPrefab;
        private UiCraftingItem[] uiCraftingItems;
        private UiCraftingRequiredItem uiCraftingRequiredItemPrefab;
        private UiCraftingRequiredItem[] uiCraftingRequiredItems = new UiCraftingRequiredItem[4];

        private void Start()
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

        private void OnDestroy()
        {
            craftButton.onClick.RemoveListener(OnCraftButtonClicked);
        }

        private void OnCraftButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void OnCraftableItemClicked(CraftableItem craftableItem, Transform itemTransform)
        {
            selectionBox.SetParent(itemTransform);
            selectionBox.anchoredPosition = Vector3.zero;
            craftableItemNameText.text = craftableItem.name;
            for (int i = 0; i < craftableItem.requires.Count; i++)
            {
                if (craftableItem.requires[i].itemId == null)
                {
                    uiCraftingRequiredItems[i].gameObject.SetActive(false);
                    continue;
                }
                uiCraftingRequiredItems[i].gameObject.SetActive(true);
                uiCraftingRequiredItems[i].Init(craftableItem.requires[i]);
            }
            // UiInventory.check
        }
    }
}