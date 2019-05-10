using UnityEngine;

public class UiInu : DragDropBase
{
    [SerializeField] private int slotCount = 5;
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            CreateUiSlotItem(maxStack);
        }
    }
}