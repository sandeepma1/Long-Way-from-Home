using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bronz.Ui
{
    public class UiInventory : DragDropBase
    {
        public static Action<Item> AddItemToInventory;
        [SerializeField] private int slotCount = 5;
        [SerializeField] private Item itemToAdd;
        [SerializeField] private Transform parentPanel;
        private RectTransform parentPanelRectTransform;
        private bool isInventoryUp = false;
        private const float moveDuration = 0.1f;

        protected override void Start()
        {
            parentPanelRectTransform = parentPanel.GetComponent<RectTransform>();
            UiPlayerControlCanvas.OnInventoryButtonClicked += OnInventoryButtonClicked;
            base.Start();
            AddItemToInventory += OnAddItemToInventory;
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
            UiPlayerControlCanvas.OnInventoryButtonClicked -= OnInventoryButtonClicked;
            AddItemToInventory -= OnAddItemToInventory;
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

        public void OnAddItemToInventory(Item itemToAdd)
        {
            AddSlotItemReturnRemaining(itemToAdd);
            GEM.PrintDebug("item added to inventory " + itemToAdd.id);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                AddSlotItemReturnRemaining(itemToAdd);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                print(CheckIfItemAvailable(itemToAdd));
            }

            if (Input.GetMouseButtonUp(1))
            {
                print(InventoryItemsDatabase.InventoryItems.InventoryItems.Length);
                Item item = new Item(UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(1, 5));
                AddSlotItemReturnRemaining(item);
            }
        }

        private void OnInventoryButtonClicked()
        {
            isInventoryUp = !isInventoryUp;
            ToggleInventoryView(isInventoryUp);
        }

        private void ToggleInventoryView(bool flag)
        {
            if (flag) // hide panel
            {
                parentPanelRectTransform.DOAnchorPos(new Vector2(0, 0), moveDuration);
                UiPlayerControlCanvas.OnToggleControlsView(false);
            }
            else  // show panel
            {
                UiPlayerControlCanvas.OnToggleControlsView(true);
                parentPanelRectTransform.DOAnchorPos(new Vector2(0, -270), moveDuration);
            }
        }
    }
}