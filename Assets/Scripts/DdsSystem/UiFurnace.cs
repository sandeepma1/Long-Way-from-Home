using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFurnace : DragDropBase
{
    [SerializeField] private Item itemToAdd;

    private void Start()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].OnSlotDrop += OnSlotDrop;
            uiSlots[i].id = i;
            uiSlots[i].name = "UiSlot" + i.ToString();
        }
    }
}
