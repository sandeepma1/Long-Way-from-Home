using System;
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
    [SerializeField] private InventoryIteo inuIteo; //make private later

    public int IteoId
    {
        get { return inuIteo.item.id; }
        set { inuIteo.item.id = value; }
    }
    public int IteoDuraCount
    {
        get { return inuIteo.item.duraCount; }
        set { inuIteo.item.duraCount = value; }
    }
    public int IteoInuSlotId
    {
        get { return inuIteo.invSlotId; }
        set { inuIteo.invSlotId = value; }
    }

    public void Init(int maxStack, InventoryIteo inuIteo)
    {
        this.maxStack = maxStack;
        this.inuIteo = inuIteo;
        lastDragDropBase = transform.parent.parent.GetComponent<DragDropBase>();
        gameObject.name = IteoId + "-" + IteoDuraCount;
    }

    private void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void SetParent(Transform parent, int slotId)
    {
        SetParent(parent);
        IteoInuSlotId = slotId;
    }

    public bool IsStackMax()
    {
        return IteoDuraCount >= maxStack;
    }

    public int IncrementDuraCount(int duraCount)
    {
        if (IsStackMax())
        {
            return -1;
        }
        int add = IteoDuraCount + duraCount;
        if (add <= maxStack)
        {
            IteoDuraCount = add;
            UpdateText();
            return 0;
        }
        else
        {
            int canAdd = Mathf.Abs(maxStack - IteoDuraCount);
            IteoDuraCount += canAdd;
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
            IteoDuraCount = duracount;
            UpdateText();
        }
    }

    public int DecrementDuraCount(int duracount)
    {
        if (duracount <= 0)
        {
            return 0;
        }

        if (IteoDuraCount > duracount)
        {
            IteoDuraCount -= duracount;
            UpdateText();
            return 0;
        }
        else if (IteoDuraCount < duracount)
        {
            int remaining = duracount - IteoDuraCount;
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
        idText.text = IteoId.ToString();
        countText.text = IteoDuraCount.ToString();
        gameObject.name = IteoId + "-" + IteoDuraCount;
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