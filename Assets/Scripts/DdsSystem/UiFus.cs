using UnityEngine;

public class UiFus : DragDropBase
{
    [SerializeField] private int slotCount = 20;
    [SerializeField] private Item iteoToAdd;

    private void Start()
    {
        CreateUiSlots(slotCount, transform);
        AddSlotItemReturnRemaining(CreateRanIteo());
        AddSlotItemReturnRemaining(CreateRanIteo());
        AddSlotItemReturnRemaining(CreateRanIteo());
        AddSlotItemReturnRemaining(CreateRanIteo());
        AddSlotItemReturnRemaining(CreateRanIteo());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddSlotItemReturnRemaining(iteoToAdd);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveSlotItemReturnNeeded(iteoToAdd);
        }
    }
}