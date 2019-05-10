using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSlotItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public Action<UiSlotItem, UiSlotItem> OnSlotItemDrop;
    public Text idText;
    public Text countText;
    public Image image;
    public int maxStack;
    public DropType dropType;
    private Iteo iteo;
    public Iteo Iteo
    {
        get
        {
            return iteo;
        }
        set
        {
            iteo = value;
            UpdateText();
        }
    }
    private DragDropBase dragDropBase;
    public bool isSwaping = false;

    public int IncrementDuraCount(int duraCount)
    {
        int add = iteo.duraCount + duraCount;
        if (add <= maxStack)
        {
            iteo.duraCount = add;
            UpdateText();
            return 0;
        }
        else
        {
            int canAdd = Mathf.Abs(maxStack - iteo.duraCount);
            iteo.duraCount += canAdd;
            int cannotAdd = Mathf.Abs(canAdd - duraCount);
            UpdateText();
            return cannotAdd;
        }
    }

    public void DecrementDuraCount(int duracount)
    {
        iteo.duraCount = duracount;
        if (iteo.duraCount <= 0)
        {
            dragDropBase.RemoveSlotItem(this);
            Destroy(this.gameObject);
        }
        else
        {
            UpdateText();
            isSwaping = true;
        }
    }

    public void UpdateText()
    {
        idText.text = iteo.id.ToString();
        countText.text = iteo.duraCount.ToString();
        gameObject.name = iteo.id + "-" + iteo.duraCount;
    }

    public void Init()
    {
        dragDropBase = transform.parent.transform.parent.GetComponent<DragDropBase>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragDropBase = transform.parent.transform.parent.GetComponent<DragDropBase>();
        dragDropBase.RemoveSlotItem(this);
        SetParentUiSlotOccupied(false);
        image.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;
        if (isSwaping)
        {
            isSwaping = false;
            return;
        }
        if (!IfEndDragOnUiSlot(eventData.hovered))//not on slot
        {
            dragDropBase.AddSlotItem(this);
            SetParentUiSlotOccupied(true);
        }
        if (IfEndDragOnUiSlotItem(eventData.hovered))//on slotItem
        {
            dragDropBase.AddIfNotContains(this);
            SetParentUiSlotOccupied(true);
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        print("drop");

        UiSlotItem uiSlotItem = eventData.pointerDrag.GetComponent<UiSlotItem>();
        if (uiSlotItem != null)
        {
            OnSlotItemDrop?.Invoke(this, uiSlotItem);
        }
    }

    #region Helper Functions
    private bool IfEndDragOnUiSlot(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].GetComponent<UiSlot>())
            {
                return true;
                UiSlot uiSlot = gameObjects[i].GetComponent<UiSlot>();
                if (uiSlot.isDropable && !uiSlot.isOccupied)
                {

                }
            }
        }
        return false;
    }

    private bool IfEndDragOnUiSlotItem(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].GetComponent<UiSlotItem>())
            {
                return true;
            }
        }
        return false;
    }

    public void SetParentUiSlotOccupied(bool flag)
    {
        transform.parent.GetComponent<UiSlot>().isOccupied = flag;
    }
    #endregion
}