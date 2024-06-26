﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSlotItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public Action<UiSlotItem, UiSlotItem> OnSlotItemDrop;
    public Action<PointerEventData> OnSlotEndDrag;
    public int maxStackable;
    public DropType dropType;
    public DragDropBase lastDragDropBase;
    public Transform lastParent;
    public ItemType itemType;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image image;
    public SlotItem slotItem; //make private later

    public int? ItemId
    {
        get { return slotItem.item.id; }
        set { slotItem.item.id = value; }
    }
    public int? ItemDuraCount
    {
        get { return slotItem.item.duraCount; }
        set { slotItem.item.duraCount = value; }
    }
    public int ItemSlotId
    {
        get { return slotItem.invSlotId; }
        set { slotItem.invSlotId = value; }
    }

    public void Init(SlotItem slotItem)
    {
        maxStackable = InventoryItemsDatabase.GetMaxStackableById(slotItem.item.id.Value);
        itemType = InventoryItemsDatabase.GetInventoryItemTypeById(slotItem.item.id.Value);
        this.slotItem = slotItem;
        lastDragDropBase = transform.parent.parent.GetComponent<DragDropBase>();
        gameObject.name = ItemId + "-" + ItemDuraCount;
        SetLastDDBaseParent();
    }

    private void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void SetParent(Transform parent, int slotId)
    {
        SetParent(parent);
        ItemSlotId = slotId;
    }

    public bool IsStackMax()
    {
        return ItemDuraCount >= maxStackable;
    }

    public int IncrementDuraCount(int duraCount)
    {
        if (IsStackMax())
        {
            return -1;
        }
        int add = ItemDuraCount.Value + duraCount;
        if (add <= maxStackable)
        {
            ItemDuraCount = add;
            UpdateText();
            return 0;
        }
        else
        {
            int canAdd = Mathf.Abs(maxStackable - ItemDuraCount.Value);
            ItemDuraCount += canAdd;
            int cannotAdd = Mathf.Abs(canAdd - duraCount);
            UpdateText();
            return cannotAdd;
        }
    }

    public void DecrementDuraCountDragDrop(int duracount)
    {
        if (duracount == -1)
        {
            return;
        }
        if (duracount == 0)
        {
            DestroyAndRemoveFromList();
        }
        else
        {
            ItemDuraCount = duracount;
            UpdateText();
        }
    }

    public int DecrementDuraCount(int duracount)
    {
        if (duracount <= 0)
        {
            return 0;
        }

        if (ItemDuraCount > duracount)
        {
            ItemDuraCount -= duracount;
            UpdateText();
            return 0;
        }
        else if (ItemDuraCount < duracount)
        {
            int remaining = duracount - ItemDuraCount.Value;
            DestroyAndRemoveFromList();
            return remaining;
        }
        else //==
        {
            DestroyAndRemoveFromList();
            return 0;
        }
    }

    public void UpdateText()
    {
        itemImage.sprite = AtlasBank.GetInventoryItemSpriteById(ItemId.Value);
        if (ItemDuraCount > 1)
        {
            countText.text = ItemDuraCount.ToString();
        }
        else
        {
            countText.text = "";
        }
        gameObject.name = ItemId + "-" + ItemDuraCount;
    }

    public void RemovedFromThis()
    {
        SetLastDDBaseParent();
        lastDragDropBase.RemoveSlotItem(this);
        SetParent(UiMainCanvas.mainCanvas);
        image.raycastTarget = false;
    }

    private void DestroyAndRemoveFromList()
    {
        //SetLastDDBaseParent();
        ItemDuraCount = 0;
        lastDragDropBase.RemoveSlotItem(this);
        Destroy(this.gameObject);
    }

    private void SetLastDDBaseParent()
    {
        //TODO: What the hell is this, refactor it
        lastDragDropBase = transform.parent.parent.parent.GetComponent<DragDropBase>();
        lastParent = transform.parent;
    }

    public void AddInThis(DragDropBase tempDragDropBase, Transform tempTransform, int slotId)
    {
        lastDragDropBase = tempDragDropBase;
        lastDragDropBase.AddSlotItem(this);
        SetParent(tempTransform, slotId);
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;
    }

    #region --Drag PointerEventData--
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        UiToolTipCanvas.OnShowToolTip(false, Vector2.zero, -1);
        RemovedFromThis();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnSlotEndDrag?.Invoke(eventData);
        if (!transform.parent.GetComponent<UiSlot>())
        {
            SetParent(lastParent);
            lastDragDropBase.AddSlotItem(this);
        }
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnSlotItemDrop?.Invoke(this, eventData.pointerDrag.GetComponent<UiSlotItem>());
    }

    #endregion
}

//BeginDrag
//Drag
//Drop
//EndDrag