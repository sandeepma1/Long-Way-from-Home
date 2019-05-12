using UnityEngine;

public class UiChest : DragDropBase
{
    [SerializeField] private int slotCount = 20;
    [SerializeField] private Item itemToAdd;  //debug

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

    //debug
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