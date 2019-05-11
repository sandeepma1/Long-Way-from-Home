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

    protected bool CheckIfItemAvailable(Item item)
    {
        int available = 0;
        for (int i = 0; i < uiSlotItems.Count; i++)
        {
            if (uiSlotItems[i].ItemId == item.id)
            {
                available += uiSlotItems[i].ItemDuraCount;
                if (available >= item.duraCount)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected Item RemoveSlotItemReturnNeeded(Item itemToRemoved)
    {
        Item item = new Item(itemToRemoved.id, itemToRemoved.duraCount);
        //reverse forloop as removing items from list in loop is not good
        for (int i = uiSlotItems.Count - 1; i >= 0; i--)
        {
            if (uiSlotItems[i].ItemId == item.id)
            {
                int remaining = uiSlotItems[i].DecrementDuraCount(item.duraCount);
                item.duraCount = remaining;
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
        if (item.duraCount > 0)
        {
            print("not in inu " + item.duraCount);
            return item;
        }
        else
        {
            return null;
        }
    }

    protected Item AddSlotItemReturnRemaining(Item itemToAdd)
    {
        Item item = new Item(itemToAdd.id, itemToAdd.duraCount);
        for (int i = 0; i < uiSlotItems.Count; i++)
        {
            if (uiSlotItems[i].ItemId == item.id)
            {
                if (uiSlotItems[i].IsStackMax())
                {
                    continue;
                }
                item.duraCount = uiSlotItems[i].IncrementDuraCount(item.duraCount);
                if (item.duraCount == 0)
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
                uiSlotItem.Init(maxStack, new InventoryItem(item.id, 0, i));
                uiSlotItem.OnSlotItemDrop += OnSlotItemDrop;
                item.duraCount = uiSlotItem.IncrementDuraCount(item.duraCount);
                AddSlotItem(uiSlotItem);
                if (item.duraCount == 0)
                {
                    return null;
                }
            }
        }
        print("No empty slots still have " + item.duraCount);
        return item;
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
        if (dragedItem.ItemId == dropedItem.ItemId)
        {
            dragedItem.DecrementDuraCountDragDrop(dropedItem.IncrementDuraCount(dragedItem.ItemDuraCount));
        }
        else
        {
            DragDropBase tempDragDropBase = dragedItem.lastDragDropBase;
            Transform tempTransform = dragedItem.lastParent;
            int tempInuSlotId = dragedItem.ItemInuSlotId;

            dropedItem.RemovedFromThis();

            dragedItem.lastDragDropBase = dropedItem.lastDragDropBase;
            dragedItem.lastParent = dropedItem.lastParent;
            dragedItem.ItemInuSlotId = dropedItem.ItemInuSlotId;

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

    protected Item CreateRanItem(int minId = 1, int maxId = 5, int min = 1, int max = 10)
    {
        Item item = new Item(UnityEngine.Random.Range(minId, maxId), UnityEngine.Random.Range(min, max));
        return item;
    }

    private Item CreateItem(int id = 1, int durcount = 10, bool isRanCount = true)
    {
        Item item = new Item(id, durcount);
        if (isRanCount)
        {
            item.duraCount = UnityEngine.Random.Range(1, durcount);
        }
        else
        {
            item.duraCount = durcount;
        }
        return item;
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