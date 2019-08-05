using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSlot : MonoBehaviour, IDropHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action<UiSlot, UiSlotItem> OnUiSlotClicked;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        UiSlotItem uiSlotItem = GetSlotItem();
        if (uiSlotItem != null)
        {
            UiToolTipCanvas.OnShowToolTip(true, RectTransformUtility.WorldToScreenPoint(null, transform.position), uiSlotItem.ItemId.Value);
        }
        OnUiSlotClicked?.Invoke(this, GetSlotItem());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UiToolTipCanvas.OnShowToolTip(false, Vector2.zero, -1);
    }

    public void SetSpriteToNormal()
    {
        slotImage.sprite = AtlasBank.GetUiSpritesByName("UiSlotBackground");
    }

    public void SetSpriteToSelected()
    {
        slotImage.sprite = AtlasBank.GetUiSpritesByName("UiSlotSelector");
    }

    public UiSlotItem GetSlotItem()
    {
        if (transform.childCount == 0)
        {
            return null;
        }
        return transform.GetChild(0).GetComponent<UiSlotItem>();
    }
}