using System.Collections.Generic;
using UnityEngine;

public class DragDropBase : MonoBehaviour
{
    private const int maxStack = 12;
    [SerializeField] private UiSlot uiSlotPrefab;
    [SerializeField] private UiSlotItem uiSlotItemPrefab;
    [SerializeField] private List<UiSlotItem> uiSlotItems = new List<UiSlotItem>(); //make it private
    protected UiSlot[] uiSlots;

    protected void CreateUiSlots(int slotCount, Transform parent)
    {
        uiSlots = new UiSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            uiSlots[i] = InstantiateUiSlot(i, parent);
        }
        //uiSlots[0].isDropable = false;
    }

    private UiSlot InstantiateUiSlot(int id, Transform parent)
    {
        UiSlot uiSlot = Instantiate(uiSlotPrefab, parent);
        uiSlot.OnSlotDrop += OnSlotDrop;
        uiSlot.id = id;
        uiSlot.name = "UiSlot" + id.ToString();
        return uiSlot;
    }

    protected bool CheckIfIteoAvailable(Item iteo)
    {
        int available = 0;
        for (int i = 0; i < uiSlotItems.Count; i++)
        {
            if (uiSlotItems[i].IteoId == iteo.id)
            {
                available += uiSlotItems[i].IteoDuraCount;
                if (available >= iteo.duraCount)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected Item RemoveSlotItemReturnNeeded(Item iteoToRemoved)
    {
        Item iteo = new Item(iteoToRemoved.id, iteoToRemoved.duraCount);
        //reverse forloop as removing items from list in loop is not good
        for (int i = uiSlotItems.Count - 1; i >= 0; i--)
        {
            if (uiSlotItems[i].IteoId == iteo.id)
            {
                int remaining = uiSlotItems[i].DecrementDuraCount(iteo.duraCount);
                iteo.duraCount = remaining;
                if (remaining > 0)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
        }
        if (iteo.duraCount > 0)
        {
            print("not in inu " + iteo.duraCount);
            return iteo;
        }
        else
        {
            return null;
        }
    }

    protected Item AddSlotItemReturnRemaining(Item iteoToAdd)
    {
        Item iteo = new Item(iteoToAdd.id, iteoToAdd.duraCount);
        for (int i = 0; i < uiSlotItems.Count; i++)
        {
            if (uiSlotItems[i].IteoId == iteo.id)
            {
                if (uiSlotItems[i].IsStackMax())
                {
                    continue;
                }
                iteo.duraCount = uiSlotItems[i].IncrementDuraCount(iteo.duraCount);
                if (iteo.duraCount == 0)
                {
                    return null;
                }
            }
        }
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i].transform.childCount == 0)
            {
                UiSlotItem uiSlotItem = Instantiate(uiSlotItemPrefab, uiSlots[i].transform);
                uiSlotItem.Init(maxStack, new InventoryIteo(iteo.id, 0, i));
                uiSlotItem.OnSlotItemDrop += OnSlotItemDrop;
                iteo.duraCount = uiSlotItem.IncrementDuraCount(iteo.duraCount);
                AddSlotItem(uiSlotItem);
                if (iteo.duraCount == 0)
                {
                    return null;
                }
            }
        }
        print("No empty slots still have " + iteo.duraCount);
        return iteo;
    }


    #region Actions from Slot and SlotItems
    private void OnSlotDrop(UiSlot uiSlot, UiSlotItem uiSlotItem)
    {
        if (uiSlot.transform.childCount == 0)
        {
            uiSlotItem.SetParent(uiSlot.transform, uiSlot.id);
            AddSlotItem(uiSlotItem);
        }
    }

    private void OnSlotItemDrop(UiSlotItem dropedItem, UiSlotItem dragedItem)
    {
        if (dragedItem.IteoId == dropedItem.IteoId)
        {
            dragedItem.DecrementDuraCountDragDrop(dropedItem.IncrementDuraCount(dragedItem.IteoDuraCount));
        }
        else
        {
            DragDropBase tempDragDropBase = dragedItem.lastDragDropBase;
            Transform tempTransform = dragedItem.lastParent;
            int tempInuSlotId = dragedItem.IteoInuSlotId;

            dropedItem.RemovedFromThis();

            dragedItem.lastDragDropBase = dropedItem.lastDragDropBase;
            dragedItem.lastParent = dropedItem.lastParent;
            dragedItem.IteoInuSlotId = dropedItem.IteoInuSlotId;

            dropedItem.AddInThis(tempDragDropBase, tempTransform, tempInuSlotId);
        }
    }
    #endregion


    #region HelperFunctions
    public void AddSlotItem(UiSlotItem uiSlotItem)
    {
        uiSlotItems.Add(uiSlotItem);
        uiSlotItem.UpdateText();
    }

    public void RemoveSlotItem(UiSlotItem uiSlotItem)
    {
        uiSlotItems.Remove(uiSlotItem);
        uiSlotItem.UpdateText();
    }

    protected Item CreateRanIteo(int minId = 1, int maxId = 5, int min = 1, int max = 10)
    {
        Item iteo = new Item(UnityEngine.Random.Range(minId, maxId), UnityEngine.Random.Range(min, max));
        return iteo;
    }

    private Item CreateIteo(int id = 1, int durcount = 10, bool isRanCount = true)
    {
        Item iteo = new Item(id, durcount);
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