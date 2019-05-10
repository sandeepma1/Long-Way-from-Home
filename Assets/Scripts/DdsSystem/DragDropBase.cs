using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropBase : MonoBehaviour
{
    [SerializeField] private UiSlot uiSlotPrefab;
    [SerializeField] private UiSlotItem uiSlotItemPrefab;
    public List<UiSlotItem> uiSlotItems = new List<UiSlotItem>(); //make it private
    protected UiSlot[] uiSlots;

    protected virtual void OnSlotDrop(UiSlot uiSlot, UiSlotItem uiSlotItem)
    {
        if (!uiSlot.isOccupied)
        {
            uiSlotItem.transform.SetParent(uiSlot.transform);
            uiSlot.isOccupied = true;
            AddSlotItem(uiSlotItem);
        }
    }

    protected virtual void OnSlotItemDrop(UiSlotItem dropedItem, UiSlotItem dragedItem)
    {
        dragedItem.isSwaping = true;
        if (dragedItem.Iteo.id == dropedItem.Iteo.id)
        {
            dragedItem.DecrementDuraCount(dropedItem.IncrementDuraCount(dragedItem.Iteo.duraCount));
        }
        else
        {
            DragDropBase dropBaseParent = dropedItem.transform.parent.transform.parent.GetComponent<DragDropBase>();
            DragDropBase dragBaseParent = dragedItem.transform.parent.transform.parent.GetComponent<DragDropBase>();
            Transform dropParent = dropedItem.transform.parent;
            Transform dragParent = dragedItem.transform.parent;
            dropedItem.transform.parent = null;
            dragedItem.transform.parent = null;
            dropBaseParent.RemoveSlotItem(dropedItem);
            dragBaseParent.RemoveSlotItem(dragedItem);

            dragBaseParent.AddIfNotContains(dropedItem);
            dropBaseParent.AddIfNotContains(dragedItem);
            dropedItem.transform.SetParent(dragParent);
            dragedItem.transform.SetParent(dropParent);
            dropedItem.transform.localPosition = Vector3.zero;
            dragedItem.transform.localPosition = Vector3.zero;
            dropedItem.SetParentUiSlotOccupied(true);
            dragedItem.SetParentUiSlotOccupied(true);
        }

    }

    protected virtual void CreateUiSlots(int slotCount, Transform parent)
    {
        uiSlots = new UiSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            uiSlots[i] = InstantiateUiSlot(i, parent);
        }
        //uiSlots[0].isDropable = false;
    }

    protected virtual UiSlot InstantiateUiSlot(int id, Transform parent)
    {
        UiSlot uiSlot = Instantiate(uiSlotPrefab, parent);
        uiSlot.OnSlotDrop += OnSlotDrop;
        uiSlot.id = id;
        uiSlot.name = "UiSlot" + id.ToString();
        return uiSlot;
    }

    protected void CreateUiSlotItem(int maxStack)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i].transform.childCount == 0)
            {
                InstantiateUiSlotItem(uiSlots[i], maxStack);
                return;
            }
        }
        print("No empty Slots");
    }

    private void InstantiateUiSlotItem(UiSlot uiSlot, int maxStack)
    {
        UiSlotItem uiSlotItem = Instantiate(uiSlotItemPrefab, uiSlot.transform);
        uiSlot.isOccupied = true;
        uiSlotItem.maxStack = maxStack;
        uiSlotItem.Iteo = CreateRanIteo();
        uiSlotItem.name = uiSlotItem.Iteo.id + "-" + uiSlotItem.Iteo.duraCount;
        uiSlotItem.OnSlotItemDrop += OnSlotItemDrop;
        uiSlotItem.Init();
        AddSlotItem(uiSlotItem);
    }

    #region HelperFunctions
    public void AddSlotItem(UiSlotItem uiSlotItem)
    {
        print("added " + uiSlotItem.name);
        uiSlotItems.Add(uiSlotItem);
    }

    public void AddIfNotContains(UiSlotItem uiSlotItem)
    {
        if (!uiSlotItems.Contains(uiSlotItem))
        {
            AddSlotItem(uiSlotItem);
        }
    }

    public void RemoveSlotItem(UiSlotItem uiSlotItem)
    {
        uiSlotItems.Remove(uiSlotItem);
    }

    private Iteo CreateRanIteo(int minId = 1, int maxId = 5, int min = 1, int max = 10)
    {
        Iteo iteo = new Iteo
        {
            id = UnityEngine.Random.Range(minId, maxId),
            duraCount = UnityEngine.Random.Range(min, max)
        };
        return iteo;
    }

    private Iteo CreateIteo(int id = 1, int durcount = 10, bool isRanCount = true)
    {
        Iteo iteo = new Iteo { id = id };
        if (isRanCount)
        {
            iteo.duraCount = UnityEngine.Random.Range(1, durcount);
        }
        else
        {
            iteo.duraCount = durcount;
        }
        return iteo;
    }
    #endregion
}

public enum DropType
{
    Any,
    TypeA,
    TypeB,
    TypeC
}