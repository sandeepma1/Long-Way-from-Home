﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSlotItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public Action<UiSlotItem, UiSlotItem> OnSlotItemDrop;
    public Action<PointerEventData> OnSlotEndDrag;
    public int maxStack;
    public DropType dropType;
    public DragDropBase lastDragDropBase;
    public Transform lastParent;
    [SerializeField] private Text idText;
    [SerializeField] private Text countText;
    [SerializeField] private Image image;
    public SlotItems slotItem; //make private later

    public int ItemId
    {
        get { return slotItem.item.id; }
        set { slotItem.item.id = value; }
    }
    public int ItemDuraCount
    {
        get { return slotItem.item.duraCount; }
        set { slotItem.item.duraCount = value; }
    }
    public int ItemSlotId
    {
        get { return slotItem.invSlotId; }
        set { slotItem.invSlotId = value; }
    }

    public void Init(int maxStack, SlotItems inuItem)
    {
        this.maxStack = maxStack;
        this.slotItem = inuItem;
        lastDragDropBase = transform.parent.parent.GetComponent<DragDropBase>();
        gameObject.name = ItemId + "-" + ItemDuraCount;
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
        return ItemDuraCount >= maxStack;
    }

    public int IncrementDuraCount(int duraCount)
    {
        if (IsStackMax())
        {
            return -1;
        }
        int add = ItemDuraCount + duraCount;
        if (add <= maxStack)
        {
            ItemDuraCount = add;
            UpdateText();
            return 0;
        }
        else
        {
            int canAdd = Mathf.Abs(maxStack - ItemDuraCount);
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
            int remaining = duracount - ItemDuraCount;
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
        idText.text = ItemId.ToString();
        countText.text = ItemDuraCount.ToString();
        gameObject.name = ItemId + "-" + ItemDuraCount;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RemovedFromThis();
    }

    private void DestroyAndRemoveFromList()
    {
        lastDragDropBase.RemoveSlotItem(this);
        Destroy(this.gameObject);
    }

    public void RemovedFromThis()
    {
        lastDragDropBase = transform.parent.parent.GetComponent<DragDropBase>();
        lastParent = transform.parent;
        lastDragDropBase.RemoveSlotItem(this);
        SetParent(transform.root);
        image.raycastTarget = false;
    }

    public void AddInThis(DragDropBase tempDragDropBase, Transform tempTransform, int slotId)
    {
        lastDragDropBase = tempDragDropBase;
        lastDragDropBase.AddSlotItem(this);
        SetParent(tempTransform, slotId);
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;
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
}

//BeginDrag
//Drag
//Drop
//EndDrag