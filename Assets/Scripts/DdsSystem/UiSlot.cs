using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiSlot : MonoBehaviour, IDropHandler
{
    public Action<UiSlot, UiSlotItem> OnSlotDrop;
    public int id;
    public bool isDropable = true;
    public DropType dropType = DropType.Any;

    public void OnDrop(PointerEventData eventData)
    {
        if (isDropable)
        {
            OnSlotDrop?.Invoke(this, eventData.pointerDrag.GetComponent<UiSlotItem>());
        }
    }
}