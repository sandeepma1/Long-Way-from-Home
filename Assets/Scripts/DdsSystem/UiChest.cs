using UnityEngine;

public class UiChest : DragDropBase
{
    [SerializeField] private int slotCount = 20;
    [SerializeField] private Item itemToAdd;  //debug

    protected override void CreateUiSlots()
    {
        base.CreateUiSlots();
        print("CreateUiSlots UiChest");
        CreateUiSlots(slotCount, transform);
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