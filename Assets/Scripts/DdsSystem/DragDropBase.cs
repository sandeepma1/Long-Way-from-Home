using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DragDropBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] protected FurnitureType slotItemsType = FurnitureType.None;
    [SerializeField] protected UiSlot[] uiSlots;
    protected static List<UiSlotItem> uiSlotItems = new List<UiSlotItem>(); //make it private
    protected PlayerSaveFurniture furniture;
    protected bool areUiSlotsCreated = false;
    private UiSlot lastClickedUiSlot;
    protected virtual void Start()
    {
        MasterSave.RequestSaveData += RequestSaveData;
    }

    private void OnDestroy()
    {
        MasterSave.RequestSaveData -= RequestSaveData;
    }

    protected void LoadFurnitureData(List<SlotItems> slotItems)
    {
        if (!areUiSlotsCreated)
        {
            CreateUiSlots();
        }
        for (int i = 0; i < slotItems.Count; i++)
        {
            int slotID = slotItems[i].invSlotId;
            SlotItems slotItem = new SlotItems(slotItems[i].item, slotID);
            UiSlotItem uiSlotItem = InstantiateUiSlotItem(uiSlots[slotID].transform, slotItem);
            uiSlotItem.OnSlotItemDrop += OnSlotItemDrop;
            AddSlotItem(uiSlotItem);
        }
    }

    protected virtual void RequestSaveData()
    {
        //This will call all the base class requesting save.
        //print("called base");
    }

    protected virtual void CreateUiSlots()
    {
        print("virtual class called CreateUiSlots");
        //Used to call all base class to create their own slots
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
        uiSlot.OnUiSlotClicked += OnUiSlotClicked;
        return uiSlot;
    }

    private void OnUiSlotClicked(UiSlot uiSlot, Item item)
    {
        uiSlot.SetSpriteToSelected();
        if (lastClickedUiSlot != null)
        {
            lastClickedUiSlot.SetSpriteToNormal();
        }
        lastClickedUiSlot = uiSlot;

        if (itemNameText != null)
        {
            if (item == null)
            {
                itemNameText.text = "";
            }
            else
            {
                itemNameText.text = InventoryItemsDatabase.GetSlugById((int)item.id);
            }
        }
    }

    protected static bool CheckIfItemAvailable(Item item)
    {
        int available = 0;
        for (int i = 0; i < uiSlotItems.Count; i++)
        {
            if (uiSlotItems[i].ItemId == item.id.Value)
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
            if (uiSlotItems[i].ItemId == item.id.Value)
            {
                int remaining = uiSlotItems[i].DecrementDuraCount((int)item.duraCount);
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
        //print("DDS name" + gameObject.name + " parent " + transform.parent.name);
        Item item = new Item(itemToAdd.id, itemToAdd.duraCount);
        for (int i = 0; i < uiSlotItems.Count; i++)
        {
            if (uiSlotItems[i].ItemId == item.id.Value)
            {
                if (uiSlotItems[i].IsStackMax())
                {
                    continue;
                }
                item.duraCount = uiSlotItems[i].IncrementDuraCount((int)item.duraCount);
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
                UiSlotItem uiSlotItem = InstantiateUiSlotItem(uiSlots[i].transform, new SlotItems((int)item.id, 0, i));
                uiSlotItem.OnSlotItemDrop += OnSlotItemDrop;
                item.duraCount = uiSlotItem.IncrementDuraCount((int)item.duraCount);
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
        uiSlotItem.Init(slotItems);
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
    Weapon,
    Armor,
    Food
}

public enum FurnitureType
{
    None,
    Inventory,
    Chest,
    Furnace
    //TODO: look here to refactor names, this can be Workstation, Storage, Decoration, Transport, Defence
}