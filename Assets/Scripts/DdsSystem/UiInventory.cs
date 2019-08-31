using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bronz.Ui
{
    public class UiInventory : DragDropBase
    {
        public static Action<UiSlotItem> OnInventoryItemClicked;
        public static Action<Item, bool> AddItemToInventory;
        public static Action<Item, bool> RemoveItemFromInventory;
        public static Action OnUiInventoryUpdated;
        public static Action OnUseActionClicked;
        [SerializeField] private GameObject sidePanel;
        [SerializeField] private Button useItemButton;
        [SerializeField] private TextMeshProUGUI useButtonText;
        [SerializeField] private Button splitItemButton;
        [SerializeField] private Button sortItemButton;
        [SerializeField] private Button deleteItemButton;
        [SerializeField] private int slotCount = 5;
        [SerializeField] private Item itemToAdd;
        [SerializeField] private Transform parentPanel;
        private RectTransform parentPanelRectTransform;
        private RectTransform thisRectTransform;
        private const float moveDuration = 0.1f;

        protected override void Start()
        {
            PlayerMovement.OnPlayerDataLoaded += SetUiSlotIdOnLoad;
            thisRectTransform = GetComponent<RectTransform>();
            UiAllMenusCanvas.OnMoveInventoryPanel += OnMoveInventoryPanelToAnotherPanel;
            base.Start();
            InitOtherOptions();
            AddItemToInventory += OnAddItemToInventory;
            RemoveItemFromInventory += OnRemoveItemFromInventory;
            OnUseActionClicked += OnUseItemButtonClick;
            if (!areUiSlotsCreated)
            {
                GEM.PrintDebug("CreateUiSlots UiInventory");
                CreateUiSlots(slotCount, parentPanel);
            }
            PlayerSaveFurniture playerSaveFurniture = MasterSave.LoadInventory();
            if (playerSaveFurniture != null)
            {
                LoadFurnitureData(playerSaveFurniture.slotItems);
            }
        }

        private void OnDestroy()
        {
            PlayerMovement.OnPlayerDataLoaded -= SetUiSlotIdOnLoad;
            UiAllMenusCanvas.OnMoveInventoryPanel -= OnMoveInventoryPanelToAnotherPanel;
            AddItemToInventory -= OnAddItemToInventory;
            RemoveItemFromInventory -= OnRemoveItemFromInventory;
            OnUseActionClicked -= OnUseItemButtonClick;
            deleteItemButton.onClick.RemoveListener(OnDeleteItemButtonClick);
            splitItemButton.onClick.RemoveListener(OnSplitItemButtonClick);
            sortItemButton.onClick.RemoveListener(OnSortItemButtonClick);
            useItemButton.onClick.RemoveListener(OnUseItemButtonClick);
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(2))
            {
                Item item = new Item(UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(1, 5));
                OnAddItemToInventory(item, true);
            }
        }

        #region UiInventory Buttons Side options

        private void InitOtherOptions()
        {
            useItemButton.onClick.AddListener(OnUseItemButtonClick);
            splitItemButton.onClick.AddListener(OnSplitItemButtonClick);
            sortItemButton.onClick.AddListener(OnSortItemButtonClick);
            deleteItemButton.onClick.AddListener(OnDeleteItemButtonClick);
            sidePanel.SetActive(false);
        }

        private void OnUseItemButtonClick()
        {
            int itemId = lastClickedUiSlotItem.ItemId.Value;
            if (PlayerStats.PlayerHunger < GEM.maxPlayerHunger)
            {
                PlayerStats.OnIncrementPlayerHunger(InventoryItemsDatabase.GetFoodPointsById(itemId));
            }
            if (PlayerStats.PlayerThirst < GEM.maxPlayerThirst)
            {
                PlayerStats.OnIncrementPlayerThirst(InventoryItemsDatabase.GetThirstPointsById(itemId));
            }
            OnDeleteItemButtonClick();
        }

        private void OnDeleteItemButtonClick()
        {
            //TODO: add common ui window to delete all or one
            DecrementSlectedUiSlotItem(1);
            OnUiInventoryUpdated?.Invoke();
            CheckIfUiSlotItemIsUseable();
            CheckIfUiSlotItemIsDeleteable();
            CheckIfUiSlotItemIsSplitable();
        }

        private void OnSplitItemButtonClick()
        {
            SplitUiSlotItems();
            CheckIfUiSlotItemIsSplitable();
        }

        private void OnSortItemButtonClick()
        {
            OnUiInventoryUpdated?.Invoke();
        }

        protected override void ToggleSplitButton(bool flag)
        {
            base.ToggleSplitButton(flag);
            splitItemButton.interactable = flag;
        }

        protected override void ToggleUseButton(bool flag)
        {
            base.ToggleUseButton(flag);
            useButtonText.text = "Eat";
            useItemButton.interactable = flag;
        }

        protected override void ToggleDeleteButton(bool flag)
        {
            base.ToggleDeleteButton(flag);
            deleteItemButton.interactable = flag;
        }

        #endregion

        private void OnMoveInventoryPanelToAnotherPanel(RectTransform parent, bool showOtherOptionsPanel = false)
        {
            sidePanel.SetActive(showOtherOptionsPanel);
            thisRectTransform.SetAndStretchToParentSize(parent);
        }

        protected override void CreateUiSlots()
        {
            base.CreateUiSlots();
            GEM.PrintDebug("base class called UiInventory");
            CreateUiSlots(slotCount, parentPanel);
        }

        protected override void RequestSaveData()
        {
            base.RequestSaveData();
            List<SlotItems> slotItems = new List<SlotItems>();
            for (int i = 0; i < uiSlotItems.Count; i++)
            {
                slotItems.Add(uiSlotItems[i].slotItem);
            }
            furniture = new PlayerSaveFurniture(slotItemsType, 0, 0, slotItems);
            furniture.slotItems = slotItems;
            MasterSave.SaveInventory(furniture);
            GEM.PrintDebug("called derived");
        }

        protected override void OnUiSlotClicked(UiSlot uiSlot, UiSlotItem uiSlotItem)
        {
            base.OnUiSlotClicked(uiSlot, uiSlotItem);
            OnInventoryItemClicked?.Invoke(uiSlotItem);
        }

        private void OnAddItemToInventory(Item itemToAdd, bool updateInventory = true)
        {
            AddSlotItemReturnRemaining(itemToAdd);
            GEM.PrintDebug("item added to inventory " + itemToAdd.id.Value);
            if (updateInventory)
            {
                OnUiInventoryUpdated?.Invoke();
            }
        }

        private void OnRemoveItemFromInventory(Item itemToRemove, bool updateInventory = true)
        {
            Item itemsNeeded = RemoveSlotItemReturnNeeded(itemToRemove);
            if (itemsNeeded != null)
            {
                GEM.PrintDebug("All items removed nothing needed");
            }
            if (updateInventory)
            {
                OnUiInventoryUpdated?.Invoke();
            }
        }

        public static List<Item> CheckIfItemsAvailable(CraftableItem craftableItem)
        {
            List<Item> requiresItems = new List<Item>();
            for (int i = 0; i < craftableItem.requires.Count; i++)
            {
                if (CheckIfItemAvailable(craftableItem.requires[i]))
                {
                    requiresItems.Add(craftableItem.requires[i]);
                }
            }
            return requiresItems;
        }

        public static int GetLastClikcedSlotId()
        {
            return lastClickedUiSlotItem.ItemSlotId;
        }

        public void SetUiSlotIdOnLoad(int id)
        {
            StartCoroutine(SetUiSlotIdOnLoadOnDelay(id));
        }

        private IEnumerator SetUiSlotIdOnLoadOnDelay(int id)
        {
            yield return new WaitForEndOfFrame();
            OnUiSlotClicked(uiSlots[id], uiSlots[id].GetSlotItem());
        }
    }
}