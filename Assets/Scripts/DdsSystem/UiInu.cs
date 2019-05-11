using UnityEngine;

public class UiInu : DragDropBase
{
    [SerializeField] private int slotCount = 5;
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddSlotItemReturnRemaining(iteoToAdd);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            print(CheckIfIteoAvailable(iteoToAdd));
        }
    }
}