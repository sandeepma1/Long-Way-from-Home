using UnityEngine;

public class UiFus : DragDropBase
{
    [SerializeField] private int slotCount = 20;
    [SerializeField] private Item itemToAdd;

    private void Start()
    {
        CreateUiSlots(slotCount, transform);
        AddSlotItemReturnRemaining(CreateRanItem());
        AddSlotItemReturnRemaining(CreateRanItem());
        AddSlotItemReturnRemaining(CreateRanItem());
        AddSlotItemReturnRemaining(CreateRanItem());
        AddSlotItemReturnRemaining(CreateRanItem());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddSlotItemReturnRemaining(itemToAdd);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveSlotItemReturnNeeded(itemToAdd);
        }
    }
}