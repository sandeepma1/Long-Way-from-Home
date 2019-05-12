using System;
using System.Collections.Generic;
using UnityEngine;

public class DragDropBase : MonoBehaviour
{
    public static Action<PlayerSaveSlotItem> OnSlotItemDataSend;
    private const int maxStack = 12;
    [SerializeField] private SlotItemsType slotItemsType = SlotItemsType.None;
    [SerializeField] private List<UiSlotItem> uiSlotItems = new List<UiSlotItem>(); //make it private
    [SerializeField] protected UiSlot[] uiSlots;
    private PlayerSaveSlotItem playerItem;
    protected bool areUiSlotsCreated = false;

    private void Awake()
    {
        MasterSave.RequestSaveData += RequestSaveData;
        MasterSave.OnSlotItemsLoaded += PlayerItemLoadedData;
    }

    protected void InitRandomItems()
    {
        AddSlotItemReturnRemaining(CreateRanItem());
        AddSlotItemReturnRemaining(CreateRanItem());
        AddSlotItemReturnRemaining(CreateRanItem());
        AddSlotItemReturnRemaining(CreateRanItem());
        AddSlotItemReturnRemaining(CreateRanItem());
    }

    protected virtual void CreateUiSlotsIfNotCreated()
    {
        print("Ui slots created");
    }

    private void PlayerItemLoadedData(List<PlayerSaveSlotItem> playerSaveItem)
    {
        if (!areUiSlotsCreated)
        {
            CreateUiSlotsIfNotCreated();
        }
        if (playerSaveItem == null)
        {
            InitRandomItems();
        }
        else
        {
            for (int i = 0; i < playerSaveItem.Count; i++)
            {
                if (playerSaveItem[i].slotItemsType == slotItemsType)
                {
                    LoadThisData(playerSaveItem[i].slotItems);
                }
            }
        }
    }

    protected void LoadThisData(List<SlotItems> slotItems)
    {
        for (int i = 0; i < slotItems.Count; i++)
        {
            int slotID = slotItems[i].invSlotId;
            SlotItems slotItem = new SlotItems(slotItems[i].item, slotID);
            UiSlotItem uiSlotItem = InstantiateUiSlotItem(uiSlots[slotID].transform, slotItem);
            uiSlotItem.OnSlotItemDrop += OnSlotItemDrop;
            AddSlotItem(uiSlotItem);
        }
    }

    private void RequestSaveData()
    {
        List<SlotItems> slotItems = new List<SlotItems>();
        for (int i = 0; i < uiSlotItems.Count; i++)
        {
            slotItems.Add(uiSlotItems[i].slotItem);
        }
        playerItem = new PlayerSaveSlotItem(slotItemsType, 0, slotItems);
        OnSlotItemDataSend?.Invoke(playerItem);
    }

    protected void CreateUiSlots(int slotCount, Transform parent)
    {
        uiSlots = new UiSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            uiSlots[i] = InstantiateUiSlot(i, parent);
        }
        areUiSlotsCreated = true;
    }

    private UiSlot InstantiateUiSlot(int id, Transform parent)
    {
        UiSlot uiSlot = Instantiate(PrefabBank.uiSlotPrefab, parent);
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
                UiSlotItem uiSlotItem = InstantiateUiSlotItem(uiSlots[i].transform, new SlotItems(item.id, 0, i));
                //UiSlotItem uiSlotItem = Instantiate(uiSlotItemPrefab, uiSlots[i].transform);
                //uiSlotItem.Init(maxStack, new InventoryItem(item.id, 0, i));
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

    private UiSlotItem InstantiateUiSlotItem(Transform parent, SlotItems slotItems)
    {
        UiSlotItem uiSlotItem = Instantiate(PrefabBank.uiSlotItemPrefab, parent);
        uiSlotItem.Init(maxStack, slotItems);
        return uiSlotItem;
    }


    #region Actions from Slot and SlotItems
    protected void OnSlotDrop(UiSlot uiSlot, UiSlotItem uiSlotItem)
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
            int tempInuSlotId = dragedItem.ItemSlotId;

            dropedItem.RemovedFromThis();

            dragedItem.lastDragDropBase = dropedItem.lastDragDropBase;
            dragedItem.lastParent = dropedItem.lastParent;
            dragedItem.ItemSlotId = dropedItem.ItemSlotId;

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

public enum SlotItemsType
{
    None,
    Inventory,
    Chest,
    Furnace
}