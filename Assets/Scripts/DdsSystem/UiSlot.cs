using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Action<UiSlot, Item> OnUiSlotClicked;
    public Action<UiSlot, UiSlotItem> OnSlotDrop;
    public int id;
    public bool isDropable = true;
    public DropType dropType = DropType.Any;
    [SerializeField] private Image slotImage;

    private void Start()
    {
        if (!isDropable)
        {
            slotImage.color = Color.gray;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isDropable)
        {
            OnSlotDrop?.Invoke(this, eventData.pointerDrag.GetComponent<UiSlotItem>());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //UiMainCanvas.OnSlotItemClicked?.Invoke(this);
        OnUiSlotClicked?.Invoke(this, GetSlotItem());
    }

    public void SetSpriteToNormal()
    {
        slotImage.sprite = AtlasBank.GetUiSpritesByName("UiSlotBackground");
    }

    public void SetSpriteToSelected()
    {
        slotImage.sprite = AtlasBank.GetUiSpritesByName("UiSlotSelector");
    }

    public Item GetSlotItem()
    {
        if (transform.childCount == 0)
        {
            return null;
        }
        return transform.GetChild(0).GetComponent<UiSlotItem>().slotItem.item;
    }
}