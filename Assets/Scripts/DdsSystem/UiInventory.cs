using System;
using System.Collections.Generic;
using UnityEngine;

public class UiInventory : DragDropBase
{
    public static Action<Item> AddItemToInventory;
    [SerializeField] private int slotCount = 5;
    [SerializeField] private Item itemToAdd;

    protected override void Start()
    {
        base.Start();
        AddItemToInventory += OnAddItemToInventory;
        if (!areUiSlotsCreated)
        {
            GEM.PrintDebug("CreateUiSlots UiInventory");
            CreateUiSlots(slotCount, transform);
        }
        PlayerSaveFurniture playerSaveFurniture = MasterSave.LoadInventory();
        if (playerSaveFurniture != null)
        {
            LoadFurnitureData(playerSaveFurniture.slotItems);
        }
    }

    private void OnDestroy()
    {
        AddItemToInventory -= OnAddItemToInventory;
    }

    protected override void CreateUiSlots()
    {
        base.CreateUiSlots();
        GEM.PrintDebug("base class called UiInventory");
        CreateUiSlots(slotCount, transform);
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
            Item item = new Item(UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(1, 5));
            AddSlotItemReturnRemaining(item);
        }
    }
}