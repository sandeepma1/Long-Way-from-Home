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
        private RectTransform thisRectTransform;
        private const float moveDuration = 0.1f;

        protected override void Start()
        {
            thisRectTransform = GetComponent<RectTransform>();
            UiAllMenusCanvas.OnMoveInventoryPanel += OnMoveInventoryPanelToAnotherPanel;
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
            UiAllMenusCanvas.OnMoveInventoryPanel -= OnMoveInventoryPanelToAnotherPanel;
            AddItemToInventory -= OnAddItemToInventory;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(2))
            {
                Item item = new Item(UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(1, 5));
                AddSlotItemReturnRemaining(item);
            }
        }

        private void OnMoveInventoryPanelToAnotherPanel(RectTransform parent)
        {
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

        public void OnAddItemToInventory(Item itemToAdd)
        {
            AddSlotItemReturnRemaining(itemToAdd);
            GEM.PrintDebug("item added to inventory " + itemToAdd.id.Value);
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