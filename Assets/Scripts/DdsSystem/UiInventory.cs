﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class UiInventory : DragDropBase
{
    [SerializeField] private int slotCount = 5;
    [SerializeField] private Item itemToAdd;

    private void Start()
    {
        if (!areUiSlotsCreated)
        {
            CreateUiSlots(slotCount, transform);
        }
    }

    protected override void CreateUiSlotsIfNotCreated()
    {
        Start();
        base.CreateUiSlotsIfNotCreated();
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
    }
}