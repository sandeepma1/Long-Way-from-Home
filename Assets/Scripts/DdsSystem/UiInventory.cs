using System;
using System.Collections.Generic;
using UnityEngine;

public class UiInventory : DragDropBase
{
    public static Action<Item> AddItemToInventory;
    [SerializeField] private int slotCount = 5;
    [SerializeField] private Item itemToAdd;

    private void Start()
    {
        AddItemToInventory += OnAddItemToInventory;
        if (!areUiSlotsCreated)
        {
            CreateUiSlots(slotCount, transform);
        }
    }

    protected override void CreateUiSlots()
    {
        base.CreateUiSlots();
        print("base class called UiInventory");
        CreateUiSlots(slotCount, transform);
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