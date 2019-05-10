using UnityEngine;

public class UiFus : DragDropBase
{
    [SerializeField] private int slotCount = 20;
    [SerializeField] private int maxStack = 12;

    private void Start()
    {
        CreateUiSlots(slotCount, transform);
        CreateUiSlotItem(maxStack);
        CreateUiSlotItem(maxStack);
        CreateUiSlotItem(maxStack);
        CreateUiSlotItem(maxStack);
        CreateUiSlotItem(maxStack);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CreateUiSlotItem(maxStack);
        }
    }
}