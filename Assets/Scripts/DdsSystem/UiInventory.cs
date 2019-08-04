using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bronz.Ui
{
    public class UiInventory : DragDropBase
    {
        public static Action<Item, bool> AddItemToInventory;
        public static Action<Item, bool> RemoveItemFromInventory;
        public static Action OnUiInventoryUpdated;
        [SerializeField] private GameObject sidePanel;
        [SerializeField] private Button deleteItemButton;
        [SerializeField] private int slotCount = 5;
        [SerializeField] private Item itemToAdd;
        [SerializeField] private Transform parentPanel;
        private RectTransform parentPanelRectTransform;
        private RectTransform thisRectTransform;
        private const float moveDuration = 0.1f;

        protected override void Start()
        {
            thisRectTransform = GetComponent<RectTransform>();
            UiAllMenusCanvas.OnMoveInventoryPanel += OnMoveInventoryPanelToAnotherPanel;
            base.Start();
            InitOtherOptions();
            AddItemToInventory += OnAddItemToInventory;
            RemoveItemFromInventory += OnRemoveItemFromInventory;

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
            UiAllMenusCanvas.OnMoveInventoryPanel -= OnMoveInventoryPanelToAnotherPanel;
            AddItemToInventory -= OnAddItemToInventory;
            RemoveItemFromInventory -= OnRemoveItemFromInventory;
            deleteItemButton.onClick.RemoveListener(OnDeleteItemButtonClick);
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(2))
            {
                Item item = new Item(UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(1, 5));
                OnAddItemToInventory(item, true);
            }
        }

        #region UiInventory Buttons

        private void InitOtherOptions()
        {
            deleteItemButton.onClick.AddListener(OnDeleteItemButtonClick);
            //all other options int goes here like split, delete, etc
            sidePanel.SetActive(false);
        }

        private void OnDeleteItemButtonClick()
        {
            DeleteSlectedUiSlotItem();
            OnUiInventoryUpdated?.Invoke();
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
    }
}