using UnityEngine;

public class UiInu : DragDropBase
{
    [SerializeField] private int slotCount = 5;
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