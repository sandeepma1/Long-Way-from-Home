using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSlot : MonoBehaviour, IDropHandler
{
    public Action<UiSlot, UiSlotItem> OnSlotDrop;
    public int id;
    public bool isDropable = true;
    public DropType dropType = DropType.Any;

    private void Start()
    {
        if (!isDropable)
        {
            GetComponent<Image>().color = Color.gray;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isDropable)
        {
            OnSlotDrop?.Invoke(this, eventData.pointerDrag.GetComponent<UiSlotItem>());
        }
    }
}