using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPlayerSlots : DragDropBase
{
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].OnSlotDrop += OnSlotDrop;
            uiSlots[i].id = i;
            uiSlots[i].name = "UiSlot" + i.ToString();
            uiSlots[i].OnUiSlotClicked += OnUiSlotClicked;
        }
    }
}